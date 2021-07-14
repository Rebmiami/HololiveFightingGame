using HololiveFightingGame.FighterEditor.GUI;
using HololiveFightingGame.FighterEditor.Menus;
using HololiveFightingGame.Gameplay.Combat;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Input;
using HololiveFightingGame.Loading;
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

		public static string loadedFighterPath;

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

		/// <summary>
		/// Empties the editor all the way.
		/// </summary>
		public static void ClearEditor()
		{
			ResetGraphics();
			menus = null;
			fighter = null;
			currentMove = null;
			currentAnimation = null;
		}

		public static void Update()
		{
			if (KeyHelper.Down(Keys.LeftControl) && KeyHelper.Pressed(Keys.O))
			{
				OpenFighter();
			}

			if (fighter != null)
			{
				if (KeyHelper.Pressed(Keys.Tab))
				{
					activeMenu++;
					activeMenu %= 2;
				}
				MenuNavTabs.Update();

				for (int i = 0; i < 7; i++)
				{
					if (KeyHelper.Pressed((Keys)(i + 49)) && KeyHelper.Down(Keys.LeftControl) || KeyHelper.Down(Keys.RightControl))
					{
						if (KeyHelper.Down(Keys.LeftShift) || KeyHelper.Down(Keys.RightShift))
						{
							rightMenu = i + 4;
						}
						else
						{
							if (i <= 3)
								leftMenu = i;
						}

						// Old nav system. Some users may prefer this, so it may be reimplemented later.
						// This code no longer works, so it used to allow the user to set any panel to any menu or editor.
						// The functionality to allow this kind of system is still present however.
						// Changes will need to be made to account for the increased number of editors.
						// Perhaps use the Alt key?
						// if (KeyHelper.Down(Keys.LeftShift))
						// {
						// 	if (leftMenu == i)
						// 	{
						// 		leftMenu = rightMenu;
						// 		activeMenu++;
						// 		activeMenu %= 2;
						// 	}
						// 	rightMenu = i;
						// }
						// else
						// {
						// 	if (rightMenu == i)
						// 	{
						// 		rightMenu = leftMenu;
						// 		activeMenu++;
						// 		activeMenu %= 2;
						// 	}
						// 	leftMenu = i;
						// }
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
						if (menus[ActiveMenu].HighlightedItem.type == EditorMenuItemType.Button)
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

		public static void OpenFighter()
		{
			System.Windows.Forms.OpenFileDialog fighterSelect = new System.Windows.Forms.OpenFileDialog()
			{
				InitialDirectory = Game1.gamePath + @"\Content\Data\Fighters",
				FileName = "[Name].json",
				Filter = "JSON files (*.json)|*.json",
				Title = "Select a fighter JSON file"
			};
			if (fighterSelect.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				LoadFighter(fighterSelect.FileName);
			}
		}

		public static void LoadFighter(string path)
		{
			// Clears the editor
			ClearEditor();
			loadedFighterPath = new Regex(@"(.+)\\.+\..+").Replace(path, "$1");

			string selected;
			selected = new Regex(@".+\\(.+)\..+").Replace(path, "$1");
			// Sets up a fighter to be edited
			fighter = new Fighter(0, selected);
			FighterLoader.LoadMoves(new Fighter[] { fighter });
			FighterLoader.LoadAnimations(new Fighter[] { fighter });
			fighter.Bottom = Program.WindowBounds().Size.ToVector2() / 2;
			fighter.takeInputs = false;
			fighter.drawObject.Bottom = fighter.Bottom;

			// TODO: Handle invalid files being selected

			menus = new EditorMenu[]
			{
				new FileMenu(),
				new FighterMenu(),
				new MoveMenu(),
				new AnimationMenu(),
				new MoveEditor(),
				new AnimationEditor(),
				new HitboxEditor(),
				new DynamicsEditor(),
				new AIEditor(),
				new ProjectileEditor(),
			};
			leftMenu = 2;
			rightMenu = 4;
			foreach (EditorMenu menu in menus)
			{
				menu.Refresh();
			}
		}

		public static void ResetFighter()
		{
			ResetGraphics();
			fighter = new Fighter(fighter.ID, fighter.character);

			if (currentMove != null)
				fighter.moveRunner = new MoveRunner(currentMove);

			FighterLoader.LoadAnimations(new Fighter[] { fighter });
			fighter.Bottom = Program.WindowBounds().Size.ToVector2() / 2;
			fighter.takeInputs = false;


			if (currentMove != null)
			{
				((AnimatedSprite)fighter.drawObject.texture).SwitchAnimation(currentMove.Data.Name, 0);
				fighter.moveTimer = fighter.moveRunner.data.MoveDuration;

				fighter.Update();
				fighter.Update_Animation();
			}
		}

		public static void Save()
		{
			// All changes made in editor are written to classes that can be directly serialized to JSON.

			FighterLoader.fighterData[fighter.character].Write(loadedFighterPath);
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
				menus[leftMenu].Draw(spriteBatch, false);
				menus[rightMenu].Draw(spriteBatch, true);

				Button.Draw(spriteBatch, EditorOffsets.overhead);
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
				//TODO: Add overhead text support for projectiles and entities
				spriteBatch.DrawString(Assets.font, text, new Vector2(262, 8), Color.White);
				MenuNavTabs.Draw(spriteBatch);
				IconDrawer.DrawIcon(spriteBatch, new Vector2(260, 160), MovePreviewer.Playing ? EditorIcon.PreviewPause : EditorIcon.PreviewPlay);
			}
		}
	}
}
