using HololiveFightingGame.Combat;
using HololiveFightingGame.FighterEditor.MenuItems;
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
			attackNames = Enum.GetNames(typeof(MoveType));

			items = new EditorMenuItem[attackNames.Length];
			for (int i = 0; i < items.Length; i++)
			{
				items[i] = new MoveListItem(this, i, attackNames[i]);
			}
		}

		public string[] attackNames;

		public override void Draw(SpriteBatch spriteBatch, bool rightMenu)
		{
			Vector2 origin = new Vector2(8, 8);
			if (rightMenu)
			{
				origin.X += 546;
			}
			spriteBatch.DrawString(Assets.font, "Move Menu", origin, Color.White);
			origin.Y += 25;

			for (int i = 0; i < attackNames.Length; i++)
			{
				((MoveListItem)items[i]).Draw(spriteBatch, origin, i);
			}
			base.Draw(spriteBatch, rightMenu);
		}
	}
}
