using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	public class Projectile : Entity
	{
		public int owner;
		public int timeLeft;

		public MoveRunner moveRunner;

		public static Projectile Add()
		{
			// TODO: Use this method to add a projectile to the list in GameState
			// Projectile.Add(...); is cleaner than Game1.gameState.projectiles.Add(new Projectile(...));
			Projectile newProjectile = new Projectile();
			Game1.gameState.projectiles.Add(newProjectile);
			return newProjectile;
		}

		public override void Update()
		{
			timeLeft--;
			base.Update();
		}
	}
}
