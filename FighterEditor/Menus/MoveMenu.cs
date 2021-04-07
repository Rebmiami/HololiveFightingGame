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
							spriteBatch.DrawString(Assets.font, move.Data.Name, new Vector2(130, 8 + count * 16), Color.White);
							count++;
						}
					}
					if (count == 0)
					{
						spriteBatch.DrawString(Assets.font, "No moves.", new Vector2(130, 8), Color.White);
					}
					name = "> " + name;
				}
				spriteBatch.DrawString(Assets.font, name, new Vector2(8, 8 + i * 16), Color.White);
			}
			base.Draw(spriteBatch, rightMenu);
        }
    }
}
