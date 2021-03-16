using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	public class MoveData
	{
		// This is some stupid fake JSON that I'm going to use until I figure out how to use real JSON
		// Contains data on hitboxes and how they should evolve

		public virtual string JSON() => "";

		// Inherit this class and override JSON() with the JSON

		public readonly HitboxData hitboxData;

		public class HitboxData
		{
			int damage;
			Vector2 origin;
			Vector2 length;
			float radius;

			public class Timeline
			{

			}
		}

		public void CreateHitboxData()
        {

        }
	}
}
