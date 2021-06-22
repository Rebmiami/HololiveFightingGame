using HololiveFightingGame.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.GUI
{
	public static class IconArtist
	{
		public static Texture2D editorIcons;

		public static void DrawIcon(SpriteBatch spriteBatch, Vector2 position, EditorIcon icon)
		{
			Point iconPositionOnSheet = new Point((int)icon % 12 * 25, (int)icon / 12 * 25);
			spriteBatch.Draw(editorIcons, position, new Rectangle(iconPositionOnSheet, new Point(24, 24)), Color.White);
		}

		static IconArtist()
		{
			editorIcons = ImageLoader.LoadTexture(@".\Content\Assets\EditorIcons.png", true);
		}
	}
}
