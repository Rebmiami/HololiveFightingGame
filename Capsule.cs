using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
    public struct Capsule
    {
        public Vector2 origin;
        public Vector2 length;

        public float radius;

        public float Distance(Vector2 vector)
        {
            throw new NotImplementedException();
        }

        public float Distance(Capsule capsule)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Vector2 vector)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Capsule capsule)
        {
            throw new NotImplementedException();
        }

        public bool Intersects(Capsule capsule)
        {
            throw new NotImplementedException();
        }

        public bool Intersects(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }





        public Rectangle GetBoundingBox()
        {
            throw new NotImplementedException();
        }
    }
}
