using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Graphics
{
	public class AnimatedSprite : SlicedSprite
	{
		public static readonly int animFrameLength = 10; // "Frames per frame" rather than FPS - calculate 60 divided by this to get FPS

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

		public class Animation
		{
			public string nextAnim; // If null, will halt on the last frame of the animation until a new animation is started.

			public int animID;
			public int frames; // Length of animation in frames.

			public bool autoAnimate;
			// If true, the animation will automatically progress through frames at a set framerate.
			// If false, the frame must be set by any other arbitrary class.

			public int progress;

			public int Frame
			{
				get
				{
					return progress / (autoAnimate ? animFrameLength : 1);
				}
				set
				{
					progress = value * (autoAnimate ? animFrameLength : 1);
				}
			}

			public Animation(int id, int length, bool auto, string next = null)
			{
				animID = id;
				frames = length;
				autoAnimate = auto;
				nextAnim = next;
			}
		}

		public void Update()
		{
			Animation animation = Playing;
			if (animation.autoAnimate)
			{
				animation.progress++;

				if (animation.Frame >= animation.frames)
				{
					if (animation.nextAnim == null)
						animation.progress--;
					else
						SetAnimation(animation.nextAnim, 0);
				}
			}
		}

		public void SetAnimFrames() // Called once after animations are set up to set all the frames. In the case of flashes, pre-set animations are used to save resources.
		{
			slices = new Dictionary<string, Rectangle>();
			foreach (string key in animations.Keys)
			{
				Animation animation = animations[key];
				for (int i = 0; i < animation.frames; i++)
				{
					slices.Add(key + i, new Rectangle(dimensions.X * animation.animID, dimensions.Y * i, dimensions.X, dimensions.Y));
				}
			}
		}

		public string GetFrame()
		{
			return currentAnim + Playing.Frame;
		}

		public void SetAnimation(string animation, int startFrame)
		{
			currentAnim = animation;
			Playing.progress = startFrame * (Playing.autoAnimate ? animFrameLength : 1);
		}

		public void SwitchAnimation(string animation, int startFrame)
		{
			if (currentAnim != animation)
			{
				currentAnim = animation;
				Playing.progress = startFrame * (Playing.autoAnimate ? animFrameLength : 1);
			}
		}
	}
}
