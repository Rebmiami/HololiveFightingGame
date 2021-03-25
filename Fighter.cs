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

namespace HololiveFightingGame
{
	public class Fighter : Entity
	{
		public bool grounded;
		public int coyote;
		public int damage; // 999.9 to 0.0 by default. Div by 10 to get damage shown on screen

		public int jumps;

		public int moveTimer;

		public MoveRunner moveRunner;

		public int ID;
		public bool keyboard = false;

		public int launchTimer; // Launch frames where player has no control
		public int invFrames; // The player is given some frames of invulnerability after getting hit to prevent the same attack from hitting multiple times when it shouldn't
		// TODO: Prevent the player from being hit by the same hitbox repeatedly somehow

		public int direction = 1;

		public string character = "Pekora";

		public List<Attack> attacks;

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

			moveRunner?.Update(moveTimer);

			// Takes player inputs to perform actions
			if (launchTimer == 0)
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
					moveTimer = 16;
				}
			}
			else
			{
				launchTimer--;
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

		public Attack? Update_Hits() // Process damage dealt to the player and resolve conflicts.
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

		public void Update_PostHit(Attack? attack) // After conflicts are resolved and a winning attack is selected, apply the effects of the attack (damage, knockback, etc.)
		{
			if (attack != null)
			{
				Damage((Attack)attack);
				attacks.Clear();
			}
		}

		public void Update_Animation() // Sets animation frames and the direction the fighter's sprite should face
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

		public Fighter(int ID)
		{
			this.ID = ID;
			//collider = new Collider(new Rectangle());
			collider = new Collider(new Capsule(position, new Vector2(0, -26), 19));
			colliderOrigin = new Vector2(-1, -1);
			colliderOffset = Vector2.Zero;
			//Dimensions = new Vector2(38, 64);
			position = new Vector2(300, 0);
			grounded = true;
			GraphicsHandler.main.children["game"].children.Add("fighter_" + ID, new DrawObject(DrawObjectType.Sprite));
			drawObject = GraphicsHandler.main.children["game"].children["fighter_" + ID];
			drawObject.texture = new AnimatedSprite(Game1.testFighter, new Point(50, 80));
			Game1.jsonLoaderFilePath = @".\Content\Data\Fighters\" + character + @"\Animations.json";
			string json = System.IO.File.ReadAllText(Game1.jsonLoaderFilePath);
			((AnimatedSprite)drawObject.texture).animations = (Dictionary<string, Animation>)JsonSerializer.Deserialize(json, typeof(Dictionary<string, Animation>));
			((AnimatedSprite)drawObject.texture).SetAnimFrames();
			drawObject.frame = "neutral";
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

			public int priority; // Used to resolve conflicts. If two attacks with the same priority conflict, they will cancel out.

			public Fighter attacker; // Set attacker to null if the damage was not caused by a player (directly or indirectly)

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
		None,
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
		TUltimate,
	}
}
