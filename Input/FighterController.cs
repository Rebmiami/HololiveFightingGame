using HololiveFightingGame.Gameplay.Combat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Input
{
	public class FighterController
	{
		public Fighter fighter;
		public ControlProfile profile;

		/// <summary>
		/// Historic positions of the control stick. (gamepad only)
		/// </summary>
		public Vector2[] historicStick;

		/// <summary>
		/// Number of frames remaining in current flick.
		/// </summary>
		public int flick;

		/// <summary>
		/// Number of frames since last direction key was released. (keyboard only)
		/// </summary>
		public int lastTap;

		/// <summary>
		/// The direction the stick is being held in, exclusively cardinal directions and neutral.
		/// </summary>
		public Vector2 direction4;

		public FighterController(Fighter fighter, ControlProfile profile)
		{
			this.fighter = fighter;
			this.profile = profile;

			historicStick = new Vector2[2] { Vector2.Zero, Vector2.Zero };
			flick = 0;
			lastTap = 0;
			direction4 = Vector2.Zero;
		}

		public void Update()
		{
			if (flick > 0)
			{
				flick--;
			}

			if (fighter.keyboard)
			{
				if (lastTap > 0)
				{
					lastTap--;
				}

				if (KeyHelper.AnyReleased(new Keys[] { profile.ControlDown.key, profile.ControlUp.key, profile.ControlLeft.key, profile.ControlRight.key }))
				{
					lastTap = 6;
					flick = 0;
				}

				if (lastTap != 0 && KeyHelper.AnyPressed(new Keys[] { profile.ControlDown.key, profile.ControlUp.key, profile.ControlLeft.key, profile.ControlRight.key }))
				{
					lastTap = 0;
					flick = 4;
				}
			}
			else
			{
				Vector2 currentStick = GamePadHelper.ThumbSticks(fighter.ID, false);
				float totalDistance = Vector2.Distance(historicStick[1], historicStick[0]) + Vector2.Distance(historicStick[0], currentStick);

				if (totalDistance > 0.5f && currentStick.Length() > 0.9f)
                {
					flick = 4;
                }

				historicStick[1] = historicStick[0];
				historicStick[0] = currentStick;
			}

			Vector2 direction = KeybindHandler.ControlDirection(fighter.keyboard, fighter.ID);
			Vector2 newDirection4;
			if (direction == Vector2.Zero)
			{
				newDirection4 = Vector2.Zero;
			}
			else if (-Math.Abs(direction.X) >= direction.Y)
			{
				newDirection4 = new Vector2(0, -1);
			}
			else if (Math.Abs(direction.X) < direction.Y)
			{
				newDirection4 = new Vector2(0, 1);
			}
			else if (direction.X > 0)
			{
				newDirection4 = new Vector2(1, 0);
			}
			else
			{
				newDirection4 = new Vector2(-1, 0);
			}

			if (direction4 != newDirection4)
            {
				direction4 = newDirection4;
				// flick = 0;
            }
		}
	}
}
