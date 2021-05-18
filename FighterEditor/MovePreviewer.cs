using HololiveFightingGame.Combat;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	public static class MovePreviewer
	{
		public static int Frame = 0;
		public static bool Playing = false;
		public static float Speed = 1f;
		public static bool Buffer = false;

		public static Vector2 Pan = Vector2.Zero;
		public static int Zoom = 1;


		private static float step;

		public static void TakeCommandInputs()
		{
			if (Editor.fighter.moveRunner != null)
				if (KeyHelper.Pressed(Keys.Space))
				{
					Playing = !Playing;
					if (Frame >= Editor.fighter.moveRunner.data.MoveDuration)
					{
						Reset();
					}
				}
		}

		public static void Update()
		{
			TakeCommandInputs();

			if (Playing)
			{
				step += Speed;

				AdvanceFrame();
				if (Editor.fighter.moveRunner == null)
				{
					Playing = false;
				}
			}
		}

		public static void AdvanceFrame()
		{
			Editor.fighter.Update();
			Editor.fighter.Update_PostHit(Editor.fighter.Update_Hits());
			Editor.fighter.Update_Animation();
			Frame++;
		}

		public static void SetFrame(int frame)
		{
			int frames = Frame;
			Reset();
			for (int i = 0; i < frames; i++)
			{
				AdvanceFrame();
			}
		}

		public static void Refresh()
		{
			SetFrame(Frame);
		}

		public static void Reset()
		{
			Frame = 0;
			Editor.ResetGraphics();
			Editor.fighter = new Fighter(Editor.fighter.ID, Editor.fighter.character)
            {
                moveRunner = new Combat.MoveRunner(Editor.currentMove)
            };
			FighterLoader.LoadAnimations(new Fighter[] { Editor.fighter });
			Editor.fighter.Bottom = Program.WindowBounds().Size.ToVector2() / 2;
			Editor.fighter.takeInputs = false;
			((AnimatedSprite)Editor.fighter.drawObject.texture).SwitchAnimation(Editor.currentMove.Data.Name, 0);
			Editor.fighter.moveTimer = Editor.fighter.moveRunner.data.MoveDuration;
		}
	}
}
