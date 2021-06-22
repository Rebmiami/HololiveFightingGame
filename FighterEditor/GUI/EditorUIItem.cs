using HololiveFightingGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HololiveFightingGame.FighterEditor.GUI
{
	public abstract class EditorUIItem
	{
		/// <summary>
		/// Called when pressing Enter while a mid-level menu item is selected. Allows the user to select menu items within this item. <br />
		/// Not to be confused with pressing Enter to confirm edits - see <see cref="Escape"/>
		/// </summary>
		public virtual void Enter()
		{
			parentMenu.escapeRoute.Push(this);
			parentMenu.cursor = 0;
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
				parentMenu.cursor = parentMenu.escapeRoute.Pop().ID;
			}
		}

		public void ComeOverHere()
		{
			parentMenu.escapeRoute.Clear();
			parentMenu.escapeRoute = new Stack<EditorUIItem>(PathToMe());
		}

		public EditorUIItem[] PathToMe()
		{
			EditorUIItem leParent = parent;
			List<EditorUIItem> path = new List<EditorUIItem>();
			while (leParent != null)
			{
				path.Add(leParent);
				leParent = leParent.parent;
			}
			path.Reverse();
			return path.ToArray();
		}

		/// <summary>
		/// Called every frame.
		/// </summary>
		public virtual void Update()
		{
			if (!onlyUpdateChildrenIfSelected || Selected || Highlighted)
				foreach (EditorUIItem item in children)
				{
					item.Update();
				}

			if (!disabled)
			{
				switch (type)
				{
					case EditorMenuItemType.Button:
						if (IsClicked())
						{
							object obj = new object();
							parentMenu.cursor = ID;
							Escape(ref obj);
						}
						break;
					case EditorMenuItemType.Hoverable:
						if (Hovering())
						{
							parentMenu.cursor = ID;
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
							parentMenu.cursor = ID;
							ComeOverHere();
							Enter();
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
		}

		/// <summary>
		/// Called when something is changed and various values need to be adjusted.
		/// </summary>
		public virtual void Refresh()
		{
			foreach (EditorUIItem child in children)
			{
				child.parent = this;
				child.Refresh();
			}
		}

		public EditorUIItem(EditorMenu parentMenu, int ID)
		{
			this.parentMenu = parentMenu;
			this.ID = ID;
			children = new EditorUIItem[0];
			Refresh();
		}

		public bool Selected
		{
			get
			{
				return parentMenu.escapeRoute.Contains(this);
			}
		}

		public bool Highlighted
		{
			get
			{
				if (Selected)
					return false;
				if (parentMenu.escapeRoute.Count == 0)
					return parentMenu.cursor == ID && new List<EditorUIItem>(parentMenu.items).Contains(this);

				if (!new List<EditorUIItem>(parentMenu.CurrentItemPool).Contains(this))
					return false;

				return parentMenu.cursor == ID && parentMenu.escapeRoute.Peek().Selected;
			}
		}

		public EditorMenu parentMenu;
		public EditorUIItem parent;
		public int ID;
		public bool lowestLevel;
		public EditorMenuItemType type;
		public ButtonFlavor flavor;
		public bool open;
		public EditorUIItem[] children;
		public Rectangle clickbox;
		public bool onlyUpdateChildrenIfSelected;

		public bool disabled;

		public string helpText;
		public string helpArticle;

		public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, ref bool drawChildren)
		{
			Button.Draw(spriteBatch, clickbox, (ButtonFlavor)Flavor);
			if (drawChildren)
				for (int j = 0; j < children.Length; j++)
				{
					var menuItem = children[j];
					bool childDrawChildren = true;
					menuItem.Draw(spriteBatch, position, ref childDrawChildren);
				}
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
			return Hovering() && MouseHelper.Pressed(MouseButtons.Left);
		}

		public void Clicked()
		{

		}
	}
}
