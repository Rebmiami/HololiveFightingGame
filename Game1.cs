using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HololiveFightingGame
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch spriteBatch;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		public static GameState gameState;

		protected override void Initialize()
		{
			gameState = new GameState();

			testFighter = Content.Load<Texture2D>("TestFighter");
			testStage = Content.Load<Texture2D>("TestStage");

			base.Initialize();
		}

		public static Texture2D testFighter;
		public static Texture2D testStage;

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			gameState.Update();

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.DarkGray);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
			spriteBatch.Draw(testFighter, gameState.fighters[0].position, Color.White);
			spriteBatch.Draw(testStage, gameState.stage.collider.Location.ToVector2(), Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
