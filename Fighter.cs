using Microsoft.Xna.Framework;
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
		public int damage;

		public int jumps;

		public override void Update()
		{
			if (!Game1.gameState.stage.stageBounds.Intersects(Hitbox()))
			{
				position = new Vector2(300, 0);
				velocity = Vector2.Zero;
				grounded = true;
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
				Rectangle colliderTop = new Rectangle(stageCollider.Left, stageCollider.Top, stageCollider.Width, 12);
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
		}

		public Fighter()
		{
			dimensions = new Vector2(38, 64);
			position = new Vector2(300, 0);
			grounded = true;
		}
	}
}
