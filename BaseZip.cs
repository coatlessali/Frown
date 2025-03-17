using Godot;
using System;

public partial class BaseZip : FileDialog
{
	[Export]
	public Button _ins;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ins.Pressed += Thingy;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void Thingy()
	{
		Show();
	}
}
