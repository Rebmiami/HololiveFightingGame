using HololiveFightingGame.Gameplay.Combat;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static HololiveFightingGame.Loading.AnimationSetData;

namespace HololiveFightingGame.Loading
{
	public class FighterData
	{
		// Picture
		Texture2D texture;
		List<AnimationData> animations;

		// Moves
		Dictionary<string, Move> moves;


		/// <summary>
		/// Loads a fighter from a folder from the specified file address.
		/// </summary>
		/// <param name="address"></param>
		public void Read(string address)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes the contents of the object to the specified file address.
		/// </summary>
		/// <param name="address"></param>
		public void Write(string address)
		{
			throw new NotImplementedException();
		}
	}
}
