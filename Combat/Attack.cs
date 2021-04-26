using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	public class Attack : IComparable
	{
		public Attack(AttackHitbox hitbox, Fighter attacker)
		{
			attackHitbox = hitbox;
			damage = hitbox.damage;
			knockback = hitbox.LaunchAngle;
			priority = hitbox.priority;
			this.attacker = attacker;
			dealDamage = true;
		}

		public AttackHitbox attackHitbox;

		public int damage;
		public Vector2 knockback;

		public int priority;
		// Used to resolve conflicts. If two attacks with the same priority conflict, they will cancel out.

		public Fighter attacker;
		// Set attacker to null if the damage was not caused by a player (directly or indirectly)

		public bool dealDamage;
		// Set to true if the attack landed but won't deal damage, ie deflected by shield or hitting an invincible hitbox.

		public int CompareTo(object obj)
		{
			if (obj is Attack attack)
			{
				return attack.priority - priority;
			}
			throw new ArgumentException("An attempt was made to compare an Attack object with an object that is not an Attack.");
		}
	}
}
