using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
{
	public enum EditorMenuItemType
	{
		Button, // A button that performs an action when clicked
		Hoverable, // An element that displays other elements when hovered over
		Tickbox, // A box that can be ticked or unticked
		ToggleButton, // A button that can be depressed and then unpressed
		EditableText, // An editable text field
		Dropdown, // A field with multiple selectable options revealed by selecting the field
		VerticalList, // A vertical scrollable list of items
		CompactList, // A vertical scrollable list of items where multiple items can occupy the same horizontal row
		Selectable, // An item that can be selected but not un-selected. Only one can be selected at a time in a given context
		AngleKnob, // A knob specifying an angle
		Spinner // A numerical value increased or decreased in increments
	}
}
