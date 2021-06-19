using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Graphics
{
	public static class ColorTracker
	{
		/// <summary>
		/// Filters out all magenta pixels from a texture.
		/// </summary>
		/// <param name="texture">The texture to depinkify.</param>
		public static void Depinkify(ref Texture2D texture)
		{
			StripColors(ref texture, new Color[] { Color.Magenta });
		}

		/// <summary>
		/// Filters out all pixels that match one of any specified colors to remove from a texture.
		/// </summary>
		/// <param name="texture">The texture to strip colors from.</param>
		/// <param name="toRemove"></param>
		public static void StripColors(ref Texture2D texture, Color[] toRemove)
		{
			Color[] textureData = new Color[texture.Width * texture.Height];
			texture.GetData(textureData);
			foreach (Color color in toRemove)
			{
				for (int i = 0; i < textureData.Length; i++)
				{
					if (textureData[i] == color)
					{
						textureData[i] = Color.Transparent;
					}
				}
			}
			texture.SetData(textureData);
		}

		/// <summary>
		/// Locates colors in a texture. Outputs information on the colors to locate.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="toTrack">The colors to locate within the texture.</param>
		/// <param name="mapped">A 2-dimensional array with the same dimensions as the texture containing only the tracked colors.</param>
		/// <param name="tracked">A dictionary containing entries for each tracked colors containing a list of positions of pixels.</param>
		/// <param name="removeTracked">Whether or not to filter tracked colors from the texture.</param>
		public static void TrackColors(ref Texture2D texture, Color[] toTrack, out Color[,] mapped, out Dictionary<Color, List<Point>> tracked, bool removeTracked = false)
		{
			mapped = new Color[texture.Width, texture.Height];
			tracked = new Dictionary<Color, List<Point>>();

			Color[] textureData = new Color[texture.Width * texture.Height];
			texture.GetData(textureData);
			foreach (Color color in toTrack)
			{
				tracked.Add(color, new List<Point>());
				for (int i = 0; i < textureData.Length; i++)
				{
					if (textureData[i] == color)
					{
						Point position = new Point(i % texture.Width, i / texture.Width);

						mapped[position.X, position.Y] = color;
						tracked[color].Add(position);

						if (removeTracked)
							textureData[i] = Color.Transparent;
					}
				}
			}

			if (removeTracked)
				texture.SetData(textureData);
		}
	}
}
