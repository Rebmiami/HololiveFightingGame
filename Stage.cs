using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using HololiveFightingGame.Collision;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Loading;

namespace HololiveFightingGame
{
	public class Stage
	{
		public Collider collider; 
		// TODO: Support multiple colliders per stage
		// TODO: Move stages from rectangle to new collider system
		public Rectangle stageBounds;

		public DrawObject drawObject;

		public Stage()
		{
			collider = new Collider(new Rectangle((Program.WindowBounds().Width - 500) / 2, 300, 500, 100));
			stageBounds = Program.WindowBounds();

			drawObject = GraphicsHandler.main.children["game"].children["stage"];
			drawObject.texture = new SlicedSprite(Assets.testStage);
			drawObject.position = collider.Rectangle.Location.ToVector2();
		}
	}
}
