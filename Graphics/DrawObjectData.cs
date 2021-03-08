using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Graphics
{
	/// <summary>
	///		Abstract class used for storing information about specific <see cref="DrawObject"/> types.
	/// </summary>
	public abstract class DrawObjectData
	{

	}

	/// <summary>
	///		Includes data for text <see cref="DrawObject"/>, including the string to draw and text color.
	/// </summary>
	public class TextData : DrawObjectData
	{
		public string text;
		public Color color;

		/// <summary>
		///		Creates a <see cref="TextData"/> with specified <see cref="string"/> and <see cref="Color"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="color"></param>
		public TextData(string text, Color color)
		{
			this.text = text;
			this.color = color;
		}

		/// <summary>
		///		Creates a <see cref="TextData"/> with specified <see cref="string"/> and white color.
		/// </summary>
		/// <param name="text"></param>
		public TextData(string text)
		{
			this.text = text;
			this.color = Color.White;
		}
	}

	/// <summary>
	///		Includes data for flash DrawObjects, including velocity and time left.
	/// </summary>
	public class FlashData : DrawObjectData
	{

	}
}
