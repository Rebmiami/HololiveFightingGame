using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HololiveFightingGame.Graphics
{
	public class SlicedSprite
	{
		public Texture2D texture;
		public Dictionary<string, Rectangle> slices;

		public SlicedSprite(Texture2D texture)
		{
			this.texture = texture;
			slices = new Dictionary<string, Rectangle>() { { "sprite", texture.Bounds } };
		}
	}
}
