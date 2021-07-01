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

		public float zoom;

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

			float largestPlayerDistance = 0;
			for (int i = 0; i < fighters.Length - 1; i++)
			{
				for (int j = i + 1; j < fighters.Length; j++)
				{
					Fighter fighter1 = fighters[i];
					Fighter fighter2 = fighters[j];
					float distance = Vector2.Distance(fighter1.Bottom * new Vector2(1, 2.5f), fighter2.Bottom * new Vector2(1, 2.5f));
					if (distance > largestPlayerDistance)
					{
						largestPlayerDistance = distance;
					}
				}
			}
			float targetZoom = Math.Min(1 / largestPlayerDistance * 500, 2f);
			zoom = (float)(zoom * 0.9 + targetZoom * 0.1);

			Vector2 cameraTarget = Vector2.Lerp(playerCenter, new Vector2(stage.collider.Rectangle.Center.X, stage.collider.Rectangle.Top), 0.3f);
			GraphicsHandler.main.children["game"].position = -(Vector2.Lerp(cameraTarget, -GraphicsHandler.main.children["game"].position + Program.WindowBounds().Size.ToVector2() / 2, 0.9f) - Program.WindowBounds().Size.ToVector2() / 2);
		}
	}
}
