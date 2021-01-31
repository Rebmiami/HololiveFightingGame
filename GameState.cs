using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	public class GameState
	{
		public Fighter[] fighters;
		public Stage stage;

		public GameState()
		{
			fighters = new Fighter[1] { new Fighter() };
			stage = new Stage();
		}

		public void Update()
		{
			foreach (Fighter fighter in fighters)
			{
				fighter.Update();
			}
			GraphicsHandler.main.children["game"].position = -(Vector2.Lerp(fighters[0].Center, -GraphicsHandler.main.children["game"].position + Program.WindowBounds().Size.ToVector2() / 2, 0.9f) - Program.WindowBounds().Size.ToVector2() / 2);
		}
	}
}
