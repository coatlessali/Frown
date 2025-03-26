using Godot;
using System;

public partial class MangoHud : CheckButton{
	public override void _Ready(){
		if (OS.GetName() == "macOS")
			Disabled = true;
	}
}
