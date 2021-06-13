using HololiveFightingGame.FighterEditor.MenuItems;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.Menus
{
	class FileMenu : EditorMenu
	{
		public FileMenu()
        {
			items = new EditorUIItem[9];
			for (int i = 0; i < 9; i++)
            {
				items[i] = new FileMenuButton(this, i);
            }
        }

		public override void Draw(SpriteBatch spriteBatch, Point position)
		{
			Vector2 origin = position.ToVector2();
			spriteBatch.DrawString(Assets.font, "File Menu", origin, Color.White);
		}
	}
}
