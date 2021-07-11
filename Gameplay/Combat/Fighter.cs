using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using HololiveFightingGame.Input;
using HololiveFightingGame.Graphics;
using System.Text.Json;
using HololiveFightingGame.Loading;
using HololiveFightingGame.Graphics.CapsuleShader;
using HololiveFightingGame.Gameplay;
using HololiveFightingGame.Gameplay.Collision;
using HololiveFightingGame.Loading.Serializable;

namespace HololiveFightingGame.Gameplay.Combat
{
    public class Fighter : Entity
	{
		public bool grounded;
		// If true, the fighter is grounded, otherwise, they are airborne.
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
		// Used by the editor only. NOT related to hitstun.
		public bool takeInputs = true;

		public HurtBody body;

		public FighterController controller;

		public AirState airState;

		// Stats

		/// <summary>
		/// The number of extra jumps past the initial jump.
		/// </summary>
		public int extraJumps;

		/// <summary>
		/// The velocity imparted onto the fighter when they leave the ground via a full jump.
		/// </summary>
		public float jumpForce;

		/// <summary>
		/// The velocity imparted onto the fighter when they jump mid-air.
		/// </summary>
		public float extraJumpForce;

		/// <summary>
		/// The amount of horizontal velocity lost per frame while airborne.
		/// </summary>
		public float airResistance;

		/// <summary>
		/// The amount of horizontal velocity lost per frame while grounded.
		/// </summary>
		public float traction;

		/// <summary>
		/// The amount of horizontal force pulling the fighter down.
		/// </summary>
		public float gravity;

		public float airSpeed;
		public float groundSpeed;
		public float fallSpeed;
		public float fastFallSpeed;

		public float airAcceleration;
		public float groundAcceleration;

		/// <summary>
		/// Determines how far fighters are launched - fighters of half the weight take twice the knockback and vice versa.
		/// </summary>
		public int weight;

		public override void Update()
		{
			if (!Game1.gameState.stage.stageBounds.Intersects(Hitbox()) && takeInputs)
			{
				position = new Vector2(300, 0);
				velocity = Vector2.Zero;
				grounded = true;
				drawObject.frame = "stand";
				damage = 0;
				launchTimer = 0;
				airState = AirState.Grounded;
			}

			velocity.Y += gravity;
			// TODO: Change the way friction is handled.
			if (controller.direction4.X == 0)
			{
				velocity.X *= grounded ? 1 - traction : 1 - airResistance;
			}

			if (Math.Abs(velocity.X) < 0.05)
            {
				velocity.X = 0;
            }

			if (launchTimer == 0)
			{
				velocity.X += KeybindHandler.ControlDirection(keyboard, ID).X * (grounded ? groundAcceleration : airAcceleration);
			}


			if (launchTimer == 0)
			{
				
					Vector2 maxVelocity = new Vector2(grounded ? groundSpeed : airSpeed, airState == AirState.FastFall ? fastFallSpeed : fallSpeed);
					velocity.X = Math.Clamp(velocity.X, -maxVelocity.X, maxVelocity.X);
					velocity.Y = Math.Min(velocity.Y, maxVelocity.Y);
			}

			// Takes player inputs to perform actions
			if (launchTimer == 0)
			{
				// TODO: Overhaul input handling
				// It should allow CPU fighters and give the user more options in-editor
				// Perhaps add a "controller" object?
				if (takeInputs)
					Update_Inputs();
			}
			else
			{
				launchTimer--;
				if (launchTimer == 0)
                {
					// TODO: Allow weaker attacks to cause flinching instead of launch.
					airState = AirState.Tumble;
                }
			}

			if (moveRunner != null)
			{
				Vector2 oldVelocity = velocity;
				moveRunner.Update(moveTimer);
				if (moveRunner.motion != null)
					velocity = moveRunner.motion * new Vector2(direction, 1);
				velocity = Vector2.Lerp(velocity, oldVelocity, moveRunner.data.Sustain);
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

			base.Update();

			// Aligns fighter's hurtbody to its current position.
			body.foot = new Vector2(Hitbox().Center.X, Hitbox().Bottom);

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
					airState = AirState.Fall;
				}
				else
				{
					grounded = false;
					jumps = 1;
					airState = AirState.Grounded;
				}
			}

			// Failsafes
			if (moveRunner != null && moveRunner.move == null)
			{
				moveRunner = null;
			}
			if (moveRunner == null)
            {
				moveTimer = 0;
            }


			if (moveRunner != null && moveRunner.move.Data.Aerial && grounded)
			{
				// TODO: Landing lag
				moveRunner = null;
			}

			// Checks if an attack is hitting an opponent and, if so, tells the opponent to be hit by the attack.
			if (moveRunner != null)
			{
				for (int i = 0; i < moveRunner.move.hitboxes.Length; i++)
				{
					AttackHitbox attackHitbox = moveRunner.move.hitboxes[i];
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

					// if (Game1.showHitboxes)
					// {
					// 	Capsule capsule1 = capsule;
					// 	capsule1.origin += position + new Vector2(Hitbox().Width / 2, 0);
					// 	CapsuleRenderer.capsuleShaders.Add(new CapsuleShaderData(capsule1, Color.Yellow * 0.3f));
					// }

					capsule.origin = Center + capsule.origin;

					for (int j = 0; j < Game1.gameState.fighters.Length; j++)
					{
						Fighter target = Game1.gameState.fighters[j];

						// Remember that aerial and grounded mean exclusive to such - an aerial hitbox is defined by its inability to hit grounded opponents and vice versa
						// If a hitbox should be able to hit any opponent, set both to "false".
						if (attackHitbox.aerial && target.grounded)
						{
							continue;
						}
						else if (attackHitbox.grounded && !target.grounded)
						{
							continue;
						}

						if (ID != j && target.invFrames == 0)
						{
							Capsule capsule1 = capsule;
							capsule1.origin -= target.body.foot;

							AttackHitbox checkHitbox = (AttackHitbox)attackHitbox.Clone();
							checkHitbox.collider = new Collider(capsule1);

							Attack attack = target.body.CheckHits(checkHitbox, this);
							// Attack attack = new Attack(attackHitbox, this);
							if (attack != null)
							{
								attack.knockback.X *= direction;
								target.attacks.Add(attack);
							}

							// if (Game1.showHitboxes)
							// {
							// 	foreach (Hurtbox hurtbox in body.body)
							// 	{
							// 		CapsuleRenderer.capsuleShaders.Add(new CapsuleShaderData(capsule1, Color.Orange * 0.3f, false));
							// 	}
							// 
							// 	foreach (Hurtbox hurtbox in target.body.body)
							// 	{
							// 		Capsule capsule2 = hurtbox.collider.Capsule;
							// 		CapsuleRenderer.capsuleShaders.Add(new CapsuleShaderData(capsule2, Color.White * 0.3f, false));
							// 	}
							// }
						}
					}
				}
			}

			if (moveTimer > 0)
			{
				moveTimer--;
				if (moveTimer == 0)
				{
					if (!grounded && moveRunner.data.SpecialFall)
					{
						airState = AirState.SpecialFall;
					}

					moveRunner = null;
				}
			}

			// if (Game1.showHitboxes)
			// {
			// 	foreach (Hurtbox hurtbox in body.body)
			// 	{
			// 		Capsule capsule = hurtbox.collider.Capsule;
			// 		capsule.origin += position + new Vector2(Hitbox().Width / 2, 0);
			// 		CapsuleRenderer.capsuleShaders.Add(new CapsuleShaderData(capsule, Color.White * 0.3f));
			// 	}
			// 	CapsuleRenderer.QuickPoint(position, Color.Red);
			// 	CapsuleRenderer.QuickPoint(Bottom, Color.Green);
			// }
		}

		public void Update_Inputs()
		{
			controller.Update();

			if (KeybindHandler.TapJump(keyboard, ID) && jumps < extraJumps + 1 && moveTimer == 0 && airState != AirState.SpecialFall)
			{
				if (jumps == 0)
				{
					velocity.Y = -jumpForce;
					airState = AirState.Jump;
				}
				else
				{
					velocity.Y = -extraJumpForce;
					airState = AirState.EJump;
				}
				coyote = 0;
				grounded = false;
				jumps++;
			}

			if (controller.direction4 == new Vector2(0, 1) && controller.flick != 0 && velocity.Y > 0)
            {
				airState = AirState.FastFall;
				velocity.Y = fastFallSpeed;
            }

			// Initialize moves based on inputs

			// TODO: Allow some moves to be interrupted early
			if (moveTimer == 0 && airState != AirState.SpecialFall)
			{
				string move = "None";
				string dir = "N";
				string type = "None";

				if (KeybindHandler.TapAtkNormal(keyboard, ID))
				{
					if (grounded)
					{
						if (controller.flick != 0)
						{
							type = "Finisher";
						}
						else
						{
							type = "Normal";
						}
					}
					else
					{
						type = "Aerial";
					}
				}

				if (KeybindHandler.TapAtkSpecial(keyboard, ID))
				{
					type = "Special";
				}

				if (type != "None")
				{
					controller.direction4.Deconstruct(out float x, out float y);
					(float, float) dirTuple = (x, y);
					dirTuple.Item1 *= direction;

					switch (dirTuple)
					{
						case (1, 0):
							dir = "F";
							break;
						case (-1, 0):
							dir = "B";
							break;
						case (0, 1):
							dir = "D";
							break;
						case (0, -1):
							dir = "U";
							break;
					}


					switch (type)
					{
						case "Normal":
							switch (dir)
							{
								case "N":
									move = "NeutralA";
									break;

								case "U":
									move = "UpA";
									break;

								case "D":
									move = "DownA";
									break;

								case "F":
								case "B":
									move = "SideA";
									break;
							}
							break;

						case "Dash":


							break;
						case "Aerial":
							switch (dir)
							{
								case "N":
									move = "NAir";
									break;

								case "U":
									move = "UpAir";
									break;

								case "D":
									move = "DownAir";
									break;

								case "F":
									move = "FAir";
									break;

								case "B":
									move = "BackAir";
									break;
							}

							break;
						case "Finisher":
							switch (dir)
							{
								case "U":
									move = "UFinish";
									break;
							
								case "D":
									move = "DFinish";
									break;
							
								case "F":
								case "B":
									move = "SFinish";
									break;
							}

							break;
						case "Special":
							switch (dir)
							{
								case "N":
									move = "NeutralB";
									break;
							
								case "U":
									move = "UpB";
									break;
							
								case "D":
									move = "DownB";
									break;
							
								case "F":
								case "B":
									move = "SideB";
									break;
							}
							break;

						case "Taunt":
							switch (dir)
							{
								case "U":
									move = "UTaunt";
									break;

								case "D":
									move = "DTaunt";
									break;

								case "F":
								case "B":
									move = "STaunt";
									break;
							}

							break;
					}

					// TODO: Allow some moves to select which moves they can be cancelled into
					// TODO: Give moves control over which phase is the first (change "_0")

					if (move != "None")
					{
						moveRunner = new MoveRunner(FighterLoader.moves[character][move + "_0"]);
						moveTimer = moveRunner.data.MoveDuration;
					}
				}
			}
		}

		/// <summary>
		/// Process damage dealt to the player and resolve conflicts.
		/// </summary>
		/// <returns></returns>
		public Attack Update_Hits()
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
		public void Update_PostHit(Attack attack)
		{
			if (attack != null)
			{
				Damage(attack);
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
				if (Math.Abs(KeybindHandler.ControlDirection(keyboard, ID).X) > 0.1f)
				{
					direction = Math.Sign(KeybindHandler.ControlDirection(keyboard, ID).X);

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
			drawObject.position = Bottom;

			AnimationSetData.AnimationData animationData = FighterLoader.fighterData[character].animationData[((AnimatedSprite)drawObject.texture).Playing.AnimID];
			Vector2 foot = animationData.Foot;
			if (direction == -1)
			{
				foot.X *= -1;
				foot.X += animationData.FrameSize.X;
			}
			drawObject.position -= foot;


			Game1.uiHandler.damages[ID] = damage;
		}

		public void AddCapsules()
		{
			if (moveRunner != null)
			{
				for (int i = 0; i < moveRunner.move.hitboxes.Length; i++)
				{
					AttackHitbox attackHitbox = moveRunner.move.hitboxes[i];
					if (!moveRunner.enabled[i])
					{
						continue;
					}
					Collider collider = attackHitbox.collider;
					Capsule capsule = collider.Capsule;
					capsule.origin += moveRunner.pos[i];
					if (direction == 1)
					{
						capsule.origin.X *= -1;
						capsule.length.X *= -1;
					}

					Capsule capsule1 = capsule;
					capsule1.origin += position + new Vector2(Hitbox().Width / 2, 0);
					CapsuleRenderer.capsuleShaders.Add(new CapsuleShaderData(capsule1, Color.Yellow * 0.3f));
				}
			}

			foreach (Hurtbox hurtbox in body.body)
			{
				Capsule capsule = hurtbox.collider.Capsule;
				capsule.origin += position + new Vector2(Hitbox().Width / 2, 0);
				CapsuleRenderer.capsuleShaders.Add(new CapsuleShaderData(capsule, Color.White * 0.3f));
			}
			CapsuleRenderer.QuickPoint(position, Color.Red);
			CapsuleRenderer.QuickPoint(Bottom, Color.Green);
		}

		public Fighter(int ID, string character)
		{
			// Moves are loaded in FighterLoader.cs, not here.

			this.ID = ID;
			this.character = character;

			collider = new Collider(new Capsule(position, new Vector2(0, -26), 19));
			colliderOrigin = new Vector2(1, 1);
			colliderOffset = new Vector2(0, 0);
			//Dimensions = new Vector2(38, 64);

			position = new Vector2(300, 0);
			grounded = true;

			GraphicsHandler.main.children["game"].children.Add("fighter_" + ID, new DrawObject(DrawObjectType.Sprite));
			drawObject = GraphicsHandler.main.children["game"].children["fighter_" + ID];

			drawObject.frame = "neutral0";
			attacks = new List<Attack>();

			body = new HurtBody();
			body.body.Add(new Hurtbox(collider.Capsule));
			controller = new FighterController(this, ProfileBinder.profile);

			// Setup stats
			FighterStats stats = FighterLoader.fighterData[character].stats;

			extraJumps = stats.EJumps;
			jumpForce = stats.JumpForce;
			extraJumpForce = stats.EJumpForce;
			airResistance = stats.AirResistance;
			traction = stats.Traction;
			gravity = stats.Gravity;

			airSpeed = stats.AirSpeed;
			groundSpeed = stats.GroundSpeed;
			fallSpeed = stats.FallSpeed;
			fastFallSpeed = stats.FastFallSpeed;

			airAcceleration = stats.AirAccel;
			groundAcceleration = stats.GroundAccel;

			weight = stats.Weight;
		}

		public void Damage(Attack attack)
		{
			// 1 unit of knockback growth = 1 unit of knockback per 100% damage
			Vector2 knockback = attack.attackHitbox.LaunchAngle * (1 + damage / 1000f * attack.attackHitbox.kbScaling) * (1 / (weight / 100f));
			knockback.X *= attack.attacker.direction;
			damage = Math.Min(attack.attackHitbox.damage + damage, 9999);
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
				airState = AirState.Launch;
			}
			invFrames = 6;
			moveRunner = null;
			moveTimer = 0;
		}
	}

	public enum MoveType
	{
		NeutralA,
		Dash,
		SideA,
		UpA,
		DownA,
		NAir,
		FAir,
		BackAir,
		DownAir,
		UpAir,
		NeutralB,
		SideB,
		UpB,
		DownB,
		UFinish,
		DFinish,
		SFinish,
		Guard,
		Sidestep,
		Roll,
		AirDodge,
		UTaunt,
		DTaunt,
		STaunt,
	}

	public enum AirState
	{
		Grounded,
		Fall,
		Jump,
		EJump,
		SpecialFall,
		Launch,
		Tumble,
		FastFall
	}
}
