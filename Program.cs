using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HololiveFightingGame
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
#if DEBUG
			using (var game1 = new Game1())
			{
				game = game1;

				game1.Run();
			}
#else
			try
			{
				using (var game1 = new Game1())
				{
					game = game1;

					game1.Run();
				}
			}
			catch (Exception exception)
			{
				WriteErrorLog(exception);
			}
			// In release builds, use a custom error handler as Visual Studio debugging will not be available to most users.
#endif
		}

		public static Game1 game;

		public static Rectangle WindowBounds()
		{
			return game.GraphicsDevice.Viewport.Bounds;
		}

		public static void WriteErrorLog(Exception exception)
        {
			StringBuilder error = new StringBuilder();

			error.Append("--- AUTO-GENERATED CRASH REPORT ---\n");
			error.Append(
				new Random().Next(11) switch
				{
					0 => "Try and catch me!",
					1 => "Error messages have feelings too...",
					2 => "Don't do spaghetti code, kids.",
					3 => "Roses are red, violets are blue, missing semicolon on line 32.",
					4 => "Oh, the joys of game development.",
					5 => "You may be entitled to a refund.",
					6 => "Someone was stupid here. It was probably Reb.",
					7 => "Turning a comma into way more of a problem than it had any right to be since 2021.",
					8 => "Error 418: I cannot run games because I am a teapot.",
					9 => "AHFG stands for \"AHFG had fun, goodbye.\"",
					10 => "\"text flavoe\" ~ Chef \"Cursed\" Crust, 2021",
					_ => "An error on the crash report? Oh, the irony.",
				} + "\n\n"
			);

			error.Append("Something went wrong and the game had to close unexpectedly.\n");
			error.Append("Please send a copy of this file to one of the game's developers so we can figure out what went wrong.\n");
			error.Append("If you are using a modified version of the game, contact the developers of the modification as well.\n");

			error.Append("The error message and stack trace are as follows:\n\n");

			error.Append(exception.Message + "\n");

			error.Append(exception.StackTrace + "\n\n");

			error.Append("The game's files are located at " + Game1.gamePath + ", including this log file.");


			using StreamWriter sw = new StreamWriter(Game1.gamePath + @"\logs.txt");
			foreach (char ch in error.ToString())
			{
				sw.Write(ch);
			}
			Process.Start("notepad.exe", Game1.gamePath + @"\logs.txt");
			game.Exit();
		}
	}
}
