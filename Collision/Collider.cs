using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Collision
{
	public struct Collider
	{
		public ColliderType type;
		public object collider;

		public Collider(Vector2 vector)
		{
			collider = vector;
			type = ColliderType.Point;
		}

		public Collider(Rectangle rectangle)
		{
			collider = rectangle;
			type = ColliderType.Rectangle;
		}

		public Collider(Capsule capsule)
		{
			collider = capsule;
			type = ColliderType.Capsule;
		}

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

		public bool Intersects(Collider collider)
		{
			switch (type)
			{
				case ColliderType.Point:
					switch (collider.type)
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
					switch (type)
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
					switch (type)
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

		public bool Intersects(Vector2 vector)
		{
            return type switch
            {
                ColliderType.Point => Point == vector,
                ColliderType.Rectangle => Rectangle.Contains(vector),
                ColliderType.Capsule => Capsule.Contains(vector),
                _ => throw new ArgumentException("The collider checked for intersection does not have a valid collider type."),
            };
        }

		public bool Intersects(Rectangle rectangle)
		{
            return type switch
            {
                ColliderType.Point => rectangle.Contains(Point),
                ColliderType.Rectangle => rectangle.Intersects(Rectangle),
                ColliderType.Capsule => Capsule.Intersects(rectangle),
                _ => throw new ArgumentException("The collider checked for intersection does not have a valid collider type."),
            };
        }

		public bool Intersects(Capsule capsule)
		{
            return type switch
            {
                ColliderType.Point => capsule.Contains(Point),
                ColliderType.Rectangle => capsule.Intersects(Rectangle),
                ColliderType.Capsule => capsule.Intersects(Capsule),
                _ => throw new ArgumentException("The collider checked for intersection does not have a valid collider type."),
            };
        }

		public void SetPosition(Vector2 vector, Vector2 offset = default)
		{
			switch (type)
			{
				case ColliderType.Point:
					collider = vector + offset;
					break;
				case ColliderType.Rectangle:
					Rectangle rectangle = Rectangle;
					rectangle.Location = (vector + offset).ToPoint();
					Rectangle = rectangle;
					break;
				case ColliderType.Capsule:
					Capsule capsule = Capsule;
					capsule.origin = vector + offset;
					Capsule = capsule;
					break;
			}
		}
	}

	public enum ColliderType
	{
		Point,
		Rectangle,
		Capsule
	}
}
