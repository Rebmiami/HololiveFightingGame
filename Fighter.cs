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
            position += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            base.Update();
        }
    }
}
