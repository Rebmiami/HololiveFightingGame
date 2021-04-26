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

        public HurtBody()
        {
            body = new List<Hurtbox>();
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
