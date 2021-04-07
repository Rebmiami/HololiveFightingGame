using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	public abstract class EditorMenuItem
	{
		public virtual void Enter()
		{
			Editor.menuItem = this;
		}

		public virtual void Erase()
		{

		}

		public virtual void Edit()
		{

		}

		public virtual void Escape()
		{
			Editor.menuItem = null;
		}
	}
}
