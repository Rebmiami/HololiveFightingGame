using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using HololiveFightingGame.Collision;
using System.Text.Json;
using HololiveFightingGame.Combat;
using HololiveFightingGame.Loading;

namespace HololiveFightingGame.Combat
{
    public class Move
	{
		public AttackHitbox[] hitboxes;

		/// <summary>
		/// Sets up the attack's hitboxes and dictates how the hitboxes should change over time
		/// </summary>
		public MoveData moveData;

		public void SetupMove()
		{
			Game1.jsonLoaderFilePath = @".\Content\Data\Moves\PekoraMoves.json";
			string json = System.IO.File.ReadAllText(Game1.jsonLoaderFilePath);
			moveData = (MoveData)JsonSerializer.Deserialize(json, typeof(MoveData), GameLoader.SerializerOptions);
			hitboxes = new AttackHitbox[moveData.Hitboxes.Length];
			for (int i = 0; i < moveData.Hitboxes.Length; i++)
			{
				MoveData.DataHitbox data = moveData.Hitboxes[i];
				AttackHitbox hitbox = new AttackHitbox
				{
					damage = data.Damage,
					angle = data.Angle,
					launch = data.Launch,
					collider = new Collider(new Capsule(data.Origin, data.Length, data.Radius)),
					kbScaling = data.KbScaling,
					priority = data.Priority,
					part = data.Part,
					autoSwipe = data.AutoSwipe,
					grounded = data.Grounded,
					aerial = data.Aerial
				};
				hitboxes[i] = hitbox;
			}
		}

		public Move(string move, string fighter)
		{
			Game1.jsonLoaderFilePath = @".\Content\Data\Fighters\" + fighter + @"\Moves\" + move + ".json";
			string json = System.IO.File.ReadAllText(Game1.jsonLoaderFilePath);
			moveData = (MoveData)JsonSerializer.Deserialize(json, typeof(MoveData), GameLoader.SerializerOptions);
			hitboxes = new AttackHitbox[moveData.Hitboxes.Length];
			for (int i = 0; i < moveData.Hitboxes.Length; i++)
			{
				MoveData.DataHitbox data = moveData.Hitboxes[i];
				AttackHitbox hitbox = new AttackHitbox
				{
					damage = data.Damage,
					angle = data.Angle,
					launch = data.Launch,
					collider = new Collider(new Capsule(data.Origin, data.Length, data.Radius))
				};
				hitboxes[i] = hitbox;
			}
		}

		// TODO: Add an array containing a series of attack hitboxes
		// Add a timeline that changes the position and properties of hitboxes over time
	}
}
