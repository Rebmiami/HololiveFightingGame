using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	public abstract class Entity
	{
		public Vector2 position;
		public Vector2 velocity;

		public Vector2 dimensions;

		public virtual void Update()
		{
			position += velocity;
		}

		public Rectangle Hitbox()
        {
			// TODO: Replace rectangle with capsule collision and cut down on collision checks
			return new Rectangle(position.ToPoint(), dimensions.ToPoint());
        }
	}
}
