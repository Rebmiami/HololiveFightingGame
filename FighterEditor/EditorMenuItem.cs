using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HololiveFightingGame.FighterEditor
{
    public abstract class EditorMenuItem
	{
		public virtual void Enter()
		{
			Editor.menuItem = this;
		}

		public virtual void Erase()
		{

		}

		public virtual void Edit()
		{

		}

		public virtual void Escape()
		{
			Editor.menuItem = null;
		}

		public bool selected;

		public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
		{

		}
	}
}
