﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	public static class MovePreviewer
	{
		public static int Frame = 0;
		public static bool Playing = false;
		public static bool Reverse = false;
		public static float Speed = 1f;

		public static Vector2 Pan = Vector2.Zero;
		public static int Zoom = 1;

		public static void TakeCommandInputs()
		{

		}

		public static void Update()
		{
			Editor.fighter.Update();
		}

		public static void AdvanceFrame()
		{

		}

		public static void SetFrame(int frame)
		{

		}

		public static void Refresh()
		{

		}
	}
}
