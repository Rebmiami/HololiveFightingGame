using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	public abstract class EditorMenu
	{
		public EditorMenuItem[] items;
		public Stack<EditorMenuItem> escapeRoute;
		public int cursor;

		public int itemCount;

		public EditorMenu()
		{
			escapeRoute = new Stack<EditorMenuItem>();
		}

		public virtual void Draw(SpriteBatch spriteBatch, bool rightMenu)
		{

		}
	}
}
