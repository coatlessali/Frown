using Godot;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using IniParser;
using IniParser.Model;
// using Microsoft.VisualBasic.FileIO;

public partial class Frown : Node2D
{
	[Export]
	public Label _progress;
	[Export]
	public Button _install;
	[Export]
	public Button _uninstall;
	[Export]
	public Label _api;
	[Export]
	public FileDialog _ukPath;
	[Export]
	public FileDialog _baseZip;
	
	enum RenderAPI : byte
	{
		OpenGL = 0,
		Vulkan = 1,
		D3D11 = 2,
		D3D12 = 3
	}
	
	string frownConfig = ProjectSettings.GlobalizePath("user://frown.ini");
	string backup = ProjectSettings.GlobalizePath("user://Managed");
	string ukPath;
	string launchCommand;
	string ukVersion = "Unknown";
	string frownVersion = "None";
	bool wayland = false;
	bool modStatus;
	byte backend = (byte) RenderAPI.OpenGL;
	// Called when the node enters the scene tree for the first time.
	
	public void GetUKInfo()
	{
		if (ukPath == "."){
			return;
		}
		string levelZero = Path.Combine(ukPath, "ULTRAKILL_Data", "level0");
		byte[] levelZeroVer = new byte[12];
		using (FileStream fs = File.OpenRead(levelZero))
		{
			fs.Seek(48, SeekOrigin.Begin);
			for (byte i = 0; i < 12; i++)
			{
				levelZeroVer[i] = (byte) fs.ReadByte();
			}
		}
		ukVersion = System.Text.Encoding.Default.GetString(levelZeroVer).Trim();
		ukVersion = Regex.Replace(ukVersion, @"[^\u0020-\u007E]", string.Empty);
		GD.Print(ukVersion);
	}
	
	public void Backup()
	{
		string Managed = Path.Combine(ukPath, "ULTRAKILL_Data", "Managed");
		if (!Directory.Exists(backup))
		{
			Directory.CreateDirectory(backup);
			// _progress.Text = "Created backup directory.";
			_progress.Set("Buffer", "Created backup directory.");
			CopyFilesRecursively(Managed, backup);
			// _progress.Text = "Created backup.";
			_progress.Set("Buffer", "Created backup.");
		}
		else
		{
			// _progress.Text = "Skipped backup creation.";
			_progress.Set("Buffer", "Skipped backup creation.");
		}
	}
	
	
	public void DeleteBackup()
	{
		GD.Print("todo");
	}
	
	public void Restore()
	{
		if (OS.GetName() == "Linux")
		{
			string VersionPath = Path.Combine(ukPath, "version.txt");
			string ukExe = Path.Combine(ukPath, "ULTRAKILL.x86_64");
			string pluginsPath = Path.Combine(ukPath, "ULTRAKILL_Data", "Plugins", "x86_64");
			string UnityPlayer = Path.Combine(ukPath, "UnityPlayer.so");
			string Managed = Path.Combine(ukPath, "ULTRAKILL_Data", "Managed");
			string MonoBleedingEdge = Path.Combine(ukPath, "ULTRAKILL_Data", "MonoBleedingEdge");
			if (!Directory.Exists(backup))
			{
				GD.Print("backup doesn't exist");
				return;
			}
			if (!File.Exists(ukExe))
			{
				GD.Print("FROWN not installed");
				return;
			}
		
			Directory.Delete(Managed, true);
			// _progress.Text = "Deleted Managed...";
			_progress.Set("Buffer", "Deleted Managed...");
			Directory.Delete(MonoBleedingEdge, true);
			// _progress.Text = "Deleted MonoBleedingEdge...";
			_progress.Set("Buffer", "Deleted MonoBleedingEdge...");
			
			string[] files =  { VersionPath, ukExe, 
								UnityPlayer, "discord_game_sdk.bundle", 
								"discord_game_sdk.dll.lib", "discord_game_sdk.dylib", 
								"discord_game_sdk.so", "steam_api64.dylib", 
								"steam_api64.so" };
			foreach (string file in files){
				try{
					File.Delete(file);
					// _progress.Text = "Deleted " + file;
					_progress.Set("Buffer", "Deleted " + file + "...");
				}
				catch(Exception e){
					// _progress.Text = file + " does not exist, skipping";
					_progress.Set("Buffer", file + " does not exist, skipping...");
				}
			
			}
			Directory.CreateDirectory(Managed);
			// _progress.Text = "Recreated Managed...";
			_progress.Set("Buffer", "Recreated Managed...");
			CopyFilesRecursively(backup, Managed);
			// _progress.Text = "Restored Managed...";
			_progress.Set("Buffer", "Restored Managed...");
		}	
		if (OS.GetName() != "macOS")
		  return;
		string ukApp = Path.Combine(ukPath, "ULTRAKILL.app");
		try{
			Directory.Delete(ukApp, true);
			// _progress.Text = "Deleted ULTRAKILL.app";
			_progress.Set("Buffer", "Deleted ULTRAKILL.app...");
		}
		catch (Exception e){
			GD.Print(e.ToString());
		}
	}
	
	public override void _Ready()
	{
		// _progress
		// _install
		// _uninstall
		// _api
		_baseZip.FileSelected += Install;
		_uninstall.Pressed += Restore;
		_ukPath.DirSelected += GetUKPath;
		
		if(!File.Exists(frownConfig)){
			string defaultConfig = "[main]\nfrown = false\nukPath = .\nwayland = false\nbepinex = false\nrenderer = 0";
			File.WriteAllText(frownConfig, defaultConfig);
			// _progress.Text = "Created frown.ini";
			_progress.Set("Buffer", "Created frown.ini...");
			_ukPath.Show();
		}
		IniData data = new FileIniDataParser().ReadFile(frownConfig);
		ukPath = data["main"]["ukPath"];
		wayland = bool.Parse(data["main"]["wayland"]);
		backend = byte.Parse(data["main"]["renderer"]);
		modStatus = bool.Parse(data["main"]["bepinex"]);
		
		// _progress.Text = "Loaded frown.ini";
		_progress.Set("Buffer", "Loaded frown.ini...");
		
		GetUKInfo();
		// _progress.Text = "Parsed ULTRAKILL install info";
		_progress.Set("Buffer", "Parsed ULTRAKILL install info...");
		
		GD.Print(ukPath);
		GD.Print(wayland);
		GD.Print(backend);
		GD.Print(modStatus);
		GD.Print(ukVersion);
	}

	public void Install(string baseZip)
	{
		// GD.Print(baseZip);
		string verStr = "Unknown";
		using (ZipArchive zip = ZipFile.Open(baseZip, ZipArchiveMode.Read)){
			foreach (ZipArchiveEntry entry in zip.Entries){
				if (entry.Name == "version.txt"){
					try{
						Stream stream = entry.Open();
						var mem = new MemoryStream();
						stream.CopyTo(mem);
						byte[] ver = mem.ToArray();
						verStr = System.Text.Encoding.Default.GetString(ver).Trim();
						GD.Print(verStr);
					}
					catch(Exception e){
						GD.Print("Failed to get version.txt, defaulting to Unknown");
						GD.Print(e.ToString());
					}
				}
			}
		}
		
		if (!verStr.Equals(ukVersion)){
			GD.Print("versions do not match!");
			GD.Print("verStr " + verStr);
			GD.Print(verStr.Length);
			GD.Print("ukVersion " + ukVersion);
			GD.Print(ukVersion.Length);
			// return;
		}
		ZipFile.ExtractToDirectory(baseZip, ukPath, true);
		// _progress.Text = "Installed FROWN for Linux";
		_progress.Set("Buffer", "Installed FROWN for Linux.");
		
		if (OS.GetName() == "macOS")
		{
			string ukApp = Path.GetFullPath(Path.Combine(ukPath, "ULTRAKILL.app"));
			string saves = Path.GetFullPath(Path.Combine(ukPath, "Saves"));
			string preferences = Path.GetFullPath(Path.Combine(ukPath, "Preferences"));
			string cybergrind = Path.GetFullPath(Path.Combine(ukPath, "Cybergrind"));
			string savesLink = Path.GetFullPath(Path.Combine(ukApp, "Saves"));
			string preferencesLink = Path.GetFullPath(Path.Combine(ukApp, "Preferences"));
			string cybergrindLink = Path.GetFullPath(Path.Combine(ukApp, "CyberGrind"));
		
			if (!Directory.Exists(ukApp))
			{
				GD.Print("ukApp not found");
				// _progress.Text = "couldn't find ultrakill.app...";
				_progress.Set("Buffer", "Couldn't find ULTRAKILL.app...");
				return;
			}
			
			try{
				File.CreateSymbolicLink(savesLink, saves);
				// _progress.Text = "Linked Saves...";
				_progress.Set("Buffer", "Linked Saves...");
				File.CreateSymbolicLink(preferencesLink, preferences);
				// _progress.Text = "Linked Preferences...";
				_progress.Set("Buffer", "Linked Preferences...");
				File.CreateSymbolicLink(cybergrindLink, cybergrind);
				// _progress.Text = "Linked CyberGrind...";
				_progress.Set("Buffer", "Linked CyberGrind...");
			}
			catch (Exception e)
			{
				GD.Print(e.ToString());
			}
			string ukData = Path.Combine(ukPath, "ULTRAKILL_Data");
			string ukAppData = Path.Combine(ukApp, "Contents", "Resources", "Data");
			CopyFilesRecursively(ukData, ukAppData);
			// _progress.Text = "Copied ULTRAKILL data to ULTRAKILL.app...";
			_progress.Set("Buffer", "Copied ULTRAKILL data to ULTRAKILL.app...");
			string oldManaged = Path.Combine(ukAppData, "Managed");
			string newManaged = Path.Combine(ukAppData, "NewManaged");
			CopyFilesRecursively(newManaged, oldManaged);
			// _progress.Text = "Finalized installation of FROWN for macOS.";
			_progress.Set("Buffer", "Finalized installation of FROWN for macOS.");
		}
	}

	public void GetUKPath(string dir)
	{
		GD.Print(dir);
		ukPath = Path.GetFullPath(dir);
		IniData data = new FileIniDataParser().ReadFile(frownConfig);
		data["main"]["ukPath"] = Path.GetFullPath(dir);
		new FileIniDataParser().WriteFile(frownConfig, data);
		// _progress.Text = "Saved changes.";
		_progress.Set("Buffer", "Saved changes.");
		
		Backup();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// https://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp
	private static void CopyFilesRecursively(string sourcePath, string targetPath){
		//Now Create all of the directories
		foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)){
			Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
		}
		//Copy all the files & Replaces any files with the same name
		foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories)){
			File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
		}
	}
}
