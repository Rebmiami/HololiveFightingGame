using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat
{
	public static class MoveLoader
	{
		public static List<Move>[] moves;

		public static void LoadMoves(Fighter[] fighters)
		{
			FighterCharacter[] characters = new FighterCharacter[fighters.Length];
			for (int i = 0; i < fighters.Length; i++)
			{
				characters[i] = fighters[i].character;
			}
			LoadMoves(characters);
		}

		public static void LoadMoves(FighterCharacter[] fighters)
		{
			moves = new List<Move>[fighters.Length];
			// TODO: Load a list of moves from JSON and add it to this list
		}
	}
}
