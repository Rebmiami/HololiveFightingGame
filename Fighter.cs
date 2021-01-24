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
			velocity *= 0.99f;
			velocity.X *= 0.8f;
			velocity.Y += 0.5f;
			velocity.X += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

			base.Update();

			if (Hitbox().Intersects(Game1.gameState.stage.collider))
			{
				grounded = true;
				coyote = 7;
			}

			if (grounded)
			{
				velocity.Y = 0;
				position.Y = Game1.gameState.stage.collider.Top - dimensions.Y;
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
		}
	}
}
