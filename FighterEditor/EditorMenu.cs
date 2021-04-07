using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	public abstract class EditorMenu
	{
		public EditorMenuItem[] items;
		public int cursor;

		public int itemCount;

		public virtual void Draw(SpriteBatch spriteBatch, bool rightMenu)
		{

		}
	}
}
