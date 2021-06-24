using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static HololiveFightingGame.Loading.AnimationSetData;

namespace HololiveFightingGame.Graphics
{
	/// <summary>
	///		<see cref="SlicedSprite"/> with additional functionality for animation.
	/// </summary>
	public class AnimatedSprite : SlicedSprite
	{
		public static readonly int animFrameLength = 10;
		// "Frames per frame" rather than FPS - calculate 60 divided by this to get FPS

		public Dictionary<string, Animation> animations;
		public string currentAnim = "neutral";

		public Animation Playing
		{
			get
			{
				return animations[currentAnim];
			}
		}

		public Point dimensions;

		public AnimatedSprite(Texture2D texture, Point dimensions) : base(texture)
		{
			this.dimensions = dimensions;
		}

		public void Update()
		{
			Animation animation = Playing;
			if (animation.AutoAnimate)
			{
				animation.progress++;

				if (animation.Frame >= animation.Frames)
				{
					if (animation.NextAnim == null)
						animation.progress--;
					else
						SetAnimation(animation.NextAnim, 0);
				}
			}
		}

		public void SetAnimFrames(List<AnimationData> animationData) // Called once after animations are set up to set all the frames.
		{
			slices = new Dictionary<string, Rectangle>();
			foreach (string key in animations.Keys)
			{
				Animation animation = animations[key];
				AnimationData data = animationData[animation.AnimID];
				for (int i = 0; i < data.length; i++)
				{
					slices.Add(key + i, new Rectangle((int)data.Origin.X, (int)data.Origin.Y + (int)data.FrameSize.Y * i, (int)data.FrameSize.X, (int)data.FrameSize.Y));
				}
			}
		}

		public string GetFrame()
		{
			return currentAnim + Playing.Frame;
		}

		/// <summary>
		/// Sets the <see cref="AnimatedSprite"/> to the specified animation and frame.
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="startFrame"></param>
		public void SetAnimation(string animation, int startFrame)
		{
			currentAnim = animation;
			Playing.progress = startFrame * (Playing.AutoAnimate ? animFrameLength : 1);
		}

		/// <summary>
		/// Sets the <see cref="AnimatedSprite"/> to the specified animation and frame if the animation specified is not currently running.
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="startFrame"></param>
		public void SwitchAnimation(string animation, int startFrame)
		{
			if (currentAnim != animation)
			{
				currentAnim = animation;
				Playing.progress = startFrame * (Playing.AutoAnimate ? animFrameLength : 1);
			}
		}

		// TODO: Combine SetAnimation and SwitchAnimation to a single method.
	}
}
