using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	public static class GraphicsHandler
	{
		static public DrawObject main;
	}

	public class DrawObject
	{
		public Dictionary<string, DrawObject> children;
		public DrawObjectType type;
		public SlicedSprite texture;
		public string frame = "sprite";
		public Vector2 position;

		public DrawObject (DrawObjectType type)
		{
			children = new Dictionary<string, DrawObject>();
			this.type = type;
		}

		public void Draw(SpriteBatch spriteBatch, Transformation transformation)
		{
			if (type == DrawObjectType.Main || type == DrawObjectType.Layer || type == DrawObjectType.ComponentSprite || type == DrawObjectType.Particle)
				foreach (KeyValuePair<string, DrawObject> toDraw in children)
				{
					toDraw.Value.Draw(spriteBatch, transformation);
				}
			else
			{
				spriteBatch.Draw(texture.texture, position, texture.slices[frame], Color.White);
			}
		}
	}

	public class SlicedSprite
	{
		public Texture2D texture;
		public Dictionary<string, Rectangle> slices;

		public SlicedSprite(Texture2D texture)
		{
			this.texture = texture;
			slices = new Dictionary<string, Rectangle>() { { "sprite", texture.Bounds } };
		}
	}

	public class InGamePreset : DrawObject
	{
		public InGamePreset(DrawObjectType type = DrawObjectType.Main) : base (type)
		{
			children = new Dictionary<string, DrawObject>() {
			{ "game", new DrawObject(DrawObjectType.Layer) {
				children = new Dictionary<string, DrawObject>() {
				{ "fighter", new DrawObject(DrawObjectType.Sprite) },
				{ "stage", new DrawObject(DrawObjectType.Sprite) }
			} } } };
		}
	}

	public enum DrawObjectType //Dictates how a draw object should behave and treat its children
	{
		Main, // Always the size of the window. Will be scaled accordingly. Origin is always top of window. Exclusively contains layers.
		Layer, // Contains sprites, component sprites, flashes, and particle systems. Origin is always top of window.
		ComponentSprite, // Draws a single object consisting of multiple sprites. Exclusively contains sprites or component sprites
		Sprite, // Contains a texture to draw. Cannot have children.
		Flash, // Refers to graphical effects like puffs of smoke or sparks. Cannot have children.
		Particle, // Refers to a particle system. Exclusively contains flashes.
	}

	public class Transformation
	{

	}
}
