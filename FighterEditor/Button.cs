using HololiveFightingGame.Graphics;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	public class Button
	{
		public static Rectangle[] Parts => new Rectangle[]
		{
			new Rectangle(0, 0, 2, 2),
			new Rectangle(3, 0, 3, 2),
			new Rectangle(7, 0, 2, 2),
			new Rectangle(0, 3, 2, 3),
			new Rectangle(3, 3, 3, 3),
			new Rectangle(7, 3, 2, 3),
			new Rectangle(0, 7, 2, 2),
			new Rectangle(3, 7, 3, 2),
			new Rectangle(7, 7, 2, 2)
		};

		public static void Draw(SpriteBatch spriteBatch, Rectangle dimensions, int flavor = 0)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					Rectangle destination = new Rectangle(dimensions.Location, new Point(2, 2));
					Rectangle source = Parts[i + j * 3];

					if (i == 1)
					{
						destination.Width = dimensions.Width - 4;
						destination.X += 2;
					}
					if (j == 1)
					{
						destination.Height = dimensions.Height - 4;
						destination.Y += 2;
					}
					if (i == 2)
					{
						destination.X += dimensions.Width - 2;
					}
					if (j == 2)
					{
						destination.Y += dimensions.Height - 2;
					}

					source.Y += flavor * 10;

					spriteBatch.Draw(Assets.editorButton, destination, source, Color.White); //, 0, Vector2.Zero, SpriteEffects.None, 0);
				}
			}
		}
	}
}
