using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Graphics.Presets
{
	public class InGamePreset : DrawObject
	{
		public InGamePreset(DrawObjectType type = DrawObjectType.Main) : base(type)
		{
			children = new Dictionary<string, DrawObject>() {
			{ "game", new DrawObject(DrawObjectType.Layer) {
				children = new Dictionary<string, DrawObject>() {
					//{ "fighter0", new DrawObject(DrawObjectType.Sprite) },
					//{ "fighter1", new DrawObject(DrawObjectType.Sprite) },
					{ "stage", new DrawObject(DrawObjectType.Sprite) }
				} }
			},

			{ "ui", new DrawObject(DrawObjectType.Layer) {
				noZoom = true,
				children = new Dictionary<string, DrawObject>() {
					{ "test", new DrawObject(DrawObjectType.Text) {
						data = new TextData("This is some test text. It's beautiful and text-y", Color.White) }
					//This is some test text. It's beautiful and text-y.
					//日本語の話し方がわかりません。このテキストはGoogle翻訳からのものです。
					//I don't know how to speak Japanese and this text is from Google Translate
					}
				} }
			} };

			// TODO: Load presets from JSON instead of classes like these

			// UI? - "DAMAGE ダメージ"
		}
	}
}
