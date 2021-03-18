using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Combat.Moves
{
	public class PekoraMoves : MoveData
	{
		// TODO: LOAD THIS FROM FILE!!!
		public override string JSON()
		{
			return 
				"{ \"Damage\": 103," +
				" \"Angle\": 45," +
				" \"Launch\": 14," +
				" \"Dims\": [" +
				"0," +
				"0," +
				"25," +
				"0," +
				"5" +
				"] }";
		}
	}
}
