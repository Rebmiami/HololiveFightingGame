using HololiveFightingGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.GUI
{
	public static class MenuNavTabs
	{
		public static Rectangle[] leftTabs;
		public static Rectangle[] rightTabs;
		public static EditorIcon[] leftIcons;
		public static EditorIcon[] rightIcons;

		static MenuNavTabs()
		{
			leftTabs = new Rectangle[4];
			rightTabs = new Rectangle[7];
			leftIcons = new EditorIcon[]
			{
				EditorIcon.MenuFile,
				EditorIcon.MenuFighter,
				EditorIcon.MenuMove,
				EditorIcon.MenuAnimation,
			};
			rightIcons = new EditorIcon[]
			{
				EditorIcon.EditorMove,
				EditorIcon.EditorAnimation,
				EditorIcon.EditorHitbox,
				EditorIcon.EditorDynamics,
				EditorIcon.EditorAI,
				EditorIcon.EditorProjectile,
				EditorIcon.EditorEntity,
			};

			Point leftOrigin = new Point(EditorOffsets.leftPanel.Right, EditorOffsets.leftPanel.Top);
			Point rightOrigin = new Point(EditorOffsets.rightPanel.Left - EditorOffsets.iconSize.X, EditorOffsets.rightPanel.Top);
			for (int i = 0; i < 4; i++)
			{
				leftTabs[i] = new Rectangle(leftOrigin, EditorOffsets.iconSize);
				leftOrigin.Y += EditorOffsets.iconSize.Y;
			}
			for (int i = 0; i < 7; i++)
			{
				rightTabs[i] = new Rectangle(rightOrigin, EditorOffsets.iconSize);
				rightOrigin.Y += EditorOffsets.iconSize.Y;
			}
		}

		public static void Draw(SpriteBatch spritebatch)
		{
			for (int i = 0; i < leftTabs.Length; i++)
			{
				DrawTab(spritebatch, leftTabs[i], leftIcons[i], i == Editor.leftMenu, true);
			}
			for (int i = 0; i < rightTabs.Length; i++)
			{
				DrawTab(spritebatch, rightTabs[i], rightIcons[i], i == Editor.rightMenu - 4, false);
			}
		}

		public static void DrawTab(SpriteBatch spriteBatch, Rectangle button, EditorIcon icon, bool open, bool left)
		{
			Rectangle stretchedButton = button;
			bool[] sides;
			if (left)
			{
				sides = new bool[] {
					false, true, true,
					false, true, true,
					false, true, true
				};
				stretchedButton.X -= open ? 4 : 2;
				stretchedButton.Width += open ? 4 : 2;
			}
			else
			{
				sides = new bool[] {
					true, true, false,
					true, true, false,
					true, true, false
				};
				stretchedButton.Width += open ? 4 : 2;
			}

			Button.Draw(spriteBatch, stretchedButton, ButtonFlavor.Latent, sides);
			IconArtist.DrawIcon(spriteBatch, button.Location.ToVector2() + new Vector2(left ? -1 : 1, 0), icon);
		}

		public static void Update()
		{
			for (int i = 0; i < leftTabs.Length; i++)
			{
				if (MouseHelper.Pressed(MouseButtons.Left) && leftTabs[i].Contains(Mouse.GetState().Position))
					Editor.leftMenu = i;
			}
			for (int i = 0; i < rightTabs.Length; i++)
			{
				if (MouseHelper.Pressed(MouseButtons.Left) && rightTabs[i].Contains(Mouse.GetState().Position))
					Editor.rightMenu = i + 4;
			}
		}
	}
}
