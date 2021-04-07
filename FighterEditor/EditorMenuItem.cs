using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
		/// Clears the item's value and allows the user to input a new one. <br />
		/// If the item is meant to be a button without an "editing" state, call <see cref="Escape"/> within this method.
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
		/// Called when pressing Escape to exit a mid-level menu item or pressing Enter to confirm edits made to a lowest-level menu item.
		/// </summary>
		public virtual void Escape()
		{
			if (!lowestLevel)
			{
				parent.escapeRoute.Pop();
				parent.cursor = ID;
			}
		}

		public bool selected;
		public bool highlighted;
		public EditorMenu parent;
		public int ID;
		public bool lowestLevel;
		public EditorMenuItem[] children;

		public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
		{

		}
	}
}
