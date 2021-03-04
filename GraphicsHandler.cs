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
		public SpriteEffects spriteEffects;
		public Vector2 dimensions;
		public DrawObjectData data;
		// Used for text of text objects and some information regarding flashes

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
					Transformation pass = new Transformation(transformation.offset, transformation.zoom);
					pass.offset += position;
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

					spriteBatch.DrawString(Game1.font, ((TextData)data).text, Vector2.Zero, ((TextData)data).color);
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
					//{ "fighter0", new DrawObject(DrawObjectType.Sprite) },
					//{ "fighter1", new DrawObject(DrawObjectType.Sprite) },
					{ "stage", new DrawObject(DrawObjectType.Sprite) }
				} }
			},

			{ "ui", new DrawObject(DrawObjectType.Layer) {
				children = new Dictionary<string, DrawObject>() {
					{ "test", new DrawObject(DrawObjectType.Text) {
						data = new TextData("This is some test text. It's beautiful and text-y", Color.White) }
					//This is some test text. It's beautiful and text-y.
					//日本語の話し方がわかりません。このテキストはGoogle翻訳からのものです。
					//I don't know how to speak Japanese and this text is from Google Translate
					}
				} }
			} };

			// UI? - "DAMAGE ダメージ"
		}
	}

	public abstract class DrawObjectData
	{

	}

	public class TextData : DrawObjectData
	{
		public string text;
		public Color color;

		public TextData(string text, Color color)
		{
			this.text = text;
			this.color = color;
		}
	}

	public class FlashData : DrawObjectData
	{

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

	public class AnimatedSprite : SlicedSprite
	{
		public static readonly int animFrameLength = 10; // "Frames per frame" rather than FPS - calculate 60 divided by this to get FPS

		public Dictionary<string, Animation> animations;
		public string currentAnim = "neutral";

		public Animation Playing
		{
			get
			{
				return animations[currentAnim];
			}
		}

		public Point dimensions;

		public AnimatedSprite(Texture2D texture, Point dimensions) : base(texture)
		{
			this.dimensions = dimensions;
		}

		public class Animation
		{
			public string nextAnim; // If null, will halt on the last frame of the animation until a new animation is started.

			public int animID;
			public int frames; // Length of animation in frames.

			public bool autoAnimate;
			// If true, the animation will automatically progress through frames at a set framerate.
			// If false, the frame must be set by any other arbitrary class.
			
			public int progress;

			public int Frame
			{
				get
				{
					return progress / (autoAnimate ? animFrameLength : 1);
				}
				set
				{
					progress = value * (autoAnimate ? animFrameLength : 1);
				}
			}

			public Animation(int id, int length, bool auto, string next = null)
			{
				animID = id;
				frames = length;
				autoAnimate = auto;
				nextAnim = next;
			}
		}

		public void Update()
		{
			Animation animation = Playing;
			if (animation.autoAnimate)
			{
				animation.progress++;

				if (animation.Frame >= animation.frames)
				{
					if (animation.nextAnim == null)
						animation.progress--;
					else
						SetAnimation(animation.nextAnim, 0);
				}
			}
		}

		public void SetAnimFrames() // Called once after animations are set up to set all the frames. In the case of flashes, pre-set animations are used to save resources.
		{
			slices = new Dictionary<string, Rectangle>();
			foreach (string key in animations.Keys)
			{
				Animation animation = animations[key];
				for (int i = 0; i < animation.frames; i++)
				{
					slices.Add(key + i, new Rectangle(dimensions.X * animation.animID, dimensions.Y * i, dimensions.X, dimensions.Y));
				}
			}
		}

		public string GetFrame()
		{
			return currentAnim + Playing.Frame;
		}

		public void SetAnimation(string animation, int startFrame)
		{
			currentAnim = animation;
			Playing.progress = startFrame * (Playing.autoAnimate ? animFrameLength : 1);
		}

		public void SwitchAnimation(string animation, int startFrame)
		{
			if (currentAnim != animation)
			{
				currentAnim = animation;
				Playing.progress = startFrame * (Playing.autoAnimate ? animFrameLength : 1);
			}
		}
	}
}