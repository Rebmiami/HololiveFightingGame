using HololiveFightingGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HololiveFightingGame.FighterEditor
{
	public abstract class EditorMenuItem
	{
		/// <summary>
		/// Called when pressing Enter while a mid-level menu item is selected. Allows the user to select menu items within this item. <br />
		/// Not to be confused with pressing Enter to confirm edits - see <see cref="Escape"/>
		/// </summary>
		public virtual void Enter()
		{
			parent.escapeRoute.Push(this);
			parent.cursor = 0;
		}

		/// <summary>
		/// Called when pressing Backspace while a lowest-level menu item is selected. <br />
		/// Clears the item's value and allows the user to input a new one.
		/// </summary>
		public virtual void Erase()
		{

		}

		/// <summary>
		/// Called every frame between <see cref="Erase"/> and <see cref="Escape"/> being called on a lowest-level menu item.
		/// </summary>
		public virtual void Edit()
		{

		}

		/// <summary>
		/// Called when pressing Escape to exit a mid-level menu item or pressing Enter to confirm edits made to a lowest-level menu item. <br />
		/// If the lowest-level menu item is a button, this is called without <see cref="Erase"/> or <see cref="Edit"/>.
		/// </summary>
		public virtual void Escape(ref object target)
		{
			if (!lowestLevel)
			{
				parent.cursor = parent.escapeRoute.Pop().ID;
			}
		}

		public EditorMenuItem(EditorMenu parent, int ID)
		{
			this.parent = parent;
			this.ID = ID;
			children = new EditorMenuItem[0];
		}

		public bool Selected
		{
			get
			{
				return parent.escapeRoute.Contains(this);
			}
		}
		public bool Highlighted
		{
			get
			{
				if (Selected)
					return false;
				if (parent.escapeRoute.Count == 0)
					return parent.cursor == ID && new List<EditorMenuItem>(parent.items).Contains(this);

				if (!new List<EditorMenuItem>(parent.CurrentItemPool).Contains(this))
					return false;

				return parent.cursor == ID && parent.escapeRoute.Peek().Selected;
			}
		}

		public EditorMenu parent;
		public int ID;
		public bool lowestLevel;
		public bool button;
		public bool open;
		public EditorMenuItem[] children;
		public Rectangle clickbox;

		public string helpText;
		public string helpArticle;

		public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
		{

		}

		public int Flavor 
		{ 
			get
			{
				if (Selected)
					return 2;
				if (Highlighted)
					return 1;
				return 0;
			}
		}

		public bool Hovering()
        {
			return clickbox.Contains(Mouse.GetState().Position);
        }

		public bool IsClicked()
        {
			return Hovering() && MouseHelper.Down(MouseButtons.Left);
        }

		public void Clicked()
        {

        }
	}
}
