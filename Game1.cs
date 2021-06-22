using HololiveFightingGame.Graphics;
using HololiveFightingGame.Graphics.Presets;
using HololiveFightingGame.Input;
using HololiveFightingGame.Loading;
using HololiveFightingGame.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System;
using HololiveFightingGame.FighterEditor;
using HololiveFightingGame.Graphics.CapsuleShader;
using HololiveFightingGame.Gameplay;

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
		
		static Game1()
		{
			gamePath = new Regex(@"\\HololiveFightingGame\.exe$").Replace(Process.GetCurrentProcess().MainModule.FileName, "");
		}

		public static GameState gameState;
		public static UIHandler uiHandler;
		public static GameScreen gameScreen = GameScreen.Loading;
		public static bool showHitboxes;

		public static readonly string gamePath;

		protected override void Initialize()
		{
			displayLanguage = DisplayLanguage.EN;
			// graphics.ToggleFullScreen();

			base.Initialize();
		}

		public static Language language;
		public static DisplayLanguage displayLanguage;
		public static Effect capsuleRenderShader;

		// The following fields are used for handling the "death screens" that pop up when the game encounters an error.
		public static bool isDeathScreen = false;

		public static bool jsonDeathScreen = false;
		public static string jsonErrorMessage;
		public static string jsonLoaderFilePath;

		public static bool loadingDeathScreen = false;
		public static bool gameplayDeathScreen = false;
		public static bool drawingDeathScreen = false;

		// Handles loading game content and assets, as well as first-run setup and repairing damaged files.
		public static GameLoader setup;

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Assets.font = Content.Load<SpriteFont>("File");
			// These must be loaded first

			setup = new GameLoader();

			Thread thread = new Thread(new ThreadStart(setup.Load));
			thread.Start();

			// TODO: Move following code to game loader.

			Assets.testStage = ImageLoader.LoadTexture(@".\Content\Assets\TestStage.png", true);
			language = new Language();
			Assets.inGameUI = ImageLoader.LoadTexture(@".\Content\Assets\GameUI.png", true);
			Assets.editorButton = ImageLoader.LoadTexture(@".\Content\Assets\EditorButtons.png", true);
			Assets.editorPlayIcon = ImageLoader.LoadTexture(@".\Content\Assets\EditorPlayIcon.png", true);
			capsuleRenderShader = Content.Load<Effect>("Capsule");
			capsuleRenderShader.Parameters["ViewportDimensions"].SetValue(Program.WindowBounds().Size.ToVector2());
			LoadGameState(new string[] { "Pekora", "Kiara" });
		}

		public static void LoadGameState(string[] fighters)
		{
			if (fighters.Length > GameState.maxFighters)
			{
				throw new ArgumentException("The maximum number of fighters is " + GameState.maxFighters + ".");
			}
			if (fighters.Length < 2)
			{
				throw new ArgumentException("The minimum number of fighters is 2.");
			}

			GraphicsHandler.main = new InGamePreset();
			gameState = new GameState(fighters);
			FighterLoader.LoadMoves(fighters);
			FighterLoader.LoadAnimations(gameState.fighters);
			uiHandler = new UIHandler();
		}

		protected override void Update(GameTime gameTime)
		{
			if (KeyHelper.Released(Keys.LeftControl))
			{
				displayLanguage = (DisplayLanguage)(((int)displayLanguage + 1) % 2);
			}
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
				if (KeyHelper.Down(Keys.LeftShift) || KeyHelper.Down(Keys.RightShift))
					Process.Start(Process.GetCurrentProcess().MainModule.FileName);
				// Is there a more direct way to re-launch the program?
			}
			if (KeyHelper.Pressed(Keys.F11))
			{
				showHitboxes = true;
			}

			switch (gameScreen)
			{
				case GameScreen.Loading:
					if (setup.done)
					{
						gameScreen = GameScreen.InGame;
					}
					break;
				case GameScreen.InGame:
					gameState.Update();
					if (KeyHelper.Pressed(Keys.F12))
					{ 
						gameScreen = GameScreen.Editor;
						Editor.Load();
					}
					break;
				case GameScreen.Editor:
					Editor.Update();
					break;
				case GameScreen.DeathScreen:
					if (KeyHelper.Pressed(Keys.Enter))
						Process.Start("explorer.exe", gamePath);
					break;
				default:
					break;
			}

			GamePadHelper.Update();
			MouseHelper.Update();
			KeyHelper.Update();
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			RenderTarget2D RenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height);
			GraphicsDevice.SetRenderTarget(RenderTarget);

			GraphicsDevice.Clear(Color.Gray * 0.5f);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

			switch (gameScreen)
			{
				case GameScreen.Loading:
					spriteBatch.DrawString(Assets.font, "Loading...", new Vector2(10), Color.White);
					if (setup.firstRun)
					{
						spriteBatch.DrawString(Assets.font, "Some user config files were missing or deformed.", new Vector2(10, 30), Color.White);
						spriteBatch.DrawString(Assets.font, "If this is your first time running this version of the game, this is expected.", new Vector2(10, 50), Color.White);
						spriteBatch.DrawString(Assets.font, "If not, check your settings after loading finishes. Some settings may have been changed.", new Vector2(10, 70), Color.White);
					}
					spriteBatch.DrawString(Assets.font, setup.Status, new Vector2(10, 90), Color.White);
					break;
				case GameScreen.InGame:
					GraphicsHandler.main.Draw(spriteBatch, new Transformation(Vector2.Zero, 2));
					break;
				case GameScreen.Editor:
					Editor.Draw(spriteBatch);
					break;
				case GameScreen.DeathScreen:
					DrawDeathScreen();
					break;
				default:
					break;
			}

			base.Draw(gameTime);
			spriteBatch.End();


			if (gameScreen == GameScreen.Editor)
				CapsuleRenderer.Draw(spriteBatch, new Transformation(Vector2.Zero, 1), GraphicsDevice);
			else
				CapsuleRenderer.Draw(spriteBatch, new Transformation(GraphicsHandler.main.children["game"].position, 2), GraphicsDevice);
			// End my suffering


			GraphicsDevice.SetRenderTarget(null);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

			spriteBatch.Draw(RenderTarget, Vector2.Zero, Color.White);
			spriteBatch.End();

			RenderTarget.Dispose();
		}

		// TODO: Create a separate "error handler" class with all these error messages/screens
		public void DrawDeathScreen()
		{
			string errorMessage = displayLanguage == 0 ?
				"An error was encountered while loading data from JSON.\n " +
				"The offending file is located at " + jsonLoaderFilePath.Replace("\\", "/") + " within the game directory.\n " +
				"If you have installed a modification that changes JSON or tried modifying the JSON yourself, this may be the cause. If the case is the former, check for mod conflicts or contact the mod authors.\n " +
				"If possible, try to revert any changes you have made. If you cannot, visit https://github.com/Rebmiami/HololiveFightingGame, find the correct version of the offending file, and replace the offending file with it.\n " +
				"The exact error message is as follows. This may help you diagnose and solve the issue:\n " +
				jsonErrorMessage :

				"JSON エラー\n " +
				"エラーメッセージ：\n " +
				jsonErrorMessage;

			errorMessage += displayLanguage == 0 ?
				"\n Press ESC to exit the program. Press ENTER to open File Explorer to the game directory. Press SHIFT+ESC to close and re-launch the game if you have fixed the issue." :

				"\n Press ESC to exit the program. Press ENTER to open File Explorer to the game directory. Press SHIFT+ESC to close and re-launch the game if you have fixed the issue.";

			string[] words = errorMessage.Split(' ');
			StringBuilder sb = new StringBuilder();
			float lineWidth = 0f;
			float spaceWidth = Assets.font.MeasureString(" ").X;
			int maxLineWidth = Program.WindowBounds().Width - 20;

			foreach (string word in words)
			{
				Vector2 size = Assets.font.MeasureString(word);

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

			spriteBatch.DrawString(Assets.font, errorMessage, new Vector2(10), Color.White);
		}
	}

	public enum DisplayLanguage
	{
		EN,
		JP
	}
}
