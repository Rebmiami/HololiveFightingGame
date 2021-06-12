﻿using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.Menus
{
	public class AnimationEditor : EditorMenu
	{
		public override void Draw(SpriteBatch spriteBatch, Point position)
		{
			Vector2 origin = position.ToVector2();
			spriteBatch.DrawString(Assets.font, "Animation Editor", origin, Color.White);
		}
	}
}
