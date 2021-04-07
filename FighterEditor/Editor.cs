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

namespace HololiveFightingGame.FighterEditor
{
	public static class Editor
	{
		public static Fighter fighter;
		public static int cursor;
		public static int items;

		public static EditorMenu[] menus;
		public static int leftMenu;
		public static int rightMenu;

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

		public static void Load()
		{
			// Unloads previously loaded graphics
			GraphicsHandler.main.children.Clear();
			GraphicsHandler.main.children.Add("game", new DrawObject(DrawObjectType.Layer));

			items = Enum.GetNames(typeof(MoveType)).Length;

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

			// MessageBox.Show("Welcome to the move editor!\n\nWhen you press \"OK\", you will be prompted to select a fighter to edit."); // If you need help at any point, you can press the * key to open documentation.\n\nThis message will only be shown once.", "Move Editor");
		}

		public static void Update()
		{
			if (KeyHelper.Down(Keys.LeftControl) && KeyHelper.Pressed(Keys.O))
			{
				System.Windows.Forms.OpenFileDialog fighterSelect = new System.Windows.Forms.OpenFileDialog()
				{
					InitialDirectory = Game1.gamePath + @"\Content\Data\Fighters",
					FileName = "[Name].json",
					Filter = "JSON files (*.json)|*.json",
					Title = "Select a fighter JSON file"
				};
				string selected = "";
				if (fighterSelect.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					selected = new Regex(@".+\\(.+)\..+").Replace(fighterSelect.FileName, "$1");
					// Sets up a fighter to be edited
					fighter = new Fighter(0, selected);
					FighterLoader.LoadMoves(new Fighter[] { fighter });
					FighterLoader.LoadAnimations(new Fighter[] { fighter });
					fighter.Bottom = Program.WindowBounds().Size.ToVector2() / 2;

					fighter.drawObject.Bottom = fighter.Bottom;
				}
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
				}

				if (menus[ActiveMenu].itemCount > 0)
				{
					if (KeyHelper.Pressed(Keys.W))
					{
						menus[ActiveMenu].cursor -= 1;
						if (menus[ActiveMenu].cursor < 0)
						{
							menus[ActiveMenu].cursor = menus[ActiveMenu].itemCount - 1;
						}
					}

					if (KeyHelper.Pressed(Keys.S))
					{
						menus[ActiveMenu].cursor++;
						menus[ActiveMenu].cursor %= menus[ActiveMenu].itemCount;
					}
				}
			}
		}

		public static void Save()
		{
			// All changes made in editor are written to classes that can be directly serialized to JSON.
			// TODO: Serialize fighter data and write to changed files
			throw new NotImplementedException();
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			if (fighter == null)
			{
				spriteBatch.DrawString(Assets.font, "There is currently no fighter loaded.\nPress CTRL+O to select a fighter.", new Vector2(8), Color.White);
			}
			else
			{
				GraphicsHandler.main.Draw(spriteBatch, new Transformation(MovePreviewer.Pan, MovePreviewer.Zoom));

				menus[leftMenu].Draw(spriteBatch, false);
				menus[rightMenu].Draw(spriteBatch, true);
			}
		}
	}
}
