﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Graphics
{
	public class DrawObject
	{
		public Dictionary<string, DrawObject> children;
		public DrawObjectType type;
		public SlicedSprite texture;
		public string frame = "sprite";
		public Vector2 position;
		public SpriteEffects spriteEffects;
		public Vector2 dimensions;
		public DrawObjectData data;
		public bool noZoom = false;
		// Used for text of text objects and some information regarding flashes

		public DrawObject(DrawObjectType type)
		{
			children = new Dictionary<string, DrawObject>();
			this.type = type;
		}

		public void Draw(SpriteBatch spriteBatch, Transformation transformation)
		{
			if (type == DrawObjectType.Main || type == DrawObjectType.Layer || type == DrawObjectType.ComponentSprite || type == DrawObjectType.Particle)
				foreach (KeyValuePair<string, DrawObject> toDraw in children)
				{
					Transformation pass = new Transformation(transformation.offset, transformation.zoom);
					pass.offset += position;
					if (noZoom)
					{
						pass.zoom = 1;
					}
					toDraw.Value.Draw(spriteBatch, pass);
				}
			else
			{
				Vector2 drawPosition = position + transformation.offset;
				drawPosition -= Program.WindowBounds().Size.ToVector2() / 2;
				drawPosition *= transformation.zoom;
				drawPosition += Program.WindowBounds().Size.ToVector2() / 2;
				if (type == DrawObjectType.Text)
				{
					//var a = Game1.font.Characters;

					spriteBatch.DrawString(Game1.font, ((TextData)data).text, drawPosition, ((TextData)data).color);
				}
				else
				{
					spriteBatch.Draw(texture.texture, drawPosition, texture.slices[frame], Color.White, 0, Vector2.Zero, transformation.zoom, spriteEffects, 0);
				}
			}
		}

		public Vector2 Bottom
		{
			get { return position + new Vector2(texture.slices[frame].Width / 2, texture.slices[frame].Height); }
			set { position = value - new Vector2(texture.slices[frame].Width / 2, texture.slices[frame].Height); }
		}

		public Vector2 Center
		{
			get { return position + new Vector2(texture.slices[frame].Width / 2, texture.slices[frame].Height / 2); }
			set { position = value - new Vector2(texture.slices[frame].Width / 2, texture.slices[frame].Height / 2); }
		}
	}

	public enum DrawObjectType //Dictates how a draw object should behave and treat its children
	{
		Main, // Always the size of the window. Will be scaled accordingly. Origin is always top of window. Exclusively contains layers.
		Layer, // Contains sprites, component sprites, flashes, particle systems, and text. Origin can be moved.
		ComponentSprite, // Draws a single object consisting of multiple sprites. Exclusively contains sprites or component sprites
		Sprite, // Contains a texture to draw. Cannot have children.
		Flash, // Refers to graphical effects like puffs of smoke or sparks. Cannot have children.
		Particle, // Refers to a particle system. Exclusively contains flashes.
		Text, // Prints text. Cannot have children. Currently supports no languages. Should support English and Japanese.
		Custom, // Runs code for drawing to avoid overhead when necessary
	}

	public class Transformation
	{
		public Vector2 offset;
		public float zoom;

		public Transformation(Vector2 offset, float zoom)
		{
			this.offset = offset;
			this.zoom = zoom;
		}
	}
}
