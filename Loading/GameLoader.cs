using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
using HololiveFightingGame.Input;

namespace HololiveFightingGame.Loading
{
	public class GameLoader
	{
		private string status = "...";
		public string Status {
			get
			{
				return status;
			}
			set
			{
				status = value;
				Debug.WriteLine(value);
			}
		}
		public bool firstRun = false;

		public bool done;

		public void Load()
		{
			try
			{
				// Status = "Doing nothing for a few seconds";
				// System.Threading.Thread.Sleep(3000);
				// This is meant to see if the loading screen works

				Status = "Checking for missing files and directories";
				if (!AllConfigFilesPresent())
				{
					Status = "Creating missing files and directories";
					firstRun = true;
					FirstRunSetup();
				}

				string json = File.ReadAllText(@".\Config\ControlProfiles\Profile0.json");
				ControlProfileLoader loader = JsonSerializer.Deserialize<ControlProfileLoader>(json);
				ControlProfile profile = new ControlProfile(loader);
				ProfileBinder.profile = profile;
			}
			catch (JsonException exception)
			{
				Game1.jsonErrorMessage = exception.Message;
				Game1.jsonDeathScreen = true;
				Game1.isDeathScreen = true;
				Game1.gameScreen = GameScreen.DeathScreen;
			}
			done = true;
		}

		public bool AllConfigFilesPresent()
		{
			return GetBadFile() == "All is good, chief.";
		}

		public string GetBadFile()
		{
			if (!Directory.Exists(@".\Config"))
			{
				return "Config";
			}
			if (!Directory.Exists(@".\Config\ControlProfiles"))
			{
				return "ControlProfiles";
			}
			if (!File.Exists(@".\Config\UserPrefs.json"))
			{
				return "UserPrefs.json";
			}
			else
			{
				try
				{
					string json = File.ReadAllText(@".\Config\UserPrefs.json");
					Dictionary<string, string> loader = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
					foreach (string element in loader.Values)
					{
						if (element == default)
						{
							return "UserPrefs.json";
						}
					}
					UserPrefLoader test = JsonSerializer.Deserialize<UserPrefLoader>(json);
				}
				catch
				{
					return "UserPrefs.json";
					// If there was an exception thrown trying to read the file, then it's most likely faulty.
				}
			}
			if (!File.Exists(@".\Config\ControlProfiles\Profile0.json"))
			{
				return "Profile0.json";
			}
			else
			{
				try
				{
					string json = File.ReadAllText(@".\Config\ControlProfiles\Profile0.json");
					Dictionary<string, string> loader = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
					foreach (string element in loader.Values)
					{
						if (element == default || element == null)
						{
							return "Profile0.json";
						}
					}
					ControlProfileLoader test = JsonSerializer.Deserialize<ControlProfileLoader>(json);
					ControlProfile test1 = new ControlProfile(test);
				}
				catch
				{
					return "Profile0.json";
					// If there was an exception thrown trying to read the file, then it's most likely faulty.
				}
			}
			return "All is good, chief.";
		}

		/// <summary>
		/// Performs necessary setup upon the first time the application is run on a client machine.
		/// </summary>
		public void FirstRunSetup()
		{
			while (GetBadFile() != "All is good, chief.")
			{
				if (GetBadFile() == "Config")
				{
					Status = "Creating config folder";
					Directory.CreateDirectory(@".\Config");
				}
				if (GetBadFile() == "ControlProfiles")
				{
					Status = "Creating controller profile folder";
					Directory.CreateDirectory(@".\Config\ControlProfiles");
				}
				if (GetBadFile() == "UserPrefs.json")
				{
					// Status = "English (1) / 日本語 (2)?";
					// TODO: Add language selection prompt. English is default for now.
					Status = "Creating user preferences config file";
					UserPrefLoader prefs = new UserPrefLoader
					{
						Language = "EN"
					};
					string json = JsonSerializer.Serialize(prefs, typeof(UserPrefLoader), SerializerOptions);
					using StreamWriter sw = new StreamWriter(@".\Config\UserPrefs.json");
					foreach (char ch in json)
					{
						sw.Write(ch);
					}
				}
				if (GetBadFile() == "Profile0.json")
				{
					Status = "Creating default control profile file";
					ControlProfileLoader profile = new ControlProfileLoader
					{
						Attack = "P",
						AttackB = "O",
						ControlLeft = "A",
						ControlRight = "D",
						ControlUp = "W",
						ControlDown = "S",
						Jump = "W"
					};
					string json = JsonSerializer.Serialize(profile, typeof(ControlProfileLoader), SerializerOptions);
					using StreamWriter sw = new StreamWriter(@".\Config\ControlProfiles\Profile0.json");
					foreach (char ch in json)
					{
						sw.Write(ch);
					}
				}
			}
		}

		public static JsonSerializerOptions SerializerOptions
		{
			get
			{
				return new JsonSerializerOptions
				{
					WriteIndented = true,
					ReadCommentHandling = JsonCommentHandling.Skip
				};
			}
		}
	}
}
