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
				return KeyHelper.Pressed(ProfileBinder.profile.Jump.key);
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
				return KeyHelper.Pressed(ProfileBinder.profile.Attack.key);
			}
			else
			{
				return GamePadHelper.Pressed(tapAtkNormalBind_Pad, gamepadNumber);
			}
		}

		public static Buttons tapAtkSpecialBind_Pad = Buttons.X;

		public static bool TapAtkSpecial(bool keyboard, int gamepadNumber)
		{
			if (keyboard)
			{
				return KeyHelper.Pressed(ProfileBinder.profile.AttackB.key);
			}
			else
			{
				return GamePadHelper.Pressed(tapAtkSpecialBind_Pad, gamepadNumber);
			}
		}

		public static Buttons holdHorizMoveBindL_Pad = 0;
		public static Buttons holdHorizMoveBindR_Pad = 0;
		public static Buttons holdHorizMoveBindU_Pad = 0;
		public static Buttons holdHorizMoveBindD_Pad = 0;
		public static bool? holdHorizMoveBindStick_IsRight_Pad = false;

		public static Vector2 ControlDirection(bool keyboard, int gamepadNumber)
		{
			if (keyboard)
			{
				float x = 0;
				float y = 0;
				if (KeyHelper.Down(ProfileBinder.profile.ControlLeft.key))
				{
					x -= 1;
				}
				if (KeyHelper.Down(ProfileBinder.profile.ControlRight.key))
				{
					x += 1;
				}
				if (KeyHelper.Down(ProfileBinder.profile.ControlUp.key))
				{
					y -= 1;
				}
				if (KeyHelper.Down(ProfileBinder.profile.ControlDown.key))
				{
					y += 1;
				}
				return new Vector2(x, y);
			}
			else
			{
				if (holdHorizMoveBindStick_IsRight_Pad == null)
				{
					float x = 0;
					float y = 0;
					if (GamePadHelper.Down(holdHorizMoveBindL_Pad, gamepadNumber))
					{
						x -= 1;
					}
					if (GamePadHelper.Down(holdHorizMoveBindR_Pad, gamepadNumber))
					{
						x += 1;
					}
					if (GamePadHelper.Down(holdHorizMoveBindU_Pad, gamepadNumber))
					{
						y -= 1;
					}
					if (GamePadHelper.Down(holdHorizMoveBindD_Pad, gamepadNumber))
					{
						y += 1;
					}
					return new Vector2(x, y);
				}
				return GamePadHelper.ThumbSticks(gamepadNumber, (bool)holdHorizMoveBindStick_IsRight_Pad) * new Vector2(1, -1);
			}
		}
	}
}
