﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Utils;

namespace HololiveFightingGame
{
	public class GameState
	{
		public int maxFighters = 4;
		public Fighter[] fighters;
		public Stage stage;
		public List<Projectile> projectiles;

		public GameState()
		{
			fighters = new Fighter[2] { new Fighter(0), new Fighter(1) };
			stage = new Stage();
			projectiles = new List<Projectile>();
			fighters[1].keyboard = true;
		}

		public void Update()
		{
			Vector2 playerCenter = Vector2.Zero;
			ListCleaner.CleanList(projectiles, delegate(Projectile projectile)
			{
				projectile.Update();
				return projectile.timeLeft <= 0;
			} );
			foreach (Fighter fighter in fighters)
			{
				fighter.Update();

				playerCenter += fighter.Center + fighter.velocity * 10;
			}
			foreach (Fighter fighter in fighters)
			{
				; // This doesn't cause an error?
			}
			foreach (Fighter fighter in fighters)
			{
				fighter.Update_PostHit(fighter.Update_Hits());
			}
			foreach (Fighter fighter in fighters)
			{
				fighter.Update_Animation();
			}
			playerCenter /= fighters.Length;
			Vector2 cameraTarget = Vector2.Lerp(playerCenter, new Vector2(stage.collider.Rectangle.Center.X, stage.collider.Rectangle.Top), 0.5f);
			GraphicsHandler.main.children["game"].position = -(Vector2.Lerp(cameraTarget, -GraphicsHandler.main.children["game"].position + Program.WindowBounds().Size.ToVector2() / 2, 0.9f) - Program.WindowBounds().Size.ToVector2() / 2);
		}
	}
}
