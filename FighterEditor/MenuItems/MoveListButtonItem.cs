using HololiveFightingGame.Combat;
using HololiveFightingGame.Graphics;
using HololiveFightingGame.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.MenuItems
{
	class MoveListButtonItem : EditorMenuItem
	{
		public string move;

		public MoveListButtonItem(EditorMenu parent, int ID, string move) : base(parent, ID)
		{
			this.move = move;
			button = true;
			lowestLevel = true;
		}

		public override void Escape(ref object target)
		{
			Editor.currentMove = FighterLoader.moves[Editor.fighter.character][move];
			Editor.fighter.moveRunner = new MoveRunner(Editor.currentMove);
			((AnimatedSprite)Editor.fighter.drawObject.texture).SwitchAnimation(Editor.currentMove.Data.Name, 0);
			Editor.fighter.moveTimer = Editor.fighter.moveRunner.data.MoveDuration;
			base.Escape(ref target);
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position, int i)
		{
			Button.Draw(spriteBatch, new Rectangle((int)position.X - 2 + 120, (int)position.Y - 2 + i * 20, 100, 20), Flavor);
			spriteBatch.DrawString(Assets.font, move, position + new Vector2(120, i * 20), Color.White);
		}
	}
}
