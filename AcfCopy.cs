using Godot;
using System;
using System.IO;

public partial class AcfCopy : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void CopyACF()
	{
		string homeDir = OS.GetEnvironment("HOME");
		string steamapps = "Library/Application Support/Steam/steamapps/";
		string fullPath = Path.Combine(homeDir, steamapps);
		GD.Print(fullPath);
		if(Directory.Exists(fullPath))
		{
			GD.Print("success!");
			File.Copy("appmanifest_1229490.acf", Path.Combine(fullPath, "appmanifest_1229490.acf"), true);
		}
	}
}
