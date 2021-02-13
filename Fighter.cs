using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	public class Fighter : Entity
	{
		public bool grounded;
		public int coyote;
		public int damage; // 1000.0 to 0.0 by default. Div by 10 to get damage shown on screen

		public int jumps;

		public int moveTimer;

		public MoveType currentMove;

		public int ID;
		public bool keyboard = false;

		public int launchTimer; //Launch frames where player has no control

		public int direction = 1;

		public override void Update()
		{
			if (!Game1.gameState.stage.stageBounds.Intersects(Hitbox()))
			{
				position = new Vector2(300, 0);
				velocity = Vector2.Zero;
				grounded = true;
				drawObject.frame = "stand";
			}

			velocity.Y += 0.5f;
			velocity.X *= grounded ? 0.8f : 0.95f;
			velocity.X += Keybinds.HoldHorizMove(keyboard, ID);
			Vector2 maxVelocity = new Vector2(6, 10);
			if (launchTimer == 0)
			{
				velocity = Vector2.Clamp(velocity, -maxVelocity, maxVelocity);
			}

			base.Update();

			Rectangle stageCollider = Game1.gameState.stage.collider;

			if (Hitbox().Intersects(stageCollider))
			{
				//Rectangle colliderTop = new Rectangle(stageCollider.Left, stageCollider.Top, stageCollider.Width, 12);
				Rectangle colliderBottom = new Rectangle(stageCollider.Left + 12, stageCollider.Top + stageCollider.Height - 12, stageCollider.Width - 24, 12);
				Rectangle colliderLeft = new Rectangle(stageCollider.Left, stageCollider.Top, 12, stageCollider.Height);
				//Rectangle colliderRight = new Rectangle(stageCollider.Left + stageCollider.Width - 4, stageCollider.Top, 4, stageCollider.Height);
				if (position.Y + dimensions.Y < stageCollider.Top + 24) //Hitbox().Intersects(colliderTop))
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
						position.X = stageCollider.Left - dimensions.X;
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
				position.Y = Game1.gameState.stage.collider.Top - dimensions.Y + 1;
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

			if (launchTimer == 0)
			{
				if (Keybinds.TapJump(keyboard, ID) && jumps < 2)
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

				if (Keybinds.TapAtkNormal(keyboard, ID) && moveTimer == 0)
				{
					moveTimer = 16;
					currentMove = MoveType.NeutralA;
				}
			}
			else
			{
				launchTimer--;
			}

			if (moveTimer == 10 )
			{
				Rectangle hitbox = new Rectangle(Center.ToPoint(), new Point(15, 10));
				if (direction == -1)
					hitbox.X -= hitbox.Width;

				for (int i = 0; i < Game1.gameState.fighters.Length; i++)
				{
					if (ID != i && Game1.gameState.fighters[i].Hitbox().Intersects(hitbox))
					{
						Game1.gameState.fighters[i].Damage(10, new Vector2(10 * direction, -10));
						Game1.gameState.fighters[i].grounded = false;
						Game1.gameState.fighters[i].coyote = 0;
					}
				}
			}

			if (moveTimer > 0)
			{
				moveTimer--;
				if (moveTimer == 0)
				{
					currentMove = MoveType.None;
				}
			}

			if (grounded)
			{
				if (Math.Abs(velocity.X) > 1f)
				{
					direction = Math.Sign(velocity.X);

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

			if (currentMove != MoveType.None)
			{
				((AnimatedSprite)drawObject.texture).SwitchAnimation("punch", 0);
				
				if (moveTimer > 10)
				{
					((AnimatedSprite)drawObject.texture).Playing.Frame = 0;
				}
				else
				{
					((AnimatedSprite)drawObject.texture).Playing.Frame = 1;
				}
			}

			if (launchTimer > 0)
			{
				((AnimatedSprite)drawObject.texture).SwitchAnimation("launch", 0);
				direction = -Math.Sign(velocity.X);
			}
			drawObject.spriteEffects = direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			drawObject.frame = ((AnimatedSprite)drawObject.texture).GetFrame();
			drawObject.Bottom = Bottom;
			((AnimatedSprite)drawObject.texture).Update();
		}

		public Fighter(int ID)
		{
			this.ID = ID;

			dimensions = new Vector2(38, 64);
			position = new Vector2(300, 0);
			grounded = true;
			GraphicsHandler.main.children["game"].children.Add("fighter" + ID, new DrawObject(DrawObjectType.Sprite));
			drawObject = GraphicsHandler.main.children["game"].children["fighter" + ID];
			drawObject.texture = new AnimatedSprite(Game1.testFighter, new Point(50, 80));
			((AnimatedSprite)drawObject.texture).animations = new Dictionary<string, AnimatedSprite.Animation>()
			{
				{ "neutral",	new AnimatedSprite.Animation(0, 1, false) },
				{ "walk",		new AnimatedSprite.Animation(1, 1, false, "walk") },
				{ "jump",		new AnimatedSprite.Animation(2, 1, false) },
				{ "punch",		new AnimatedSprite.Animation(3, 2, false, "neutral") },
				{ "launch",		new AnimatedSprite.Animation(4, 1, false) },
			};
			((AnimatedSprite)drawObject.texture).SetAnimFrames();
			drawObject.frame = "neutral";
		}

		public void Damage(int damage, Vector2 knockback)
		{
			launchTimer = 20;
			velocity += knockback;
			this.damage += damage;
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
