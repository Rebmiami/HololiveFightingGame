using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HololiveFightingGame.Graphics
{
	public static class ImageLoader
	{
		/// <summary>
		/// Loads a <see cref="Texture2D"/> from the specified path.
		/// </summary>
		/// <param name="path">The path to load the image from (relative to the executable directory).</param>
		/// <param name="depinkify">Whether or not to replace magenta (255,0,255,255) with transparency.</param>
		/// <returns></returns>
		public static Texture2D LoadTexture(string path, bool depinkify = false)
		{
			Texture2D texture;
			using (var filestream = new FileStream(path, FileMode.Open))
			{
				texture = Texture2D.FromStream(Program.game.GraphicsDevice, filestream);
			}

			if (depinkify)
			{
				ColorTracker.Depinkify(ref texture);
			}

			return texture;
		}
	}
}
