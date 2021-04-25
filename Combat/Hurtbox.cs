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
	}
}
