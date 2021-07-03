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
		public Keybind Attack;
		public Keybind AttackB;
		public Keybind ControlLeft;
		public Keybind ControlRight;
		public Keybind ControlUp;
		public Keybind ControlDown;
		public Keybind Jump;
		// TODO: Further automate this.

		public ControlProfile(ControlProfileLoader loader)
		{
			Attack = new Keybind();
			AttackB = new Keybind();
			ControlLeft = new Keybind();
			ControlRight = new Keybind();
			ControlUp = new Keybind();
			ControlDown = new Keybind();
			Jump = new Keybind();

			Attack.key = GetKeys(loader.Attack);
			AttackB.key = GetKeys(loader.AttackB);
			ControlLeft.key = GetKeys(loader.ControlLeft);
			ControlRight.key = GetKeys(loader.ControlRight);
			ControlUp.key = GetKeys(loader.ControlUp);
			ControlDown.key = GetKeys(loader.ControlDown);
			Jump.key = GetKeys(loader.Jump);
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
			throw new ArgumentException(BadKeybindErrorMessage());
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
