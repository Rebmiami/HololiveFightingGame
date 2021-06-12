using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	/// <summary>
	/// A set of positions used by the editor for handling the placement of UI objects.
	/// </summary>
	public static class EditorOffsets
	{
		/// <summary>
		/// Used to create a small area of blank space between panels and bars.
		/// </summary>
		public static Point panelPadding = new Point(2, 2);

		/// <summary>
		/// Used to create a small area of blank space between the edges of panels/bars and their contents.
		/// </summary>
		public static Point innerPadding = new Point(6, 6);

		/// <summary>
		/// The number of pixels between the top of the panel header and the panel's contents.
		/// </summary>
		public static int headerHeight = 25;

		/// <summary>
		/// The dimensions of the overhead bar.
		/// </summary>
		public static readonly Rectangle overhead = new Rectangle(
			panelPadding.X,
			panelPadding.Y,
			800,
			30
			);

		/// <summary>
		/// The difference in pixels between the X positions of the left and right panel origins.
		/// </summary>
		public static readonly int rightPanelOffset = 546;

		/// <summary>
		/// The dimensions of the left panel.
		/// </summary>
		public static readonly Rectangle leftPanel = new Rectangle(
			0 + panelPadding.X,
			overhead.Y + overhead.Height + panelPadding.Y,
			250,
			420
			);

		/// <summary>
		/// The dimensions of the right panel.
		/// </summary>
		public static readonly Rectangle rightPanel = new Rectangle(
			rightPanelOffset + panelPadding.X,
			overhead.Y + overhead.Height + panelPadding.Y,
			250,
			420
			);

		/// <summary>
		/// The size of editor icons.
		/// </summary>
		public static readonly Point iconSize = new Point(24, 24);
	}
}
