using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
			public int damage;
			public float angle;
			public float launch;
			public Vector2 origin;
			public Vector2 length;
			public float radius;

			public class Timeline
			{

			}
		}
		
		public MoveData()
		{
			hitboxData = new HitboxData();
		}

		public void CreateHitboxData()
		{
			string json = JSON();
			MoveLoadTemplate toLoad = (MoveLoadTemplate)JsonSerializer.Deserialize(json, typeof(MoveLoadTemplate));
			hitboxData.damage = toLoad.Damage;
			hitboxData.angle = toLoad.Angle;
			hitboxData.launch = toLoad.Launch;
			hitboxData.origin = new Vector2(toLoad.Dims[0], toLoad.Dims[1]);
			hitboxData.length = new Vector2(toLoad.Dims[2], toLoad.Dims[3]);
			hitboxData.radius = toLoad.Dims[4];
		}
	}
}