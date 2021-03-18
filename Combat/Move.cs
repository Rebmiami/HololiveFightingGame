using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using HololiveFightingGame.Collision;
using System.Text.Json;
using HololiveFightingGame.Combat;

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
			hitboxes = new AttackHitbox[1];
			AttackHitbox hitbox = new AttackHitbox();
			string json = System.IO.File.ReadAllText(Game1.gamePath + @"\Data\Moves\PekoraMoves.json");
			moveData = (MoveData)JsonSerializer.Deserialize(json, typeof(MoveData));
			hitbox.damage = moveData.Damage;
			hitbox.angle = moveData.Angle;
			hitbox.launch = moveData.Launch;
			hitbox.collider = new Collider(new Capsule(
				new Vector2(moveData.Dims[0], moveData.Dims[1]),
				new Vector2(moveData.Dims[2], moveData.Dims[3]),
				moveData.Dims[4]));
			hitboxes[0] = hitbox;
		}

		public Move()
		{
			
		}

		// TODO: Add an array containing a series of attack hitboxes
		// Add a timeline that changes the position and properties of hitboxes over time
	}
}
