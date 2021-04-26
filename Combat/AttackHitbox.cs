using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using HololiveFightingGame.Collision;
using HololiveFightingGame.Utils;

namespace HololiveFightingGame.Combat
{
	public class AttackHitbox : ICloneable
	{
		public Collider collider;
		public int damage;
		public float angle;
		public float launch;

		public float kbScaling;
		public int priority;
		public int part;

		public bool grounded;
		public bool aerial;

		public bool autoSwipe;

		public AttackHitboxType type;

		public Vector2 LaunchAngle
		{
			get
			{
				return MathTools.RotateVector(new Vector2(launch, 0), MathHelper.ToRadians(-angle));
			}
			set
			{
				// I assume atan2 or dot products will be used when this is implemented
				launch = value.Length();
				throw new NotImplementedException();
			}
		}

		// AttackHitbox is not a struct but this will be necessary for hit detection
        public object Clone()
        {
			return new AttackHitbox
			{
				collider = collider,
				damage = damage,
				angle = angle,
				launch = launch,
				kbScaling = kbScaling,
				priority = priority,
				part = part,
				grounded = grounded,
				aerial = aerial,
				autoSwipe = autoSwipe,
				type = type
			};
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
