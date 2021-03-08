using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Graphics
{
	public abstract class DrawObjectData
	{

	}

	public class TextData : DrawObjectData
	{
		public string text;
		public Color color;

		public TextData(string text, Color color)
		{
			this.text = text;
			this.color = color;
		}

		public TextData(string text)
		{
			this.text = text;
			this.color = Color.White;
		}
	}

	public class FlashData : DrawObjectData
	{

	}
}
