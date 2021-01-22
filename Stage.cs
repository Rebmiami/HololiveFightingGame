using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame
{
	public class Stage
	{
		public Rectangle collider;
		
		public Stage()
		{
			collider = new Rectangle(150, 300, 500, 100);
		}
	}
}
