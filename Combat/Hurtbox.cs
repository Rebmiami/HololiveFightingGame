using HololiveFightingGame.Collision;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	public class Hurtbox
	{
		public bool Vulnerable;
		public bool Tangible;

		public Collider collider;
		// Should always be a capsule
		// if possible, exception for rushia (rectangles are flatter than capsules)

		public Hurtbox(Capsule capsule)
        {
			collider = new Collider(capsule);
			Vulnerable = true;
			Tangible = true;
        }
	}
}
