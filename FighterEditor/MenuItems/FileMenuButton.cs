using HololiveFightingGame.Combat;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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

					Editor.LoadFighter();
					break;
				case FileButtonAction.Save:
					Editor.Save();
					break;
				case FileButtonAction.SaveAs:
					break;
				case FileButtonAction.ReloadFighterTexture:
					break;
				case FileButtonAction.ReloadAllTextures:
					break;
				case FileButtonAction.ReloadSounds:
					break;
				case FileButtonAction.ReloadAssets:
					break;
				case FileButtonAction.OpenFileLocation:
					break;
				case FileButtonAction.Exit:
					break;
				default:
					break;
			}
			base.Escape(ref target);
		}

		public override void Refresh()
		{
			Vector2 origin = new Vector2(8, 8);
			// origin.X += 546;
			origin.Y += 25;
			origin.Y += 32;
			clickbox = new Rectangle((int)origin.X - 2 + 120, (int)origin.Y - 2 + ID * 20, 100, 20);
			base.Refresh();
		}

		public override void Draw(SpriteBatch spriteBatch, Vector2 position, ref bool drawChildren)
		{
			base.Draw(spriteBatch, position, ref drawChildren);
			spriteBatch.DrawString(Assets.font, Enum.GetNames(typeof(FileButtonAction))[(int)action], clickbox.Location.ToVector2() + new Vector2(2, 2), Color.White);
		}

		public enum FileButtonAction
		{
			Open,
			Save,
			SaveAs,
			ReloadFighterTexture,
			ReloadAllTextures,
			ReloadSounds,
			ReloadAssets,
			OpenFileLocation,
			Exit
		}
	}
}
