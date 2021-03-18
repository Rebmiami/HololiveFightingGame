using System;
using System.Collections.Generic;
using System.Text;
using HololiveFightingGame.Collision;

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
			moveData.CreateHitboxData();
			hitbox.damage = moveData.hitboxData.damage;
			hitbox.angle = moveData.hitboxData.angle;
			hitbox.launch = moveData.hitboxData.launch;
			hitbox.collider = new Collider(new Capsule(moveData.hitboxData.origin, moveData.hitboxData.length, moveData.hitboxData.radius));
			hitboxes[0] = hitbox;
		}

		public Move()
		{

		}

		public Move(MoveData data)
		{
			moveData = data;
		}

		// TODO: Add an array containing a series of attack hitboxes
		// Add a timeline that changes the position and properties of hitboxes over time
	}
}
