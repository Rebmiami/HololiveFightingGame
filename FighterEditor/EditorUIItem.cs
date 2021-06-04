using HololiveFightingGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HololiveFightingGame.FighterEditor
{
	public abstract class EditorUIItem
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

		/// <summary>
		/// Called every frame.
		/// </summary>
		public virtual void Update()
        {
			foreach (EditorUIItem item in children)
            {
				item.Update();
            }

            switch (type)
            {
                case EditorMenuItemType.Button:
                    break;
                case EditorMenuItemType.Hoverable:
					if (Hovering())
                    {
						parent.cursor = ID;
                    }
                    break;
                case EditorMenuItemType.Tickbox:
                    break;
                case EditorMenuItemType.ToggleButton:
                    break;
                case EditorMenuItemType.EditableText:
                    break;
                case EditorMenuItemType.Dropdown:
                    break;
                case EditorMenuItemType.VerticalList:
                    break;
                case EditorMenuItemType.CompactList:
                    break;
                case EditorMenuItemType.Selectable:
					if (IsClicked())
					{
						parent.cursor = ID;
					}
					break;
				case EditorMenuItemType.AngleKnob:
                    break;
                case EditorMenuItemType.Spinner:
                    break;
                default:
                    break;
            }
        }

		/// <summary>
		/// Called when something is changed and the clickbox (or something else) needs to be adjusted
		/// </summary>
		public virtual void Refresh()
        {

        }

		public EditorUIItem(EditorMenu parent, int ID)
		{
			this.parent = parent;
			this.ID = ID;
			children = new EditorUIItem[0];
			Refresh();
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
					return parent.cursor == ID && new List<EditorUIItem>(parent.items).Contains(this);

				if (!new List<EditorUIItem>(parent.CurrentItemPool).Contains(this))
					return false;

				return parent.cursor == ID && parent.escapeRoute.Peek().Selected;
			}
		}

		public EditorMenu parent;
		public int ID;
		public bool lowestLevel;
		public bool button;
		public EditorMenuItemType type;
		public bool open;
		public EditorUIItem[] children;
		public Rectangle clickbox;

		public bool disabled;

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
