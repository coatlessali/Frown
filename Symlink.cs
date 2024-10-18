using Godot;
using System;
using System.IO;

public partial class Symlink : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void CreateSymlinks(string ukpath)
	{
		GD.Print(ukpath);
	}
}
