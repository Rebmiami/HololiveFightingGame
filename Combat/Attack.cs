using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
    public struct Attack : IComparable
    {
        public Attack(int damage, Vector2 knockback, int priority = 1, Fighter attacker = null)
        {
            this.damage = damage;
            this.knockback = knockback;
            this.priority = priority;
            this.attacker = attacker;
        }

        public int damage;
        public Vector2 knockback;

        public int priority;
        // Used to resolve conflicts. If two attacks with the same priority conflict, they will cancel out.

        public Fighter attacker;
        // Set attacker to null if the damage was not caused by a player (directly or indirectly)

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
