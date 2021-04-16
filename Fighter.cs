using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using HololiveFightingGame.Input;
using HololiveFightingGame.Collision;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Combat;
using System.Text.Json;
using HololiveFightingGame.Loading;

namespace HololiveFightingGame
{
	public class Fighter : Entity
	{
		public bool grounded;
		// If true, the fighter is grounded, otherwise, they are aerial.
		public int coyote;
		// Frames of coyote time.
		public int damage; 
		// 999.9 to 0.0 by default. Div by 10 to get damage shown on screen.

		public int jumps;

		public int moveTimer;

		public MoveRunner moveRunner;

		public int ID;
		public bool keyboard = false;

		public int launchTimer;
		// Launch frames where player has no control
		public int invFrames;
		// The player is given some frames of invulnerability after getting hit to prevent the same attack from hitting multiple times when it shouldn't
		// TODO: Prevent the player from being hit by the same hitbox repeatedly somehow

		public int direction = 1;

		public string character = "Pekora";

		public List<Attack> attacks;

		// This is temporary.
		public bool takeInputs = true;

		public override void Update()
		{
			if (!Game1.gameState.stage.stageBounds.Intersects(Hitbox()))
			{
				position = new Vector2(300, 0);
				velocity = Vector2.Zero;
				grounded = true;
				drawObject.frame = "stand";
				damage = 0;
				launchTimer = 0;
			}

			velocity.Y += 0.5f;
			velocity.X *= grounded ? 0.8f : 0.95f;
			if (launchTimer == 0)
			{
				velocity.X += KeybindHandler.HoldHorizMove(keyboard, ID);
			}
			Vector2 maxVelocity = new Vector2(6, 10);
			if (launchTimer == 0)
			{
				velocity = Vector2.Clamp(velocity, -maxVelocity, maxVelocity);
			}

			// Takes player inputs to perform actions
			if (launchTimer == 0)
			{
				// TODO: Overhaul input handling
				// It should allow CPU fighters and give the user more options in-editor
				if (takeInputs)
					Update_Inputs();
			}
			else
			{
				launchTimer--;
			}

			if (moveRunner != null)
			{
				Vector2 oldVelocity = velocity;
				moveRunner.Update(moveTimer);
				if (moveRunner.motion != null)
					velocity = moveRunner.motion * new Vector2(direction, 1);
				velocity = Vector2.Lerp(velocity, oldVelocity, moveRunner.data.Sustain);
			}

			base.Update();

			Rectangle stageCollider = Game1.gameState.stage.collider.Rectangle;

			if (collider.Intersects(stageCollider))
			{
				Rectangle colliderBottom = new Rectangle(stageCollider.Left + 12, stageCollider.Top + stageCollider.Height - 12, stageCollider.Width - 24, 12);
				Rectangle colliderLeft = new Rectangle(stageCollider.Left, stageCollider.Top, 12, stageCollider.Height);
				if (position.Y + Dimensions.Y < stageCollider.Top + 24)
				{
					grounded = true;
					coyote = 7;
				}
				else if (Hitbox().Intersects(colliderBottom))
				{
					position.Y = stageCollider.Bottom;
				}
				else
				{
					if (Hitbox().Intersects(colliderLeft))
					{
						position.X = stageCollider.Left - Dimensions.X;
					}
					else
					{
						position.X = stageCollider.Right;
					}
				}
			}

			if (grounded)
			{
				velocity.Y = 0;
				position.Y = Game1.gameState.stage.collider.Rectangle.Top - Dimensions.Y + 1;
				if (coyote > 0)
				{
					coyote--;
					jumps = 0;
				}
				else
				{
					grounded = false;
					jumps = 1;
				}
			}

			

			// Checks if an attack is hitting an opponent and, if so, tells the opponent to be hit by the attack.
			if (moveTimer > 0)
			{
				for (int i = 0; i < FighterLoader.moves[character][moveRunner.name].hitboxes.Length; i++)
				{
					AttackHitbox attackHitbox = FighterLoader.moves[character][moveRunner.name].hitboxes[i];
					if (!moveRunner.enabled[i])
					{
						continue;
					}
					Collider collider = attackHitbox.collider;
					Capsule capsule = collider.Capsule;
					capsule.origin += moveRunner.pos[i];
					if (direction == -1)
					{
						capsule.origin.X *= -1;
						capsule.length.X *= -1;
					}
					capsule.origin = Center + capsule.origin;

					for (int j = 0; j < Game1.gameState.fighters.Length; j++)
					{
						if (ID != j && Game1.gameState.fighters[j].collider.Capsule.Intersects(capsule) && Game1.gameState.fighters[j].invFrames == 0)
						{
							Attack attack = new Attack
							{
								damage = attackHitbox.damage,
								knockback = attackHitbox.LaunchAngle
							};
							attack.knockback.X *= direction;
							Game1.gameState.fighters[j].attacks.Add(attack);
						}
					}
				}
			}

			if (moveTimer > 0)
			{
				moveTimer--;
				if (moveTimer == 0)
				{
					moveRunner = null;
				}
			}

			// Pushes fighters apart to prevent overlapping
			for (int i = 0; i < Game1.gameState.fighters.Length; i++)
			{
				Fighter fighter = Game1.gameState.fighters[i];
				if (ID != i && fighter.Hitbox().Intersects(Hitbox()))
				{
					float direction = Math.Sign(position.X - fighter.position.X) * 0.4f;
					velocity.X += direction;
					fighter.velocity.X -= direction;
				}
			}
		}

		public void Update_Inputs()
		{
			if (KeybindHandler.TapJump(keyboard, ID) && jumps < 2)
			{
				if (jumps == 0)
				{
					velocity.Y = -10;
				}
				else
				{
					velocity.Y = -8;
				}
				coyote = 0;
				grounded = false;
				jumps++;
			}

			// TODO: Change this to accept more move types
			// Reworking keybinds may be necessary
			if (KeybindHandler.TapAtkNormal(keyboard, ID) && moveTimer == 0)
			{
				moveRunner = new MoveRunner(FighterLoader.moves[character]["NeutralA_0"]);
				moveTimer = moveRunner.data.MoveDuration;
			}

			if (KeybindHandler.TapAtkSecond(keyboard, ID) && moveTimer == 0)
			{
				moveRunner = new MoveRunner(FighterLoader.moves[character]["NeutralB_0"]);
				moveTimer = moveRunner.data.MoveDuration;
			}
		}

		/// <summary>
		/// Process damage dealt to the player and resolve conflicts.
		/// </summary>
		/// <returns></returns>
		public Attack? Update_Hits()
		{
			if (invFrames > 0)
			{
				invFrames--;
				attacks.Clear();
				return null;
			}

			if (attacks.Count == 0)
			{
				return null;
			}

			attacks.Sort();
			return attacks[0];
			// TODO: Loop through all incoming attacks and create a list of attackers to find potential conflicts
			// Sort incoming attacks by priority
			// Disregard all attacks with priority less than the highest priority attack

			// TODO: If two fighters are trying to use attacks with the same priority and the hitboxes collide, cancel the attacks.
		}

		/// <summary>
		/// After conflicts are resolved and a winning attack is selected, apply the effects of the attack (damage, knockback, etc.)
		/// </summary>
		/// <param name="attack"></param>
		public void Update_PostHit(Attack? attack)
		{
			if (attack != null)
			{
				Damage((Attack)attack);
				attacks.Clear();
			}
		}

		/// <summary>
		/// Sets animation frames and the direction the fighter's sprite should face.
		/// </summary>
		public void Update_Animation()
		{
			if (grounded)
			{
				if (Math.Abs(KeybindHandler.HoldHorizMove(keyboard, ID)) > 0.1f)
				{
					direction = Math.Sign(KeybindHandler.HoldHorizMove(keyboard, ID));

					((AnimatedSprite)drawObject.texture).SwitchAnimation("walk", 0);
				}
				else
				{
					((AnimatedSprite)drawObject.texture).SwitchAnimation("neutral", 0);
				}
			}
			else
			{
				((AnimatedSprite)drawObject.texture).SwitchAnimation("jump", 0);
			}

			if (moveRunner != null)
			{
				((AnimatedSprite)drawObject.texture).SwitchAnimation(moveRunner.name, 0);
				((AnimatedSprite)drawObject.texture).Playing.Frame = moveRunner.frame;
			}

			if (launchTimer > 0)
			{
				((AnimatedSprite)drawObject.texture).SwitchAnimation("launch", 0);
				direction = -Math.Sign(velocity.X);
			}
			drawObject.spriteEffects = direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			((AnimatedSprite)drawObject.texture).Update();
			drawObject.frame = ((AnimatedSprite)drawObject.texture).GetFrame();
			drawObject.Bottom = Bottom;
			Game1.uiHandler.damages[ID] = damage;
		}

		public Fighter(int ID, string character)
		{
			// Moves are loaded in FighterLoader.cs, not here.

			this.ID = ID;
			this.character = character;

			collider = new Collider(new Capsule(position, new Vector2(0, -26), 19));
			colliderOrigin = new Vector2(-1, -1);
			colliderOffset = Vector2.Zero;
			//Dimensions = new Vector2(38, 64);

			position = new Vector2(300, 0);
			grounded = true;

			// TODO: Offload this to another class so that fighter graphics in-editor can be handled differently
			GraphicsHandler.main.children["game"].children.Add("fighter_" + ID, new DrawObject(DrawObjectType.Sprite));
			drawObject = GraphicsHandler.main.children["game"].children["fighter_" + ID];
			

			drawObject.frame = "neutral0";
			attacks = new List<Attack>();
		}

		public void Damage(Attack attack)
		{
			Damage(attack.damage, attack.knockback);
		}

		public void Damage(int damage, Vector2 knockback)
		{
			knockback += knockback * (this.damage / 2000f);
			this.damage = Math.Min(this.damage + damage, 9999);
			launchTimer = (int)(Math.Abs(knockback.Y) * 2.5f);
			if (grounded)
			{
				velocity = knockback;
			}
			else
			{
				if (velocity.Y > 0)
				{
					velocity.Y += knockback.Y;
				}
				else
				{
					velocity.Y = knockback.Y;
				}
				velocity.X += knockback.X;
			}

			if (knockback.Y < 0)
			{
				grounded = false;
				coyote = 0;
			}
			invFrames = 6;
		}

		public struct Attack : IComparable
		{
			public Attack(int damage, Vector2 knockback, int priority = 1, Fighter attacker = null)
			{
				this.damage = damage;
				this.knockback = knockback;
				this.priority = priority;
				this.attacker = attacker;
			}

			public int damage;
			public Vector2 knockback;

			public int priority;
			// Used to resolve conflicts. If two attacks with the same priority conflict, they will cancel out.

			public Fighter attacker;
			// Set attacker to null if the damage was not caused by a player (directly or indirectly)

			public int CompareTo(object obj)
			{
				if (obj is Attack attack)
				{
					return attack.priority - priority;
				}
				throw new ArgumentException("An attempt was made to compare an Attack object with an object that is not an Attack.");
			}
		}
	}

	public enum MoveType
	{
		NeutralA,
		SideA,
		UpA,
		DownA,
		Dash,
		NeutralAir,
		ForwardAir,
		BackAir,
		DownAir,
		NeutralB,
		SideB,
		UpB,
		DownB,
		TDefend,
		TSide,
		TRecover,
		TFinisher1,
		TFinisher2,
		TFinisher3,
	}
}
