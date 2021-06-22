using HololiveFightingGame.FighterEditor.GUI;
using HololiveFightingGame.FighterEditor.MenuItems;
using HololiveFightingGame.Gameplay.Combat;
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

			items = new EditorUIItem[attackNames.Length];
			for (int i = 0; i < items.Length; i++)
			{
				items[i] = new MoveListItem(this, i, attackNames[i]);
			}
		}

		public string[] attackNames;

		public override void Draw(SpriteBatch spriteBatch, Point position)
		{
			Vector2 origin = position.ToVector2();
			spriteBatch.DrawString(Assets.font, "Move Menu", origin, Color.White);
			origin.Y += EditorOffsets.headerHeight;

			for (int i = 0; i < attackNames.Length; i++)
			{
				bool childDrawChildren = true;
				((MoveListItem)items[i]).Draw(spriteBatch, origin, ref childDrawChildren);
			}
		}
	}
}
