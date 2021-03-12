using Microsoft.Xna.Framework;
using System;
using HololiveFightingGame.Collision;
using HololiveFightingGame.Graphics;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	public abstract class Entity
	{
		public Vector2 position;
		public Vector2 velocity;

		public Vector2 Dimensions
		{
			get
			{
				return collider.type switch
				{
					ColliderType.Point => throw new InvalidOperationException("Colliders of type point do not have dimensions"),
					ColliderType.Rectangle => collider.Rectangle.Size.ToVector2(),
					ColliderType.Capsule => collider.Capsule.GetBoundingBox().Size.ToVector2(),
					_ => throw new InvalidOperationException("Dimensions could not be retrieved because the collider does not have a valid type."),
				};
			}
			set
			{
				if (collider.type == ColliderType.Rectangle)
				{
					Rectangle rectangle = collider.Rectangle;
					rectangle.Size = value.ToPoint();
					collider.Rectangle = rectangle;
					return;
				}
				throw new InvalidOperationException("Setting dimensions of colliders via Dimensions property is only supported by rectangle colliders.");
			}
		}

		public Collider collider;
		public Vector2 colliderOrigin;
		// The origin of the collider relative to the point position of the entity.
		// (-1, -1) will anchor the collider's top left point to position. (0, 0) will center it.
		// Both components should be between 1 and -1 in most cases but this is not strictly required.
		// In the case of capsules, the bounding box of the capsule will be used to find the origin.
		// The capsule origin (base) will be anchored to the collider origin point.
		// Collider offset of the capsule's radius can be used to align the bottom of the capsule
		public Vector2 colliderOffset;
		// Linear offset for the collider. Applied after collider origin, shifting the collider position by the value specified.

		public DrawObject drawObject;

		public virtual void Update()
		{
			position += velocity;

			switch (collider.type)
			{
				case ColliderType.Point:
					break;
				case ColliderType.Rectangle:
					Vector2 dimensions = Hitbox().Size.ToVector2();
					Vector2 center = dimensions / 2 * colliderOrigin + dimensions / 2;
					center += colliderOffset;
					collider.SetPosition(position, center);
					break;
				case ColliderType.Capsule:
					collider.SetPosition(position, new Vector2(19, 45)); // TODO: Do this automatically
					break;
			}
		}

		public Rectangle Hitbox()
		{
			// TODO: Replace rectangle with capsule collision and cut down on collision checks
			return new Rectangle(position.ToPoint(), Dimensions.ToPoint());
		}

		public Vector2 Bottom
		{
			get { return position + new Vector2(Dimensions.X / 2, Dimensions.Y); }
			set { position = value - new Vector2(Dimensions.X / 2, Dimensions.Y); }
		}

		public Vector2 Center
		{
			get { return position + new Vector2(Dimensions.X / 2, Dimensions.Y / 2); }
			set { position = value - new Vector2(Dimensions.X / 2, Dimensions.Y / 2); }
		}
	}
}
