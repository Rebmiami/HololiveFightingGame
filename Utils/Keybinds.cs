using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	class Keybinds
	{
		public static Keys tapJumpBind_Key = Keys.Space;
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


		public static Keys tapAtkNormalBind_Key = Keys.Z;
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




	}
}
