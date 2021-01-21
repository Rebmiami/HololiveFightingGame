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
		public int damage;

        public override void Update()
        {
            velocity *= 0.99f;
            velocity.Y += 0.5f;
            velocity.X += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

            if (Hitbox().Intersects(Game1.gameState.stage.collider))
            {
                velocity.Y = 0;
                position.Y = Game1.gameState.stage.collider.Top - dimensions.Y;
                velocity.X *= 0.8f;
            }


            base.Update();
        }

        public Fighter()
        {
            dimensions = new Vector2(38, 64);
            position = new Vector2(100, 0);
        }
    }
}
