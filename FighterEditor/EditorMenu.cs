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

		/// <summary>
		/// The <see cref="EditorMenuItem"/> that the cursor is focused on.
		/// </summary>
		public EditorMenuItem HighlightedItem
		{
			get
			{
				if (escapeRoute.Count == 0)
					return items[cursor];
				return escapeRoute.Peek().children[cursor];
			}
		}

		/// <summary>
		/// An array of type <see cref="EditorMenuItem"/> containing all items that the user can select in the current context.
		/// </summary>
		public EditorMenuItem[] CurrentItemPool
		{
			get
			{
				if (escapeRoute.Count == 0)
					return items;
				return escapeRoute.Peek().children;
			}
		}

		public EditorMenu()
		{
			escapeRoute = new Stack<EditorMenuItem>();
		}

		public virtual void Draw(SpriteBatch spriteBatch, bool rightMenu)
		{

		}

		public void Scroll(bool up)
		{
			if (up)
			{
				cursor -= 1;
				if (cursor < 0)
				{
					cursor = CurrentItemPool.Length - 1;
				}
			}
			else
			{
				cursor++;
				cursor %= CurrentItemPool.Length;
			}
		}
	}
}
