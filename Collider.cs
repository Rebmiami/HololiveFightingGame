using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	public struct Collider
	{
		public ColliderType colliderType;
		public object collider;

		public Vector2 Point
		{
			get
			{
				return (Vector2)collider;
			}
			set
			{
				collider = value;
			}
		}

		public Rectangle Rectangle
		{
			get
			{
				return (Rectangle)collider;
			}
			set
			{
				collider = value;
			}
		}

		public Capsule Capsule
		{
			get
			{
				return (Capsule)collider;
			}
			set
			{
				collider = value;
			}
		}

		public enum ColliderType
		{
			Point,
			Rectangle,
			Capsule
		}

		public bool Intersecting(Collider collider)
		{
			switch (colliderType)
			{
				case ColliderType.Point:
					switch (collider.colliderType)
					{
						case ColliderType.Point:
							return Point == collider.Point;
						case ColliderType.Rectangle:
							return collider.Rectangle.Contains(Point);
						case ColliderType.Capsule:
							return collider.Capsule.Contains(Point);
					}
					break;
				case ColliderType.Rectangle:
					switch (colliderType)
					{
						case ColliderType.Point:
							return Rectangle.Contains(collider.Point);
						case ColliderType.Rectangle:
							return Rectangle.Intersects(collider.Rectangle);
						case ColliderType.Capsule:
							return collider.Capsule.Intersects(Rectangle);
					}
					break;
				case ColliderType.Capsule:
					switch (colliderType)
					{
						case ColliderType.Point:
							return Capsule.Contains(collider.Point);
						case ColliderType.Rectangle:
							return Capsule.Intersects(collider.Rectangle);
						case ColliderType.Capsule:
							return Capsule.Intersects(collider.Capsule);
					}
					break;
			}
			throw new ArgumentException("One or more colliders checked for intersection do not have a valid collider type.");
		}
	}
}
