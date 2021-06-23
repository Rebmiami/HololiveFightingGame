using HololiveFightingGame.Gameplay.Combat;
using HololiveFightingGame.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HololiveFightingGame.Loading
{
	public static class FighterLoader
	{
		// A list of loaded fighters.
		public static List<string> allFighters;

		// Loading middleman. Contains textures and information directly deserialized from JSON.
		public static Dictionary<string, FighterData> fighterData;

		// A list of moves that each fighter can perform.
		public static Dictionary<string, Dictionary<string, Move>> moves;
		// The first index is the fighter, and the second index is the move.

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
				foreach (string moveName in fighterData[fighter].moves.Keys)
				{
					moves[fighter].Add(moveName, new Move(fighterData[fighter].moves[moveName]));
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
				// TODO: Use new animation frames system instead of the system with the fixed frame sizes
				fighter.drawObject.texture = new AnimatedSprite(fighterData[fighter.character].texture, new Point(50, 80));
				((AnimatedSprite)fighter.drawObject.texture).animations = fighterData[fighter.character].animations;
				((AnimatedSprite)fighter.drawObject.texture).SetAnimFrames();
			}
		}

		public static void LoadFighterData(string[] fighters)
		{
			fighterData = new Dictionary<string, FighterData>();
			for (int i = 0; i < fighters.Length; i++)
			{
				string fighter = fighters[i];
				FighterData data = new FighterData(fighter);
				fighterData.Add(fighter, data);
				data.Load(@".\Content\Data\Fighters\" + fighter);
			}
		}
	}
}