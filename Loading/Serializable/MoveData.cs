using HololiveFightingGame.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HololiveFightingGame.Loading.Serializable
{
    public class MoveData
    {
        /// <summary>
        /// The number of frames the move lasts for.
        /// </summary>
        public int MoveDuration { get; set; }
        /// <summary>
        /// The internal name of the move.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The hitboxes of the move.
        /// </summary>
        public DataHitbox[] Hitboxes { get; set; }
        /// <summary>
        /// Which frames should be shown and when.
        /// </summary>
        public Dictionary<string, int> AnimationFrames { get; set; }
        /// <summary>
        /// Which part of the move should this move lead in to? This field is used for various things.
        /// </summary>
        public int LeadInto { get; set; }

        /// <summary>
        /// The velocity of the player as the move progresses.
        /// </summary>
        public Dictionary<string, VectorLoader> Motion { get; set; }

        /// <summary>
        /// How much of the player's existing momentum is preserved.
        /// </summary>
        public float Sustain { get; set; }
        // <summary>
        // Sustain change over time. To be implemented.
        // </summary>
        // public Dictionary<string, float> Motion { get; set; }


        /// <summary>
        /// Whether or not the move is aerial. Aerial moves will be interrupted if the fighter lands on the ground.
        /// </summary>
        public bool Aerial { get; set; }

        /// <summary>
        /// Whether or not the move should end in special fall. Special fall leaves the fighter unable to perform any action other than moving sideways.
        /// </summary>
        public bool SpecialFall { get; set; }

        /// <summary>
        /// Whether or not the player is allowed to turn at this point in the move.
        /// </summary>
        public Dictionary<string, bool> CanTurn { get; set; }
        /// <summary>
        /// Whether or not the player is allowed to cancel the move by jumping, shielding, etc. at this point in the move.
        /// </summary>
        public Dictionary<string, bool> CanCancel { get; set; }



        /// <summary>
        /// The move animation.
        /// </summary>
        public Animation Animation { get; set; }

        public class DataHitbox
        {
            // Attack information
            public int Damage { get; set; }
            public float Angle { get; set; }
            public float Launch { get; set; }
            public float KbScaling { get; set; }
            // TODO: Add "effect" property for attacks with other effects
            // TODO: Add support for projectiles

            // Hitbox information
            public VectorLoader Origin { get; set; }
            public VectorLoader Length { get; set; }
            public float Radius { get; set; }
            public bool AutoSwipe { get; set; }

            // Collision data
            public bool Enabled { get; set; }
            public int Priority { get; set; }
            public int Part { get; set; }
            public bool Grounded { get; set; }
            public bool Aerial { get; set; }

            // Timeline information
            public Dictionary<string, VectorLoader> Motion { get; set; }
            public Dictionary<string, bool> Activation { get; set; }
        }
    }
}
