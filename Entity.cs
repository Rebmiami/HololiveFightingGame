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

		public virtual void Update()
        {
			position += velocity;
        }
	}
}
