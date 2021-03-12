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

		public Language(Dictionary<string, string> pairs)
		{
			Lang = pairs;
			pairs.Add("Test_EN", "This is some test text. It's beautiful and text-y.");
			pairs.Add("Test_JP", "日本語の話し方がわかりません。このテキストはGoogle翻訳からのものです。");
			// I don't know how to speak Japanese and this text is from Google Translate
			// TODO: Automatically import lang data from JSON file
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

	//[ContentImporter]
	//public class LangImporter : ContentImporter<Language>

	public class LangReader : ContentTypeReader<Language>
	{
		protected override Language Read(ContentReader input, Language existingInstance)
		{
			string json = input.ReadString();
			var lang = (Dictionary<string, string>)JsonSerializer.Deserialize(json, typeof(Dictionary<string, string>));
			return new Language(lang);
		}
	}
}
