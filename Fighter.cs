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
			velocity.X += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
			Vector2 maxVelocity = new Vector2(6, 10);
			velocity = Vector2.Clamp(velocity, -maxVelocity, maxVelocity);

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

			if (GamePadHelper.Pressed(Buttons.A, PlayerIndex.One) && jumps < 2)
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

			if (GamePadHelper.Pressed(Buttons.B, PlayerIndex.One) && moveTimer == 0)
			{
				moveTimer = 20;
				currentMove = MoveType.NeutralA;
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
					drawObject.spriteEffects = velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
					drawObject.frame = "walk";
				}
				else
				{
					drawObject.frame = "stand";
				}
			}
			else
			{
				drawObject.frame = "jump";
			}

			if (currentMove != MoveType.None)
			{
				if (moveTimer > 10)
				{
					drawObject.frame = "punch1";
				}
				else
				{
					drawObject.frame = "punch2";
				}
			}

			drawObject.Bottom = Bottom;
		}

		public Fighter()
		{
			dimensions = new Vector2(38, 64);
			position = new Vector2(300, 0);
			grounded = true;
			drawObject = GraphicsHandler.main.children["game"].children["fighter"];
			drawObject.texture = new SlicedSprite(Game1.testFighter);
			drawObject.texture.slices = new Dictionary<string, Rectangle>()
			{
				{ "stand", new Rectangle(0, 80 * 0, 50, 80) },
				{ "walk", new Rectangle(0, 80 * 1, 50, 80) },
				{ "jump", new Rectangle(0, 80 * 2, 50, 80) },
				{ "punch1", new Rectangle(0, 80 * 3, 50, 80) },
				{ "punch2", new Rectangle(0, 80 * 4, 50, 80) },
			};
			drawObject.frame = "idle";
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
