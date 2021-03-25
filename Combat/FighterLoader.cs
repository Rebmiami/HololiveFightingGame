using HololiveFightingGame.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HololiveFightingGame.Combat
{
	public static class FighterLoader
	{
		public static List<string> allFighters;

		public static Dictionary<string, Dictionary<string, Move>> moves;

		public static void LoadFighters()
		{
			// Load a list of fighter names.
			Game1.jsonLoaderFilePath = @"\Data\FighterList.json";
			string json = File.ReadAllText(Game1.gamePath + Game1.jsonLoaderFilePath);
			allFighters = JsonSerializer.Deserialize<List<string>>(json, GameLoader.SerializerOptions);
		}

		public static void LoadMoves(Fighter[] fighters)
		{
			string[] characters = new string[fighters.Length];
			for (int i = 0; i < fighters.Length; i++)
			{
				characters[i] = fighters[i].character;
			}
			LoadMoves(characters);
		}

		public static void LoadMoves(string[] fighters)
		{
			moves = new Dictionary<string, Dictionary<string, Move>>();
			for (int i = 0; i < fighters.Length; i++)
			{
				string fighter = fighters[i];

				// Avoid loading the same fighter twice.
				if (moves.ContainsKey(fighter))
				{
					continue;
				}

				moves.Add(fighter, new Dictionary<string, Move>());
				string movePath = @"\Data\Fighters\" + fighter + @"\Moves";

				// Finds all moves in the fighter's data folder and loads them.
				string[] movesToLoad = Directory.GetFiles(Game1.gamePath + movePath);
				foreach (string moveName in movesToLoad)
				{
					// Clip off the directory and file type
					string clippedName = moveName.Substring((Game1.gamePath + movePath + @"\").Length);
					clippedName = clippedName.Split('.')[0];
					moves[fighter].Add(clippedName, new Move(clippedName, fighter));
				}
			}
		}
	}
}