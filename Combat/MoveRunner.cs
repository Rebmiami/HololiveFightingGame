using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	/// <summary>
	/// Used by the fighter to store information about the currently running move.
	/// </summary>
	public class MoveRunner
	{
		public bool[] hitboxEnabled;
		public Vector2[] hitboxPositions;
		public Vector2[] hitboxVelocities;

		public Move move;
		public MoveData data;
		public string name;

		public MoveRunner(Move move)
        {
			this.move = move;
			data = move.moveData;
			name = data.Name;
			hitboxEnabled = new bool[move.hitboxes.Length];
			hitboxPositions = new Vector2[move.hitboxes.Length];
			hitboxVelocities = new Vector2[move.hitboxes.Length];
            for (int i = 0; i < move.hitboxes.Length; i++)
            {
				hitboxEnabled[i] = move.hitboxes[i].enabled;
				hitboxPositions[i] = Vector2.Zero;
				hitboxVelocities[i] = Vector2.Zero;
			}
		}

		public void Update(int time)
        {
			for (int i = 0; i < data.Hitboxes.Length; i++)
			{
				// Keys not being found is expected and can safely be ignored.
				try
				{
					hitboxEnabled[i] = data.Hitboxes[i].Activation[i.ToString()];
				}
				catch (KeyNotFoundException) { }

				try
				{
					hitboxVelocities[i] = data.Hitboxes[i].Motion[i.ToString()];
				}
				catch (KeyNotFoundException) { }

				hitboxPositions[i] += hitboxVelocities[i];
			}
        }
	}
}
