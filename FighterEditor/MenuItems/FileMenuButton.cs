using HololiveFightingGame.Combat;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace HololiveFightingGame.FighterEditor.MenuItems
{
	public class FileMenuButton : EditorUIItem
	{
		public FileButtonAction action;

		public FileMenuButton(EditorMenu parentMenu, int ID) : base(parentMenu, ID)
		{
			action = (FileButtonAction)ID;
			type = EditorMenuItemType.Button;
		}

		public override void Escape(ref object target)
		{
			switch (action)
			{
				case FileButtonAction.Open:
					// Editor.ResetGraphics();

					// TODO:
					// Add method for emptying editor if necessary
					// Add warning before emptying editor

					Editor.OpenFighter();
					break;
				case FileButtonAction.Save:
					Editor.Save();
					break;
				case FileButtonAction.SaveAs:
					// TODO: Open save dialog and change focus to new location
					break;
				case FileButtonAction.ReloadMoves:
					FighterLoader.ReloadMoves();
					MovePreviewer.Refresh();
					break;
				case FileButtonAction.ReloadFighterTexture:
					Editor.ResetFighter();
					break;
				case FileButtonAction.ReloadAllTextures:
					// TODO: Add this when there are actually textures to reload
					break;
				case FileButtonAction.ReloadSounds:

					break;
				case FileButtonAction.ReloadAssets:

					break;
				case FileButtonAction.OpenFileLocation:
					// new Regex(@"\\HololiveFightingGame\.exe$").Replace(Process.GetCurrentProcess().MainModule.FileName, "");
					// Process.Start("explorer.exe", Editor.loadedFighterPath);
					break;
				case FileButtonAction.Exit:
					Program.game.Exit();
					break;
				default:
					break;
			}
			// base.Escape(ref target);
			// Some actions make this impossible
		}

		public override void Refresh()
		{
			Vector2 origin = new Vector2(8, 8);
			// origin.X += 546;
			origin.Y += 25;
			origin.Y += 32;
			clickbox = new Rectangle((int)origin.X - 2, (int)origin.Y - 2 + ID * 20, 150, 20);
			base.Refresh();
		}

		public override void Draw(SpriteBatch spriteBatch, Vector2 position, ref bool drawChildren)
		{
			base.Draw(spriteBatch, position, ref drawChildren);
			spriteBatch.DrawString(Assets.font, Enum.GetNames(typeof(FileButtonAction))[(int)action], clickbox.Location.ToVector2() + new Vector2(2, 2), Color.White);
		}

		public enum FileButtonAction
		{
			/// <summary>
			/// Prompts the user to open a fighter's file. If there are unsaved changes, the user will be warned.
			/// </summary>
			Open,
			/// <summary>
			/// Saves the loaded fighter.
			/// </summary>
			Save,
			/// <summary>
			/// Allows the user to save the fighter to a new location and sets the focus to the new location.
			/// </summary>
			SaveAs,
			/// <summary>
			/// Clears the editor and reloads the focused fighter from JSON. If there are unsaved changes, the user will be warned.
			/// </summary>
			ReloadMoves,
			/// <summary>
			/// Reloads the player sprites belonging to the focused fighter.
			/// </summary>
			ReloadFighterTexture,
			/// <summary>
			/// Reloads all sprites related to the focused fighter.
			/// </summary>
			ReloadAllTextures,
			/// <summary>
			/// Reloads all sound effects related to the focused fighter.
			/// </summary>
			ReloadSounds,
			/// <summary>
			/// Reloads all graphical and audio assets related to the focused fighter.
			/// </summary>
			ReloadAssets,
			/// <summary>
			/// Opens File Explorer to the focused address.
			/// </summary>
			OpenFileLocation,
			/// <summary>
			/// Exits the Fighter Editor. If there are unsaved changes, the user will be warned.
			/// </summary>
			Exit
		}
	}
}
