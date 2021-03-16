using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	public class Move
	{
		public AttackHitbox[] hitboxes;

		/// <summary>
		/// Sets up the attack's hitboxes and dictates how the hitboxes should change over time
		/// </summary>
		public MoveData moveData;

		// TODO: Add an array containing a series of attack hitboxes
		// Add a timeline that changes the position and properties of hitboxes over time
	}
}
