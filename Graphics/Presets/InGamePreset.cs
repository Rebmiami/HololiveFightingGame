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
				//noZoom = true,
				children = new Dictionary<string, DrawObject>() }
			} };

			// TODO: Load presets from JSON instead of classes
			// or offload construction of the graphics tree to all the classes using it
			// or implement presets into the GraphicsHandler class somehow.
		}
	}
}
