﻿using HololiveFightingGame.Combat;
using HololiveFightingGame.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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

			MessageBox.Show("Welcome to the move editor!\n\nWhen you press \"OK\", you will be prompted to select a fighter to edit."); // If you need help at any point, you can press the * key to open documentation.\n\nThis message will only be shown once.", "Move Editor");

			OpenFileDialog fighterSelect = new OpenFileDialog()
			{
				InitialDirectory = Game1.gamePath + @"\Content\Data\Fighters",
				FileName = "[Name].json",
				Filter = "JSON files (*.json)|*.json",
				Title = "Select a fighter JSON file"
			};
			string selected = "";
			if (fighterSelect.ShowDialog() == DialogResult.OK)
			{
				selected = new Regex(@".+\\(.+)\..+").Replace(fighterSelect.FileName, "$1");
				MessageBox.Show(selected);
			}

			// Sets up a fighter to be edited
			fighter = new Fighter(0, selected);
			FighterLoader.LoadMoves(new Fighter[] { fighter });
			FighterLoader.LoadAnimations(new Fighter[] { fighter });
			fighter.drawObject.position += new Vector2(100);
		}
	}
}
