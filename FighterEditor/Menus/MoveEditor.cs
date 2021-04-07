using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.Menus
{
	class MoveEditor : EditorMenu
	{
		public override void Draw(SpriteBatch spriteBatch, bool rightMenu)
		{
			Vector2 origin = new Vector2(8, 8);
			if (rightMenu)
			{
				origin.X += 550;
			}

			spriteBatch.DrawString(Assets.font, "Move Editor", origin, Color.White);
			base.Draw(spriteBatch, rightMenu);
		}
	}
}
