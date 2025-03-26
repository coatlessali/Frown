using Godot;
using System;

public partial class Description : Label{
	public override void _Ready(){
		Text = "A SmileOS 2.0 compatibility layer for " + OS.GetName() + ".";
	}
}
