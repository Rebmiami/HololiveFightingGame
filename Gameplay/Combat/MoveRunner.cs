using HololiveFightingGame.Loading.Serializable;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Gameplay.Combat
{
    /// <summary>
    /// Used by the fighter to store information about the currently running move.
    /// </summary>
    public class MoveRunner
	{
		public bool[] enabled;
		public Vector2[] pos;
		public Vector2[] vel;
		public Vector2 motion;
		public int frame;

		public Move move;
		public MoveData data;
		public string name;

		public MoveRunner(Move move)
		{
			this.move = move;
			data = move.Data;
			name = data.Name;
			enabled = new bool[move.hitboxes.Length];
			pos = new Vector2[move.hitboxes.Length];
			vel = new Vector2[move.hitboxes.Length];
			for (int i = 0; i < move.hitboxes.Length; i++)
			{
				enabled[i] = move.Data.Hitboxes[i].Enabled;
				pos[i] = Vector2.Zero;
				vel[i] = Vector2.Zero;
			}
			motion = Vector2.Zero;
			// Update(data.MoveDuration);
		}

		public void Update(int time)
		{
			time = data.MoveDuration - time;
			for (int i = 0; i < data.Hitboxes.Length; i++)
			{
				if (data.Hitboxes[i].Activation.ContainsKey(time.ToString()))
					enabled[i] = data.Hitboxes[i].Activation[time.ToString()];

				if (data.Hitboxes[i].Motion.ContainsKey(time.ToString()))
					pos[i] = data.Hitboxes[i].Motion[time.ToString()];

				pos[i] += vel[i];
			}
			if (data.AnimationFrames.ContainsKey(time.ToString()))
				frame = data.AnimationFrames[time.ToString()];

			if (data.Motion != null && data.Motion.ContainsKey(time.ToString()))
				motion = data.Motion[time.ToString()];
		}
	}
}
