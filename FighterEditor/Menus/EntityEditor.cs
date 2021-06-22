using HololiveFightingGame.FighterEditor.GUI;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.Menus
{
    public class EntityEditor : EditorMenu
	{
		public override void Draw(SpriteBatch spriteBatch, Point position)
		{
			Vector2 origin = position.ToVector2();
			spriteBatch.DrawString(Assets.font, "Entity Editor", origin, Color.White);
		}
	}
}
