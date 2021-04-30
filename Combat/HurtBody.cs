using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	public class HurtBody
	{
		public List<Hurtbox> body;

		public Vector2 foot;
		// Determine this using colliderOrigin/Offset

		public HurtBody()
		{
			body = new List<Hurtbox>();
		}

		public Attack CheckHits(AttackHitbox[] hitboxes, Fighter fighter)
        {
			List<Attack> attacks = new List<Attack>();
			foreach (AttackHitbox hitbox in hitboxes)
            {
				int index = 0;
				foreach (Attack attack in attacks)
                {
					if (attack.attackHitbox.priority < hitbox.priority)
                    {
						index++;
                    }
                }
				attacks.Insert(index, CheckHits(hitbox, fighter));
            }
			return null;
        }

		public Attack CheckHits(AttackHitbox hitbox, Fighter fighter)
		{
			bool hit = false;
			foreach (Hurtbox hurtbox in body)
			{
				if (!hurtbox.Tangible)
				{
					continue;
				}

				if (hurtbox.collider.Intersects(hitbox.collider))
				{
					if (hurtbox.Vulnerable)
					{
						hit = true;
					}
					else
					{
						// Invulnerable hitboxes always take priority over vulnerable ones.
						// ie if an attack hits a vulnerable hitbox and an invulnerable one, the attack will deal no damage.
						Attack attack = new Attack(hitbox, fighter)
						{
							dealDamage = false
						};
						return attack;
					}
				}
			}
			if (hit)
			{
				return new Attack(hitbox, fighter);
			}
			return null;
		}
	}
}
