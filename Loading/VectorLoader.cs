using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Loading
{
	public class VectorLoader
	{
		public float X { get; set; }
		public float Y { get; set; }

		public static implicit operator Vector2(VectorLoader vector)
		{
			return new Vector2(vector.X, vector.Y);
		}
	}
}
