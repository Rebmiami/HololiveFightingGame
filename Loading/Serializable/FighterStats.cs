using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Loading.Serializable
{
	public class FighterStats
	{
		public int EJumps { get; set; }
		
		public float JumpForce { get; set; }
		public float EJumpForce { get; set; }

		public float AirResistance { get; set; }
		public float Traction { get; set; }

		public float Gravity { get; set; }

		public float AirSpeed { get; set; }
		public float GroundSpeed { get; set; }

		public float FallSpeed { get; set; }
		public float FastFallSpeed { get; set; }

		public float AirAccel { get; set; }
		public float GroundAccel { get; set; }

		public int Weight { get; set; }


		public Dictionary<string, float> HurtboxRadii { get; set; }
	}
}
