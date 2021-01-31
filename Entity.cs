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

		public DrawObject drawObject;

		public virtual void Update()
		{
			position += velocity;
		}

		public Rectangle Hitbox()
		{
			// TODO: Replace rectangle with capsule collision and cut down on collision checks
			return new Rectangle(position.ToPoint(), dimensions.ToPoint());
		}

		public Vector2 Bottom
		{
			get { return position + new Vector2(dimensions.X / 2, dimensions.Y); }
			set { position = value - new Vector2(dimensions.X / 2, dimensions.Y); }
		}

		public Vector2 Center
		{
			get { return position + new Vector2(dimensions.X / 2, dimensions.Y / 2); }
			set { position = value - new Vector2(dimensions.X / 2, dimensions.Y / 2); }
		}
	}
}
