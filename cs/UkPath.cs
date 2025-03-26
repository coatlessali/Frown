using Godot;
using System;

public partial class UkPath : FileDialog
{
	[Export]
	public Button _ukb;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ukb.Pressed += Thingy;
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
