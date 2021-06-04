﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	public abstract class EditorMenu
	{
		public EditorUIItem[] items;
		public Stack<EditorUIItem> escapeRoute;
		public int cursor;

		/// <summary>
		/// The <see cref="EditorUIItem"/> that the cursor is focused on.
		/// </summary>
		public EditorUIItem HighlightedItem
		{
			get
			{
				if (escapeRoute.Count == 0)
					return items[cursor];
				return escapeRoute.Peek().children[cursor];
			}
		}

		/// <summary>
		/// An array of type <see cref="EditorUIItem"/> containing all items that the user can select in the current context.
		/// </summary>
		public EditorUIItem[] CurrentItemPool
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
			escapeRoute = new Stack<EditorUIItem>();
			items = new EditorUIItem[0];
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
