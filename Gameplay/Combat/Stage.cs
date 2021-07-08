using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Loading;
using HololiveFightingGame.Gameplay.Collision;

namespace HololiveFightingGame.Gameplay.Combat
{
	public class Stage
	{
		public Collider collider;
		// TODO: Support multiple colliders per stage
		// TODO: Move stages from rectangle to new collider system (triangles or arbitrary polygons)
		public Rectangle stageBounds;

		public DrawObject drawObject;

		public Stage()
		{
			collider = new Collider(new Rectangle(150, 300, 500, 100));
			stageBounds = new Rectangle(-150, -200, 1100, 780);

			drawObject = GraphicsHandler.main.children["game"].children["stage"];
			drawObject.texture = new SlicedSprite(Assets.testStage);
			drawObject.position = collider.Rectangle.Location.ToVector2();
		}
	}
}
