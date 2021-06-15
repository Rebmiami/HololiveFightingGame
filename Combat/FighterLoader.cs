using HololiveFightingGame.Graphics;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		// The first index is the fighter, and the second index is the move.

		public static void LoadFighters()
		{
			// Load a list of fighter names.
			Game1.jsonLoaderFilePath = @".\Content\Data\FighterList.json";
			string json = File.ReadAllText(Game1.jsonLoaderFilePath);
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
				string movePath = @".\Content\Data\Fighters\" + fighter + @"\Moves";

				// Finds all moves in the fighter's data folder and loads them.
				string[] movesToLoad = Directory.GetFiles(movePath);
				foreach (string moveName in movesToLoad)
				{
					// Clip off the directory and file type
					string clippedName = moveName.Substring((movePath + @"\").Length);
					clippedName = clippedName.Split('.')[0];
					moves[fighter].Add(clippedName, new Move(clippedName, fighter));
				}
			}
		}

		public static void ReloadMoves()
        {
			string[] fighters = new string[moves.Count];
			moves.Keys.CopyTo(fighters, 0);

			moves.Clear();
			LoadMoves(fighters);
        }

		public static void LoadAnimations(Fighter[] fighters)
		{
			// Sets up a fighter's sprite and animations.
			foreach (Fighter fighter in fighters)
			{
				Texture2D fighterSprite = ImageLoader.LoadTexture(@".\Content\Data\Fighters\" + fighter.character + @"\Fighter.png", true);
				fighter.drawObject.texture = new AnimatedSprite(fighterSprite, new Point(50, 80));

				// Loads basic animations, such as idle, walking, jumping, etc.
				Game1.jsonLoaderFilePath = @".\Content\Data\Fighters\" + fighter.character + @"\Animations.json";
				string json = File.ReadAllText(Game1.jsonLoaderFilePath);
				((AnimatedSprite)fighter.drawObject.texture).animations = JsonSerializer.Deserialize<Dictionary<string, Animation>>(json);
				
				// Loads animations for moves.
				foreach (Move move in moves[fighter.character].Values)
				{
					((AnimatedSprite)fighter.drawObject.texture).animations.Add(move.Data.Name, move.Data.Animation);
				}

				// Finalizes animation setup.
				((AnimatedSprite)fighter.drawObject.texture).SetAnimFrames();
			}
		}
	}
}