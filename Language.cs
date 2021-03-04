using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Web;

namespace HololiveFightingGame
{
	public class Language
	{
		public Dictionary<string, string> lang { get; set; }

		public Language(Dictionary<string, string> pairs)
		{
			lang = pairs;
			pairs.Add("Test_EN", "This is some test text. It's beautiful and text-y.");
			pairs.Add("Test_JP", "日本語の話し方がわかりません。このテキストはGoogle翻訳からのものです。");

			// TODO: Automatically import lang data from JSON file
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
