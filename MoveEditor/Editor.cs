using HololiveFightingGame.Combat;
using HololiveFightingGame.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.MoveEditor
{
	public static class Editor
	{
		public static Fighter fighter;

		public static void Load()
        {
			// Unloads previously loaded graphics
			GraphicsHandler.main.children.Clear();
			GraphicsHandler.main.children.Add("game", new DrawObject(DrawObjectType.Layer));

			// Sets up a fighter to be edited
			fighter = new Fighter(0, "Pekora");
			FighterLoader.LoadMoves(new Fighter[] { fighter });
			FighterLoader.LoadAnimations(new Fighter[] { fighter });
			fighter.drawObject.position += new Vector2(100);
		}
	}
}
