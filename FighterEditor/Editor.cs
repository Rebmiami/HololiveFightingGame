using HololiveFightingGame.Combat;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Input;
using HololiveFightingGame.Loading;
using HololiveFightingGame.FighterEditor.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text.RegularExpressions;
using HololiveFightingGame.Graphics.CapsuleShader;
using HololiveFightingGame.Collision;

namespace HololiveFightingGame.FighterEditor
{
	public static class Editor
	{
		public static Fighter fighter;

		public static EditorMenu[] menus;
		public static int leftMenu;
		public static int rightMenu;

		public const int panelOffset = 546;
		// TODO: Make this dependent on screen size (when resizing screens is implemented)

		public static int activeMenu;

		public static int ActiveMenu
		{
			get
			{
				return activeMenu == 1 ? rightMenu : leftMenu;
			}
		}

		public static Move currentMove;
		public static string currentAnimation;
		// If currentMove is not null, then currentAnimation should always match the animation of currentMove.
		// Find some way to ensure this is always the case?
		// Note that animation names for moves are identical to the associated move's name.
		// Perhaps use the MoveType_# format for moves and single words for animations?
		public static string CurrentActionName
		{
			get
			{
				if (CurrentActionIsMove)
				{
					return currentMove.Data.Name;
				}
				else
				{
					return currentAnimation;
				}
			}
		}
		public static bool CurrentActionIsMove
		{
			get
			{
				return currentMove != null;
			}
		}

		public static void Load()
		{
			// Unloads previously loaded graphics
			ResetGraphics();
			// MessageBox.Show("Welcome to the move editor!\n\nWhen you press \"OK\", you will be prompted to select a fighter to edit."); // If you need help at any point, you can press the * key to open documentation.\n\nThis message will only be shown once.", "Move Editor");
		}

		public static void ResetGraphics()
		{
			GraphicsHandler.main.children.Clear();
			GraphicsHandler.main.children.Add("game", new DrawObject(DrawObjectType.Layer));
		}

		public static void Update()
		{
			if (KeyHelper.Down(Keys.LeftControl) && KeyHelper.Pressed(Keys.O))
			{
				LoadFighter();
			}

			if (fighter != null)
			{
				if (KeyHelper.Pressed(Keys.Tab))
				{
					activeMenu++;
					activeMenu %= 2;
				}

				for (int i = 0; i < menus.Length; i++)
				{
					if (KeyHelper.Pressed((Keys)(i + 49)))
					{
						if (KeyHelper.Down(Keys.LeftShift))
						{
							if (leftMenu == i)
							{
								leftMenu = rightMenu;
								activeMenu++;
								activeMenu %= 2;
							}
							rightMenu = i;
						}
						else
						{
							if (rightMenu == i)
							{
								rightMenu = leftMenu;
								activeMenu++;
								activeMenu %= 2;
							}
							leftMenu = i;
						}
					}
					menus[rightMenu].Update();
					menus[leftMenu].Update();
				}

				if (menus[ActiveMenu].items.Length > 0)
				{
					if (KeyHelper.Pressed(Keys.W))
					{
						menus[ActiveMenu].Scroll(true);
					}

					if (KeyHelper.Pressed(Keys.S))
					{
						menus[ActiveMenu].Scroll(false);
					}
				}

				if (KeyHelper.Pressed(Keys.D) && menus[ActiveMenu].CurrentItemPool.Length > 0)
				{
					if (menus[ActiveMenu].HighlightedItem.lowestLevel)
					{
						if (menus[ActiveMenu].HighlightedItem.button)
						{
							object obj = new object();
							menus[ActiveMenu].HighlightedItem.Escape(ref obj);
						}
						else
						{
							// TODO: Call "erase" and implement editing
						}
					}
					else if (menus[ActiveMenu].HighlightedItem.children.Length > 0)
					{
						menus[ActiveMenu].HighlightedItem.Enter();
					}
				}

				if (KeyHelper.Pressed(Keys.A) && menus[ActiveMenu].escapeRoute.Count > 0)
				{
					object obj = new object();
					menus[ActiveMenu].escapeRoute.Peek().Escape(ref obj);
				}
				MovePreviewer.Update();
				fighter.AddCapsules();
			}
		}

		public static void LoadFighter()
		{
			System.Windows.Forms.OpenFileDialog fighterSelect = new System.Windows.Forms.OpenFileDialog()
			{
				InitialDirectory = Game1.gamePath + @"\Content\Data\Fighters",
				FileName = "[Name].json",
				Filter = "JSON files (*.json)|*.json",
				Title = "Select a fighter JSON file"
			};
			string selected;
			if (fighterSelect.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				selected = new Regex(@".+\\(.+)\..+").Replace(fighterSelect.FileName, "$1");
				// Sets up a fighter to be edited
				fighter = new Fighter(0, selected);
				FighterLoader.LoadMoves(new Fighter[] { fighter });
				FighterLoader.LoadAnimations(new Fighter[] { fighter });
				fighter.Bottom = Program.WindowBounds().Size.ToVector2() / 2;
				fighter.takeInputs = false;
				fighter.drawObject.Bottom = fighter.Bottom;

				menus = new EditorMenu[]
				{
					new FileMenu(),
					new FighterMenu(),
					new MoveMenu(),
					new AnimationMenu(),
					new MoveEditor(),
					new AnimationEditor(),
					new HurtboxEditor(),
					new HitboxEditor()
				};
				leftMenu = 2;
				rightMenu = 4;
			}
		}

		public static void ResetFighter()
		{
			ResetGraphics();
			fighter = new Fighter(fighter.ID, fighter.character)
			{
				moveRunner = new MoveRunner(currentMove)
			};
			FighterLoader.LoadAnimations(new Fighter[] { fighter });
			fighter.Bottom = Program.WindowBounds().Size.ToVector2() / 2;
			fighter.takeInputs = false;
			((AnimatedSprite)fighter.drawObject.texture).SwitchAnimation(currentMove.Data.Name, 0);
			fighter.moveTimer = fighter.moveRunner.data.MoveDuration;
			fighter.Update();
			fighter.Update_Animation();
		}

		public static void Save()
		{
			// All changes made in editor are written to classes that can be directly serialized to JSON.
			// TODO: Serialize fighter data and write to changed files
			throw new NotImplementedException();
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			if (fighter != null)
			{
				GraphicsHandler.main.Draw(spriteBatch, new Transformation(MovePreviewer.Pan, MovePreviewer.Zoom));
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

			if (fighter == null)
			{
				spriteBatch.DrawString(Assets.font, "There is currently no fighter loaded.\nPress CTRL+O to select a fighter.", new Vector2(8), Color.White);
			}
			else
			{
				Button.Draw(spriteBatch, new Rectangle(2, 2, 250, 420), activeMenu == 0 ? 2 : 0);
				menus[leftMenu].Draw(spriteBatch, false);

				Button.Draw(spriteBatch, new Rectangle(2 + panelOffset, 2, 250, 420), activeMenu == 1 ? 2 : 0);
				menus[rightMenu].Draw(spriteBatch, true);

				Button.Draw(spriteBatch, new Rectangle(254, 2, 292, 30));
				string text;
				if (currentMove == null && currentAnimation == null)
				{
					text = "No move or animation selected.";
				}
				else if (CurrentActionIsMove)
				{
					text = "Current Move: " + CurrentActionName;
				}
				else
				{
					text = "Current Anim: " + CurrentActionName;
				}
				spriteBatch.DrawString(Assets.font, text, new Vector2(262, 8), Color.White);
				spriteBatch.Draw(Assets.editorPlayIcon, new Vector2(260, 80), new Rectangle(MovePreviewer.Playing ? 9 : 0, 0, 8, 8), Color.White);
			}
		}
	}
}
