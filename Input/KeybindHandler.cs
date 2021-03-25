using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Input
{
	class KeybindHandler
	{
		public static Buttons tapJumpBind_Pad = Buttons.A;

		public static bool TapJump(bool keyboard, int gamepadNumber)
		{
			if (keyboard)
			{
				return KeyHelper.Pressed(ProfileBinder.profile.Keyboard_Jump);
			}
			else
			{
				return GamePadHelper.Pressed(tapJumpBind_Pad, gamepadNumber);
			}
		}

		public static Buttons tapAtkNormalBind_Pad = Buttons.B;

		public static bool TapAtkNormal(bool keyboard, int gamepadNumber)
		{
			if (keyboard)
			{
				return KeyHelper.Pressed(ProfileBinder.profile.Keyboard_Attack);
			}
			else
			{
				return GamePadHelper.Pressed(tapAtkNormalBind_Pad, gamepadNumber);
			}
		}

		public static Buttons holdHorizMoveBindL_Pad = 0;
		public static Buttons holdHorizMoveBindR_Pad = 0;
		public static bool? holdHorizMoveBindStick_IsRight_Pad = false;

		public static float HoldHorizMove(bool keyboard, int gamepadNumber)
		{
			if (keyboard)
			{
				float val = 0;
				if (KeyHelper.Down(ProfileBinder.profile.Keyboard_MoveLeft))
				{
					val -= 1;
				}
				if (KeyHelper.Down(ProfileBinder.profile.Keyboard_MoveRight))
				{
					val += 1;
				}
				return val;
			}
			else
			{
				if (holdHorizMoveBindStick_IsRight_Pad == null)
				{
					float val = 0;
					if (GamePadHelper.Down(holdHorizMoveBindL_Pad, gamepadNumber))
					{
						val -= 1;
					}
					if (GamePadHelper.Down(holdHorizMoveBindR_Pad, gamepadNumber))
					{
						val += 1;
					}
					return val;
				}
				return GamePadHelper.ThumbSticks(gamepadNumber, (bool)holdHorizMoveBindStick_IsRight_Pad).X;
			}
		}
	}
}
