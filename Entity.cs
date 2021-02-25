using Microsoft.Xna.Framework;
using System;
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

		public DrawObject drawObject;

		public virtual void Update()
		{
			position += velocity;
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
