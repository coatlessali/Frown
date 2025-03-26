using Godot;
using System;

public partial class API : CheckButton{
	public override void _Ready(){
		if (OS.GetName() == "macOS")
			Disabled = true;
	}
}
