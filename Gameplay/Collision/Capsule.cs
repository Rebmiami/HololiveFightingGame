using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Gameplay.Collision
{
	public struct Capsule
	{
		public Vector2 origin;
		public Vector2 length;

		public float radius;

		public Capsule(Vector2 origin, Vector2 length, float radius)
		{
			this.origin = origin;
			this.length = length;
			this.radius = radius;
		}

		public float Distance(Vector2 vector)
		{
			Vector2 normal = Vector2.Normalize(length);
			if (length == Vector2.Zero)
			{
				normal = Vector2.Zero;
			}
			float dot = Vector2.Dot(vector - origin, normal);
			Vector2 point = normal * dot + origin;
			Vector2 lower = new Vector2(Math.Min(Base.X, Tip.X), Math.Min(Base.Y, Tip.Y));
			Vector2 upper = new Vector2(Math.Max(Base.X, Tip.X), Math.Max(Base.Y, Tip.Y));
			point = Vector2.Clamp(point, lower, upper);
			return Vector2.Distance(vector, point);
		}

		public float Distance(Capsule capsule)
		{
			float[] distances = new float[4];

			distances[0] = Distance(capsule.Base);
			distances[1] = Distance(capsule.Tip);
			distances[2] = capsule.Distance(Base);
			distances[3] = capsule.Distance(Tip);

			Array.Sort(distances);
			return distances[0] - (capsule.radius + radius);
		}

		public bool Contains(Vector2 vector)
		{
			return Distance(vector) < radius;
		}

		public bool Contains(Capsule capsule)
		{
			throw new NotImplementedException();
		}

		public bool Intersects(Capsule capsule)
		{
			// TODO: Figure this out later
			// if (GetBoundingBox().Intersects(capsule.GetBoundingBox()))
			{
				float distance = Distance(capsule);
				return distance <= 0;
			}
		}

		public bool Intersects(Rectangle rectangle)
		{
			// This implementation is NOT complete!
			if (GetBoundingBox().Intersects(rectangle))
			{
				return true;
			}
			return false;
			//throw new NotImplementedException();
		}

		public Rectangle GetBoundingBox()
		{
			Rectangle A = new Rectangle(origin.ToPoint() - new Point((int)radius), new Point((int)radius));
			Rectangle B = new Rectangle(Tip.ToPoint() - new Point((int)radius), new Point((int)radius));
			return Rectangle.Union(A, B);
		}

		public Vector2 Base
		{
			get
			{
				return origin;
			}
			set
			{
				length += value - origin;
				origin = value;
			}
		}

		public Vector2 Tip
		{
			get
			{
				return origin + length;
			}
			set
			{
				length = value - origin;
			}
		}
	}
}
