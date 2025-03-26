using Godot;
using System;

public partial class LaunchCommand : Button{
	public override void _Ready(){
		if (OS.GetName() == "macOS")
			Disabled = true;
	}
}
