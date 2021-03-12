﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using HololiveFightingGame.Collision;

namespace HololiveFightingGame
{
	public class AttackHitbox
	{
		public Collider collider;
		public int damage;
		public Vector2 launchAngle;
		public AttackHitboxType type;
    }

	public enum AttackHitboxType
    {
		Attack,
		Shield,
		Counter,
    }
}
