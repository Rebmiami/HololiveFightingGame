using HololiveFightingGame.Gameplay.Combat;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Loading.Serializable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static HololiveFightingGame.Loading.Serializable.AnimationSetData;

namespace HololiveFightingGame.Loading
{
    public class FighterData
	{
		// Fighter
		public string name;

		// Textures
		public Texture2D texture;

		// Moves
		public Dictionary<string, MoveData> moves;

		// Animations
		public List<AnimationData> animationData;
		public Dictionary<string, Animation> animations;

		// Stats and hurtboxes
		public FighterStats stats;

		public FighterData(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Loads a fighter from a folder from the specified file address.
		/// </summary>
		/// <param name="address">The address to load the fighter from.</param>
		public void Load(string address)
		{
			// --- LOAD TEXTURE ---
			texture = ImageLoader.LoadTexture(address + @"\Fighter.png", true);
			ColorTracker.StripColors(ref texture, new Color[] { Color.Lime, Color.Red, Color.Yellow, Color.Blue });


			// --- LOAD MOVES ---
			string movePath = address + @"\Moves";

			// Finds all moves in the fighter's data folder and loads them.
			string[] movesToLoad = Directory.GetFiles(movePath);
			moves = new Dictionary<string, MoveData>();
			foreach (string moveName in movesToLoad)
			{
				string json1 = File.ReadAllText(Game1.jsonLoaderFilePath = moveName);
				MoveData Data = JsonSerializer.Deserialize<MoveData>(json1, GameLoader.SerializerOptions);

				// Clip off the directory and file type
				string clippedName = moveName.Substring((movePath + @"\").Length);
				clippedName = clippedName.Split('.')[0];
				moves.Add(clippedName, Data);
			}


			// --- LOAD ANIMATION FRAMES ---
			Game1.jsonLoaderFilePath = address + @"\AnimationFrames.json";
			animationData = GetAnimations(File.ReadAllText(Game1.jsonLoaderFilePath));


			// --- LOAD ANIMATIONS ---
			// Loads basic animations, such as idle, walking, jumping, etc.
			Game1.jsonLoaderFilePath = address + @"\Animations.json";
			string json = File.ReadAllText(Game1.jsonLoaderFilePath);
			animations = JsonSerializer.Deserialize<Dictionary<string, Animation>>(json);

			// Loads animations for moves.
			foreach (MoveData data in moves.Values)
			{
				animations.Add(data.Name, data.Animation);
			}

			// --- LOAD STATS AND HURTBOXES ---
			Game1.jsonLoaderFilePath = address + @"\Stats.json";
			json = File.ReadAllText(Game1.jsonLoaderFilePath);
			stats = JsonSerializer.Deserialize<FighterStats>(json);
		}

		/// <summary>
		/// Writes the contents of the object to the specified file address.
		/// </summary>
		/// <param name="address"></param>
		public void Write(string address)
		{
			JsonSerializerOptions options = new JsonSerializerOptions();
			options.WriteIndented = true;
			// TODO: Make this an option, somehow

			// TODO: Find a way to get textures to work here.
			// You could load the texture without stripping any colors and cache that somewhere


			// --- SAVE MOVES ---
			foreach (KeyValuePair<string, MoveData> pair in moves)
			{
				string name = pair.Key;
				string moveJson = JsonSerializer.Serialize(pair.Value, options);
				File.WriteAllText(address + @"\Moves\" + name + ".json", moveJson);
			}


			// --- SAVE ANIMATION FRAMES --- 
			string animFramesJson = ToJSON(animationData, options);
			File.WriteAllText(address + @"\AnimationFrames.json", animFramesJson);


			// --- SAVE ANIMATIONS ---
			Dictionary<string, Animation> nonMoveAnims = new Dictionary<string, Animation>(animations);
			foreach (MoveData data in moves.Values)
			{
				nonMoveAnims.Remove(data.Name);
			}
			string animsJson = JsonSerializer.Serialize(nonMoveAnims, options);
			File.WriteAllText(address + @"\Animations.json", animsJson);


			// --- SAVE STATS AND HURTBOXES --- 
			string statsJson = JsonSerializer.Serialize(stats, options);
			File.WriteAllText(address + @"\Stats.json", statsJson);
		}
	}
}
