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
using HololiveFightingGame.Combat;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;

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
		public static string gamePath;

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

		public static bool jsonFailsafe = false;
		public static string jsonErrorMessage;
		public static string jsonLoaderFilePath;
		// This is used for diagnostic purposes

		protected override void LoadContent()
		{
			font = Content.Load<SpriteFont>("File");
			try
			{
				gamePath = new Regex(@"HololiveFightingGame\.exe$").Replace(Process.GetCurrentProcess().MainModule.FileName, "");
				spriteBatch = new SpriteBatch(GraphicsDevice);

				testFighter = Content.Load<Texture2D>("TestFighter");
				testStage = Content.Load<Texture2D>("TestStage");
				language = new Language();
				GraphicsHandler.main = new InGamePreset();
				gameState = new GameState();
				inGameUI = Content.Load<Texture2D>("GameUI");
				uiHandler = new UIHandler();
				MoveLoader.LoadMoves(gameState.fighters);
			}
			catch (JsonException exception)
			{
				jsonErrorMessage = exception.Message;
				jsonFailsafe = true;
			}
		}

		protected override void Update(GameTime gameTime)
		{
			
			if (KeyHelper.Released(Keys.LeftControl))
			{
				displayLanguage = (DisplayLanguage)(((int)displayLanguage + 1) % 2);
			}
			if (jsonFailsafe)
			{
				if (KeyHelper.Pressed(Keys.Enter))
					Process.Start("explorer.exe", gamePath);

				if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				{
					Exit();
					if (KeyHelper.Down(Keys.LeftShift) || KeyHelper.Down(Keys.RightShift))
						Process.Start(Process.GetCurrentProcess().MainModule.FileName);
				}
			}
			else
			{
				gameState.Update();
			}
			GamePadHelper.Update();
			MouseHelper.Update();
			KeyHelper.Update();
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Gray * 0.5f);
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
			if (jsonFailsafe)
			{
				string errorMessage = displayLanguage == 0 ?
					"An error was encountered while loading data from JSON.\n " +
					"The offending file is located at " + jsonLoaderFilePath.Replace("\\", "/") + " within the game directory.\n " +
					"If you have installed a modification that changes JSON or tried modifying the JSON yourself, this may be the cause. If the case is the former, check for mod conflicts or contact the mod authors.\n " +
					"If possible, try to revert any changes you have made. If you cannot, visit https://github.com/Rebmiami/HololiveFightingGame, find the correct version of the offending file, and replace the offending file with it.\n " +
					"The exact error message is as follows. This may help you diagnose and solve the issue:\n " +
					jsonErrorMessage + "\n " +
					"Press ESC to exit the program. Press ENTER to open File Explorer to the game directory. Press SHIFT+ESC to close and re-launch the game if you have fixed the issue." :

					"Japanese error text has not been added yet";

				string[] words = errorMessage.Split(' ');
				StringBuilder sb = new StringBuilder();
				float lineWidth = 0f;
				float spaceWidth = font.MeasureString(" ").X;
				int maxLineWidth = Program.WindowBounds().Width - 20;

				foreach (string word in words)
				{
					Vector2 size = font.MeasureString(word);

					if (word.Contains("\n"))
					{
						lineWidth = 0f;
						sb.Append(word + "\n");
						continue;
					}

					if (lineWidth + size.X < maxLineWidth)
					{
						sb.Append(word + " ");
						lineWidth += size.X + spaceWidth;
					}
					else
					{
						sb.Append("\n" + word + " ");
						lineWidth = size.X + spaceWidth;
					}
				}
				errorMessage = sb.ToString();

				spriteBatch.DrawString(font, errorMessage, new Vector2(10), Color.White);
			}
			else
			{
				GraphicsHandler.main.Draw(spriteBatch, new Transformation(Vector2.Zero, 2));
			}
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
