// Acf.cs
// This file is basically finished. I see no reason this should ever need to be updated.

using Godot;
using System;
using System.IO;

public partial class Acf : Button
{	
	[Export]
	public Label _progress;
	
	string home;
	string steamapps;
	string acf;
	string contents;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		// This button shouldn't exist on anything but macOS
		if (OS.GetName() != "macOS"){
			Disabled = true;
			return;
		}
		
		// get home dir
		home = System.Environment.GetEnvironmentVariable("HOME");
		// get steamapps location
		steamapps = Path.Combine(home, "Library/Application Support/Steam/steamapps/");
		// acf location
		acf = Path.Combine(steamapps, "appmanifest_1229490.acf");
		// can I ever be forgiven for what I'm about to do
		contents = "\"AppState\"\n    {\n      \"AppID\"  \"1229490\"\n      \"Universe\" \"1\"\n      \"installdir\" \"ULTRAKILL\"\n      \"StateFlags\" \"1026\"\n    }";
		// GD.Print(acf);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_pressed()
	{
		if (!Directory.Exists(steamapps)){
			GD.Print("Steam install not found.");
			_progress.Text = "Steam install not found.";
			return;
		}
		if (File.Exists(acf)){
			GD.Print("acf already found, deleting");
			_progress.Text = "Deleting acf file...";
			try{File.Delete(acf);}
			catch(Exception e){GD.Print(e.ToString());_progress.Text="Failed to delete ACF. Check console...";}
		}
		try{
			File.WriteAllText(acf, contents);
			GD.Print("wrote acf file");
			_progress.Text = "Wrote ACF file...";
		}
		catch(Exception e){
			_progress.Text = "Failed to write ACF file, check console...";
			GD.Print(e.ToString());
		}
	}
}
