using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Utils;
using HololiveFightingGame.Gameplay.Combat;

namespace HololiveFightingGame.Gameplay
{
    public class GameState
	{
		public static readonly int maxFighters = 4;
		public Fighter[] fighters;
		public Stage stage;
		public List<Projectile> projectiles;

		public GameState(string[] fighterNames)
		{
			fighters = new Fighter[fighterNames.Length];
			for (int i = 0; i < fighterNames.Length; i++)
			{
				fighters[i] = new Fighter(i, fighterNames[i]);
			}
			stage = new Stage();
			projectiles = new List<Projectile>();
			fighters[1].keyboard = true;
		}

		public void Update()
		{
			Vector2 playerCenter = Vector2.Zero;
			ListCleaner.CleanList(projectiles, delegate (Projectile projectile)
			{
				projectile.Update();
				return projectile.timeLeft <= 0;
			});
			foreach (Fighter fighter in fighters)
			{
				fighter.Update();

				playerCenter += fighter.Center + fighter.velocity * 10;
			}
			foreach (Fighter fighter in fighters)
			{
				fighter.Update_PostHit(fighter.Update_Hits());
			}
			foreach (Fighter fighter in fighters)
			{
				fighter.Update_Animation();
				if (Game1.showHitboxes)
					fighter.AddCapsules();
			}
			playerCenter /= fighters.Length;
			Vector2 cameraTarget = Vector2.Lerp(playerCenter, new Vector2(stage.collider.Rectangle.Center.X, stage.collider.Rectangle.Top), 0.5f);
			GraphicsHandler.main.children["game"].position = -(Vector2.Lerp(cameraTarget, -GraphicsHandler.main.children["game"].position + Program.WindowBounds().Size.ToVector2() / 2, 0.9f) - Program.WindowBounds().Size.ToVector2() / 2);
		}
	}
}
