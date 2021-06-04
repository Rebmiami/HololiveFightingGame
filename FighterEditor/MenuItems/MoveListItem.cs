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
			type = EditorMenuItemType.Hoverable;
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
			Button.Draw(spriteBatch, new Rectangle((int)position.X - 2, (int)position.Y - 2 + i * 20, 100, 20), Flavor);
			spriteBatch.DrawString(Assets.font, move, position + new Vector2(0, i * 20), Color.White);
		}
	}
}
