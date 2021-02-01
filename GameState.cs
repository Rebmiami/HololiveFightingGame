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
			fighters = new Fighter[2] { new Fighter(0), new Fighter(1) };
			stage = new Stage();
		}

		public void Update()
		{
			Vector2 playerCenter = Vector2.Zero;
			foreach (Fighter fighter in fighters)
			{
				fighter.Update();
				playerCenter += fighter.Center + fighter.velocity * 10;
			}
			playerCenter /= fighters.Length;
			Vector2 cameraTarget = Vector2.Lerp(playerCenter, new Vector2(stage.collider.Center.X, stage.collider.Top), 0.5f);
			GraphicsHandler.main.children["game"].position = -(Vector2.Lerp(cameraTarget, -GraphicsHandler.main.children["game"].position + Program.WindowBounds().Size.ToVector2() / 2, 0.9f) - Program.WindowBounds().Size.ToVector2() / 2);
		}
	}
}
