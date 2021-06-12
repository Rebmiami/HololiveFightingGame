using HololiveFightingGame.Combat;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.MenuItems
{
	public class MoveListItem : EditorUIItem
	{
		public string move;
		public string[] subMoves;

		public MoveListItem(EditorMenu parent, int ID, string move) : base(parent, ID)
		{
			this.move = move;
			List<string> subMoveList = new List<string>();
			foreach (Move move1 in FighterLoader.moves[Editor.fighter.character].Values)
			{
				if (move1.Data.Name.Contains(move))
				{
					subMoveList.Add(move1.Data.Name);
				}
			}
			subMoves = subMoveList.ToArray();
			children = new EditorUIItem[subMoves.Length];
			for (int i = 0; i < subMoves.Length; i++)
			{
				children[i] = new MoveListButtonItem(parent, i, subMoves[i]);
			}
			type = EditorMenuItemType.Selectable;
			onlyUpdateChildrenIfSelected = true;
		}

        public override void Refresh()
        {
			Vector2 origin = new Vector2(8, 8);
			// origin.X += 546;
			origin.Y += 25;
			origin.Y += 32;
			clickbox = new Rectangle((int)origin.X - 2, (int)origin.Y - 2 + ID * 20, 100, 20);
			base.Refresh();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int i)
		{
			if (Highlighted || Selected)
			{
				for (int j = 0; j < children.Length; j++)
				{
					MoveListButtonItem menuItem = (MoveListButtonItem)children[j];

					menuItem.Draw(spriteBatch, position, j);
				}
				if (children.Length == 0)
				{
					spriteBatch.DrawString(Assets.font, "No moves.", position + new Vector2(120, 0), Color.White);
				}
			}
			Button.Draw(spriteBatch, clickbox, (ButtonFlavor)Flavor);
			spriteBatch.DrawString(Assets.font, move, position + new Vector2(0, i * 20), Color.White);
		}
	}
}
