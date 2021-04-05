using HololiveFightingGame.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HololiveFightingGame.Combat
{
	public class MoveData
	{
		/// <summary>
		/// The number of frames the move lasts for.
		/// </summary>
		public int MoveDuration { get; set; }
		/// <summary>
		/// The internal name of the move.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// The hitboxes of the move.
		/// </summary>
		public DataHitbox[] Hitboxes { get; set; }
		/// <summary>
		/// Which frames should be shown and when.
		/// </summary>
		public Dictionary<string, int> AnimationFrames { get; set; }
		/// <summary>
		/// Which part of the move should this move lead in to? This field is used for various things.
		/// </summary>
		public int LeadInto { get; set; }
		/// <summary>
		/// The velocity of the player as the move progresses.
		/// </summary>
		public Dictionary<string, VectorLoader> Motion { get; set; }
		/// <summary>
		/// How much of the player's existing momentum is preserved.
		/// </summary>
		public float Sustain { get; set; }
		/// <summary>
		/// The move animation.
		/// </summary>
		public Animation Animation { get; set; }

		public class DataHitbox
		{
			// Attack information
			public int Damage { get; set; }
			public float Angle { get; set; }
			public float Launch { get; set; }
			public float KbScaling { get; set; }
			// TODO: Add "effect" property for attacks with other effects
			// TODO: Add support for projectiles

			// Hitbox information
			public VectorLoader Origin { get; set; }
			public VectorLoader Length { get; set; }
			public float Radius { get; set; }
			public bool AutoSwipe { get; set; }

			// Collision data
			public bool Enabled { get; set; }
			public int Priority { get; set; }
			public int Part { get; set; }
			public bool Grounded { get; set; }
			public bool Aerial { get; set; }

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
