using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HololiveFightingGame.Loading
{
    public static class AnimationSetData
	{
		public class AnimationData
		{
			public VectorLoader Origin { get; set; }
			public VectorLoader FrameSize { get; set; }

			public VectorLoader Foot { get; set; }
			public int length { get; set; }
		}

		/// <summary>
		/// Creates a list of AnimationData from JSON.
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public static List<AnimationData> GetAnimations(string json)
		{
			return JsonSerializer.Deserialize<List<AnimationData>>(json);
		}

		/// <summary>
		/// Converts a list of AnimationData to JSON.
		/// </summary>
		/// <param name="animations"></param>
		/// <returns></returns>
		public static string ToJSON(List<AnimationData> animations, JsonSerializerOptions options = null)
		{
			return JsonSerializer.Serialize(animations, options);
		}

		/// <summary>
		/// Constructs a list of AnimationData from a frame-marked texture.
		/// </summary>
		/// <param name="texture"></param>
		/// <returns></returns>
		public static List<AnimationData> ConstructAnimations(Texture2D texture)
		{
			List<AnimationData> data = new List<AnimationData>();
			// Locates pixels of relevant colors.
			ColorTracker.TrackColors(ref texture, new Color[] { Color.Lime, Color.Red, Color.Yellow, Color.Blue }, out Color[,] colors, out Dictionary<Color, List<Point>> positions, true);
			// Constructs animations using these pixels using the position of green pixels, which indicate an animation.
			foreach (Point point in positions[Color.Lime])
			{
				data.Add(GetData(point, colors));
			}
			return data;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="colors"></param>
		/// <returns></returns>
		private static AnimationData GetData(Point point, Color[,] colors)
		{
			AnimationData data = new AnimationData();
			data.Origin = new VectorLoader(point.X, point.Y);
			data.FrameSize = new VectorLoader(0, 0);
			data.Foot = new VectorLoader(0, 0);

			bool yFootNotSet = true;
			bool xFootNotSet = true;


			// Finds the height of the frame by searching downward from the origin until reaching a red or blue dot.
			// Also finds the Y position of the entity's foot by watching for a yellow dot along the way.
			while (colors[point.X, (int)data.FrameSize.Y + point.Y] != Color.Red && colors[point.X, (int)data.FrameSize.Y + point.Y] != Color.Blue)
			{
				// If a yellow pixel hasn't yet been found, move the Y position of the foot down.
				if (yFootNotSet)
				{
					data.Foot.Y++;
					if (colors[point.X, (int)data.FrameSize.Y + point.Y] == Color.Yellow)
					{
						yFootNotSet = false;
					}
				}

				data.FrameSize.Y++;
			}

			// If the dot is blue, we already know that the animation has a length of 1 and we do not need to search for it again.
			if (colors[point.X, (int)data.FrameSize.Y + point.Y] == Color.Blue)
			{
				data.length = 1;
			}

			// The height of the frame should be extended one pixel longer, as the frame includes the height of the red/blue pixel.
			data.FrameSize.Y++;


			// Finds the width of the frame by searching rightward from the origin until reaching a red dot.
			// Also finds the X position of the entity's foot by watching for a yellow dot along the way.
			while (colors[(int)data.FrameSize.X + point.X, point.Y] != Color.Red)
			{
				// If a yellow pixel hasn't yet been found, move the X position of the foot right.
				if (xFootNotSet)
				{
					data.Foot.X++;
					// The yellow pixel appears at the bottom of the frame, which isn't where we're looking for the red pixel.
					// But we already know the height of the frame, so we can look along the bottom of the frame while finding its width.
					if (colors[(int)data.FrameSize.X + point.X, (int)data.FrameSize.Y + point.Y - 1] == Color.Yellow)
					{
						xFootNotSet = false;
					}
				}

				data.FrameSize.X++;
			}
			// The width of the frame should be extended one pixel longer, as the frame includes the width of the red pixel.
			data.FrameSize.X++;

			// Only look for the length of the animation if we don't already know it.
			if (data.length == 0)
			{
				// Checks for blue pixels at the position they would be found at the bottom of a frame and uses this to find the animation length in frames.
				// A blue pixel is at the bottom left corner of the last frame of the animation.
				// If the length is 0, then the check may cause an error. Incrementation should occur before the check.
				do
				{
					data.length++;
				} 
				while (colors[point.X, (int)data.FrameSize.Y * data.length + point.Y - 1] != Color.Blue);
			}

			// TODO: Handle failures caused by bad pixel placement on user's end.
			// Diagnose error, give user feedback, and abort without program crashing.

			return data;
		}

		[Serializable]
		public class AnimationConstructionException : Exception
		{
			public AnimationConstructionException() { }
			public AnimationConstructionException(string message) : base(message) { }
			public AnimationConstructionException(string message, Exception inner) : base(message, inner) { }
			protected AnimationConstructionException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}
	}
}
