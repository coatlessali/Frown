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
	
	public void CreateSymlinks(string ukpath, string plat)
	{
		GD.Print(plat);
		if (plat == "macOS"){
			// GD.Print(ukpath);
			string uk_data = Path.Combine(ukpath, "ULTRAKILL_Data");
			string uk_app = Path.Combine(ukpath, "ULTRAKILL.app");
			foreach (string dirs in Directory.GetDirectories(uk_data))
			{
				//GD.Print("dirs");
				//GD.Print(dirs);
				string dirName = new DirectoryInfo(dirs).Name;
				//GD.Print("dirName");
				//GD.Print(dirName);
				CopyFilesRecursively(dirs, Path.Combine(uk_app, "Contents", "Resources", "Data", dirName));
				//Directory.CreateSymbolicLink(Path.Combine(uk_app, "Contents", "Resources", "Data", dirName), dirs);
			}
			foreach (string files in Directory.GetFiles(uk_data))
			{
				GD.Print("files:");
				GD.Print(files);
				string fileName = new FileInfo(files).Name;
				GD.Print("fileName:");
				GD.Print(fileName);
				GD.Print("Destination:");
				GD.Print(Path.Combine(uk_app, "Contents", "Resources", "Data", fileName));
				File.Copy(files, Path.Combine(uk_app, "Contents", "Resources", "Data", fileName), true);
			}
			try{File.CreateSymbolicLink(Path.Combine(uk_app, "Saves"), Path.Combine(ukpath, "Saves"));}catch{}
			try{File.CreateSymbolicLink(Path.Combine(uk_app, "Preferences"), Path.Combine(ukpath, "Preferences"));}catch{}
			try{File.CreateSymbolicLink(Path.Combine(uk_app, "Cybergrind"), Path.Combine(ukpath, "CyberGrind"));}catch{}
		}
	}
	private static void CopyFilesRecursively(string sourcePath, string targetPath)
	{
		//Now Create all of the directories
		foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
		{
			Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
		}

		//Copy all the files & Replaces any files with the same name
		foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
		{
			File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
		}
	}
}
