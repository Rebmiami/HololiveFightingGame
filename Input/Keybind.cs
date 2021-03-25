using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using HololiveFightingGame.Input;

namespace HololiveFightingGame.Input
{
	public class Keybind
	{
		public bool isKey = true;
		public Keys key;
		public MouseButtons mouse;
		// Keyboard and mouse are treated as one controller, so only one of these needs to be specified.

		public Buttons button;

		public bool Down(bool keyboard, int gamepad)
		{
			if (keyboard)
			{
				if (isKey)
				{
					return KeyHelper.Down(key);
				}
				else
				{
					return MouseHelper.Down(mouse);
				}
			}
			else
			{
				return GamePadHelper.Down(button, gamepad);
			}
		}
	}
}
