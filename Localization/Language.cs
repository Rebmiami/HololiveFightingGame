using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Web;

namespace HololiveFightingGame.Localization
{
	public class Language
	{
		private Dictionary<string, string> Lang { get; set; }

		public Language()
		{
			Game1.jsonLoaderFilePath = @".\Data\Language.json";
			string json = File.ReadAllText(Game1.jsonLoaderFilePath);
			Lang = (Dictionary<string, string>)JsonSerializer.Deserialize(json, typeof(Dictionary<string, string>), GameLoader.SerializerOptions);
		}

		public string GetLocalizedString(string name, DisplayLanguage? language = null)
		{
			if (language == null)
			{
				language = Game1.displayLanguage;
			}

			return Lang[name + "_" + Enum.GetNames(typeof(DisplayLanguage))[(int)language]];
		}
	}
}
