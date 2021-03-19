using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Graphics
{
	/// <summary>
	///		Specifies animations to be played by the <see cref="AnimatedSprite"/>.
	/// </summary>
	public class Animation
	{
		public string NextAnim { get; set; } // If null, will halt on the last frame of the animation until a new animation is started.

		public int AnimID { get; set; }
		public int Frames { get; set; } // Length of animation in frames.

		public bool AutoAnimate { get; set; }
		// If true, the animation will automatically progress through frames at a set framerate.
		// If false, the frame must be set by any other arbitrary class.

		public int progress = 0;

		public int Frame
		{
			get
			{
				return progress / (AutoAnimate ? AnimatedSprite.animFrameLength : 1);
			}
			set
			{
				progress = value * (AutoAnimate ? AnimatedSprite.animFrameLength : 1);
			}
		}

		public Animation()
        {

        }

		public Animation(int id, int length, bool auto, string next = null)
		{
			AnimID = id;
			Frames = length;
			AutoAnimate = auto;
			NextAnim = next;
		}
	}
}
