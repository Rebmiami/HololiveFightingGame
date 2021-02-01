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
			Vector2 cameraTarget = Vector2.Lerp(fighters[0].Center + fighters[0].velocity * 10, new Vector2(stage.collider.Center.X, stage.collider.Top), 0.5f);
			GraphicsHandler.main.children["game"].position = -(Vector2.Lerp(cameraTarget, -GraphicsHandler.main.children["game"].position + Program.WindowBounds().Size.ToVector2() / 2, 0.9f) - Program.WindowBounds().Size.ToVector2() / 2);
		}
	}
}
