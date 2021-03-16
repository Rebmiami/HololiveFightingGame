using System;
using System.Collections.Generic;
using System.Text;
using HololiveFightingGame.Collision;
using Microsoft.Xna.Framework;

namespace HololiveFightingGame.Combat
{
	public static class MoveLoader
	{
		public static List<Move>[] moves;

		public static void LoadMoves(Fighter[] fighters)
		{
			FighterCharacter[] characters = new FighterCharacter[fighters.Length];
			for (int i = 0; i < fighters.Length; i++)
			{
				characters[i] = fighters[i].character;
			}
			LoadMoves(characters);
		}

		public static void LoadMoves(FighterCharacter[] fighters)
		{
			moves = new List<Move>[fighters.Length];
			// TODO: Load a list of moves from JSON and add it to this list
		}

		public static Move TestMove()
		{
			Move move = new Move();
			move.hitboxes = new AttackHitbox[1];
			move.hitboxes[0] = new AttackHitbox()
			{
				damage = 103,
				type = AttackHitboxType.Attack,
				collider = new Collider(new Capsule(Vector2.Zero, new Vector2(25, 0), 5))
			};
			return move;
		}
		// Temporary - will be removed soon
	}
}
