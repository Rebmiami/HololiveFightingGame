using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Input
{
	class Keybinds
	{
		public static Keys tapJumpBind_Key = Keys.W;
		public static Buttons tapJumpBind_Pad = Buttons.A;

		public static bool TapJump(bool keyboard, int gamepadNumber)
		{
			if (keyboard)
			{
				return KeyHelper.Pressed(tapJumpBind_Key);
			}
			else
			{
				return GamePadHelper.Pressed(tapJumpBind_Pad, gamepadNumber);
			}
		}


		public static Keys tapAtkNormalBind_Key = Keys.P;
		public static Buttons tapAtkNormalBind_Pad = Buttons.B;

		public static bool TapAtkNormal(bool keyboard, int gamepadNumber)
		{
			if (keyboard)
			{
				return KeyHelper.Pressed(tapAtkNormalBind_Key);
			}
			else
			{
				return GamePadHelper.Pressed(tapAtkNormalBind_Pad, gamepadNumber);
			}
		}

		public static Keys holdHorizMoveBindL_Key = Keys.A;
		public static Keys holdHorizMoveBindR_Key = Keys.D;
		public static Buttons holdHorizMoveBindL_Pad = 0;
		public static Buttons holdHorizMoveBindR_Pad = 0;
		public static bool? holdHorizMoveBindStick_IsRight_Pad = false;

		public static float HoldHorizMove(bool keyboard, int gamepadNumber)
		{
			if (keyboard)
			{
				float val = 0;
				if (KeyHelper.Down(holdHorizMoveBindL_Key))
				{
					val -= 1;
				}
				if (KeyHelper.Down(holdHorizMoveBindR_Key))
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
