using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using HololiveFightingGame.Localization;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Graphics.Presets;
using HololiveFightingGame.Input;

namespace HololiveFightingGame
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		public static GameState gameState;
		public static UIHandler uiHandler;

		protected override void Initialize()
		{
			displayLanguage = DisplayLanguage.EN;
			//graphics.ToggleFullScreen();

			base.Initialize();
		}

		public static Texture2D testFighter;
		public static Texture2D testStage;
		public static Texture2D inGameUI;
		public static SpriteFont font;
		public static Language language;
		public static DisplayLanguage displayLanguage;

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			testFighter = Content.Load<Texture2D>("TestFighter");
			testStage = Content.Load<Texture2D>("TestStage");
			font = Content.Load<SpriteFont>("File");
			//language = Content.Load<Language>("language");
			language = new Language(new Dictionary<string, string>());
			GraphicsHandler.main = new InGamePreset();
			gameState = new GameState();
			inGameUI = Content.Load<Texture2D>("GameUI");
			uiHandler = new UIHandler();
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (KeyHelper.Released(Keys.LeftControl))
			{
				displayLanguage = (DisplayLanguage)(((int)displayLanguage + 1) % 2);
			}
			gameState.Update();

			GamePadHelper.Update();
			MouseHelper.Update();
			KeyHelper.Update();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Gray * 0.5f);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
			GraphicsHandler.main.Draw(spriteBatch, new Transformation(Vector2.Zero, 2));
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}

	public enum DisplayLanguage
	{
		EN,
		JP
	}
}
