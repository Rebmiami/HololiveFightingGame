using HololiveFightingGame.Combat;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Input;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Input;

namespace HololiveFightingGame.MoveEditor
{
	public static class Editor
	{
		public static Fighter fighter;

		public static void Load()
		{
			// Unloads previously loaded graphics
			GraphicsHandler.main.children.Clear();
			GraphicsHandler.main.children.Add("game", new DrawObject(DrawObjectType.Layer));

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
					fighter.drawObject.position += new Vector2(100);
				}
			}
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			if (fighter == null)
			{
				spriteBatch.DrawString(Assets.font, "There is currently no fighter loaded.\nPress CTRL+O to select a fighter.", Vector2.Zero, Color.White);
			}
			else
			{
				GraphicsHandler.main.Draw(spriteBatch, new Transformation(MovePreviewer.Pan, MovePreviewer.Zoom));

				string[] attackNames = Enum.GetNames(typeof(MoveType));

                for (int i = 0; i < attackNames.Length; i++)
                {
					string name = attackNames[i];
					spriteBatch.DrawString(Assets.font, name, new Vector2(8, 8 + i * 16), Color.White);
				}
			}
		}
	}
}
