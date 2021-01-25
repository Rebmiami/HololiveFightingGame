using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	public class Stage
	{
		public Rectangle collider; 
		// TODO: Support multiple colliders per stage
		public Rectangle stageBounds;

		public DrawObject drawObject;

		public Stage()
		{
			collider = new Rectangle((Program.WindowBounds().Width - 500) / 2, 300, 500, 100);
			stageBounds = Program.WindowBounds();

			drawObject = GraphicsHandler.main.children["game"].children["stage"];
			drawObject.texture = new SlicedSprite(Game1.testStage);
			drawObject.position = collider.Location.ToVector2();
		}
	}
}
