using HololiveFightingGame.Combat;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.Menus
{
	class MoveMenu : EditorMenu
	{
		public MoveMenu()
		{
			itemCount = Enum.GetNames(typeof(MoveType)).Length;
		}

		public override void Draw(SpriteBatch spriteBatch, bool rightMenu)
		{
			string[] attackNames = Enum.GetNames(typeof(MoveType));

			Vector2 origin = new Vector2(8, 8);
			if (rightMenu)
			{
				origin.X += 546;
			}

			for (int i = 0; i < attackNames.Length; i++)
			{
				string name = attackNames[i];
				if (cursor == i)
				{
					int count = 0;
					foreach (Move move in FighterLoader.moves[Editor.fighter.character].Values)
					{
						if (move.Data.Name.Contains(name))
						{
							Button.Draw(spriteBatch, new Rectangle((int)origin.X - 2 + 120, (int)origin.Y - 2 + count * 20, 100, 20), cursor == i ? 1 : 0);
							spriteBatch.DrawString(Assets.font, move.Data.Name, origin + new Vector2(120, count * 20), Color.White);
							count++;
						}
					}
					if (count == 0)
					{
						spriteBatch.DrawString(Assets.font, "No moves.", origin + new Vector2(120, 0), Color.White);
					}
				}
				Button.Draw(spriteBatch, new Rectangle((int)origin.X - 2, (int)origin.Y - 2 + i * 20, 100, 20), cursor == i ? 1 : 0);
				spriteBatch.DrawString(Assets.font, name, origin + new Vector2(0, i * 20), Color.White);
			}
			base.Draw(spriteBatch, rightMenu);
		}
	}
}
