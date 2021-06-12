using HololiveFightingGame.Combat;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.MenuItems
{
	class MoveListButtonItem : EditorUIItem
	{
		public string move;

		public MoveListButtonItem(EditorMenu parent, int ID, string move) : base(parent, ID)
		{
			this.move = move;
			lowestLevel = true;
			type = EditorMenuItemType.Button;
		}

		public override void Escape(ref object target)
		{
			Editor.currentMove = FighterLoader.moves[Editor.fighter.character][move];
			Editor.ResetFighter();
			base.Escape(ref target);
		}

        public override void Refresh()
		{
			Vector2 origin = new Vector2(8, 8);
			// origin.X += 546;
			origin.Y += 25;
			origin.Y += 32;
			clickbox = new Rectangle((int)origin.X - 2 + 120, (int)origin.Y - 2 + ID * 20, 100, 20);
			base.Refresh();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int i)
		{
			Button.Draw(spriteBatch, new Rectangle((int)position.X - 2 + 120, (int)position.Y - 2 + i * 20, 100, 20), (ButtonFlavor)Flavor);
			spriteBatch.DrawString(Assets.font, move, position + new Vector2(120, i * 20), Color.White);
		}
	}
}
