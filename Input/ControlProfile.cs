using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using HololiveFightingGame.Input;
using HololiveFightingGame.Loading;

namespace HololiveFightingGame.Input
{
	public class ControlProfile
	{
		public Keys Keyboard_Attack;
		public Keys Keyboard_MoveLeft;
		public Keys Keyboard_MoveRight;
		public Keys Keyboard_Jump;

		public MouseButtons Mouse_Attack;
		public MouseButtons Mouse_MoveLeft;
		public MouseButtons Mouse_MoveRight;
		public MouseButtons Mouse_Jump;
		// Mouse and Keyboard are treated as one controller.

		public Buttons Gamepad_Attack;
		public Buttons Gamepad_MoveLeft;
		public Buttons Gamepad_MoveRight;
		public Buttons Gamepad_Jump;

		// TODO: Create a class for handling all inputs.
		// Something involving the Keybinds class?
		// Do this before adding any more inputs

		public ControlProfile(ControlProfileLoader loader)
		{
			Keyboard_Attack = GetKeys(loader.Attack);
			Keyboard_MoveLeft = GetKeys(loader.MoveLeft);
			Keyboard_MoveRight = GetKeys(loader.MoveRight);
			Keyboard_Jump = GetKeys(loader.Jump);
		}

		public void SetKeybind()
		{

		}

		public Keys GetKeys(string keybind)
		{
			string[] keyNames = Enum.GetNames(typeof(Keys));
			foreach (string name in keyNames)
			{
				if (keybind == name)
				{
					return Enum.Parse<Keys>(keybind);
				}
			}
			Program.WriteErrorLog(new ArgumentException(BadKeybindErrorMessage()));
			return Keys.None;
		}

		// public MouseButton GetMouseButton(string keybind)
		// {
		// 
		// }

		public static string BadKeybindErrorMessage()
		{
			StringBuilder error = new StringBuilder();
			error.Append("A control profile with invalid keybinds was found while loading. The JSON is formatted properly, but one or more keybinds specified could not be mapped to a key or button.\n\n");
			error.Append("Here is a list of valid keys:\n");
			foreach (string keyname in Enum.GetNames(typeof(Keys)))
			{
				error.Append("\"" + keyname + "\" ");
			}
			error.Append("\n\nHere is a list of valid mouse buttons:\n");
			foreach (string keyname in Enum.GetNames(typeof(MouseButtons)))
			{
				error.Append("\"" + keyname + "\" ");
			}
			error.Append("\n\nHere is a list of valid controller buttons:\n");
			foreach (string keyname in Enum.GetNames(typeof(Buttons)))
			{
				error.Append("\"" + keyname + "\" ");
			}
			error.Append("\n\nNote that these are case sensitive!");

			return error.ToString();
			// I've decided to handle invalid keybinds differently than other JSON errors so I can list all keybinds to make it easier for the user to resolve the issue until a better solution is implemented.
		}
	}
}
