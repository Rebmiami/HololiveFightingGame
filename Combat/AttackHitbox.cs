using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using HololiveFightingGame.Collision;
using HololiveFightingGame.Utils;

namespace HololiveFightingGame.Combat
{
	public class AttackHitbox
	{
		public Collider collider;
		public bool enabled;

		public int damage;
		public float angle;
		public float launch;

		public AttackHitboxType type;


		public Vector2 LaunchAngle
		{
			get
			{
				return MathTools.RotateVector(new Vector2(launch, 0), MathHelper.ToRadians(-angle));
			}
			set
			{
				launch = value.Length();
				throw new NotImplementedException();
			}
		}
	}

	public enum AttackHitboxType
	{
		Attack,
		Shield,
		Counter,
		Grab,
	}
}
