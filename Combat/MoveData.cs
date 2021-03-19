﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HololiveFightingGame.Combat
{
	// TODO: Place this within a larger class containing a dictionary of all the fighter's moves
	public class MoveData
	{
		public int MoveDuration { get; set; }
		public string Name { get; set; }
		public DataHitbox[] Hitboxes { get; set; }
		public Dictionary<string, int> AnimationFrames { get; set; }

		// TODO: Add support for "branching" attacks, such as charge-and-release or branches

		public class DataHitbox
		{
			// Attack information
			public int Damage { get; set; }
			public float Angle { get; set; }
			public float Launch { get; set; }
			// TODO: Add "effect" property for attacks with other effects
			// TODO: Add support for projectiles

			// Hitbox information
			public VectorLoader Origin { get; set; }
			public VectorLoader Length { get; set; }
			public float Radius { get; set; }

			// Collision data
			public bool Enabled { get; set; }
			public int Priority { get; set; }

			// Timeline information
			public Dictionary<string, VectorLoader> Motion { get; set; }
			public Dictionary<string, bool> Activation { get; set; }
		}

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
}