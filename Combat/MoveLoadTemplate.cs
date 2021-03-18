using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	public class MoveLoadTemplate
	{
		public int Damage { get; set; }
		public float Angle { get; set; }
		public float Launch { get; set; }

		public float[] Dims { get; set; }
		// Origin X, Origin Y, Length X, Length Y, and Radius in that order
	}
}
