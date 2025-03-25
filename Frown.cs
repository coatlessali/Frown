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
	// Labels
	[Export]
	public RichTextLabel _progress;
	// Buttons
	[Export]
	public Button _install;
	[Export]
	public Button _uninstall;
	[Export]
	public Button _command;
	[Export]
	public Button _launch;
	[Export]
	public Button _save;
	// Switches
	[Export]
	public CheckButton _mods;
	[Export]
	public CheckButton _api;
	[Export]
	public CheckButton _wayland;
	[Export]
	public CheckButton _mangohud;
	// FileDialogs
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
	// string launchCommand; // convert to string[] later? might make launch button easier
	string ukVersion = "Unknown";
	string frownVersion = "None";
	bool wayland = false;
	bool mangohud = false;
	bool modStatus = false;
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
		// GD.Print(ukVersion);
	}
	
	public void GetAPI()
	{
		if (_api.ToggleMode){
			backend = (byte) RenderAPI.Vulkan;
		}
		else{
			backend = (byte) RenderAPI.OpenGL;
		}
	}
	
	public void Backup()
	{
		string Managed = Path.Combine(ukPath, "ULTRAKILL_Data", "Managed");
		if (!Directory.Exists(backup))
		{
			Directory.CreateDirectory(backup);
			_progress.Text += "[I] Created backup directory.\n";
			// _progress.Set("Buffer", "Created backup directory.");
			CopyFilesRecursively(Managed, backup);
			_progress.Text += "[I] Created backup.\n";
			// _progress.Set("Buffer", "Created backup.");
		}
		else
		{
			_progress.Text += "[I] Skipped backup creation.\n";
			// _progress.Set("Buffer", "Skipped backup creation.");
		}
	}
	
	
	public void DeleteBackup()
	{
		// GD.Print("todo");
		_progress.Text += "[W] Not yet implemented.\n";
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
				_progress.Text += "[E] Backup not found.\n";
				// GD.Print("backup doesn't exist");
				return;
			}
			if (!File.Exists(ukExe))
			{
				_progress.Text += "[E] FROWN not installed.\n";
				// GD.Print("FROWN not installed");
				return;
			}
		
			Directory.Delete(Managed, true);
			_progress.Text += "[I] Deleted Managed...\n";
			Directory.Delete(MonoBleedingEdge, true);
			_progress.Text += "[I] Deleted MonoBleedingEdge...\n";
			
			string[] files =  { VersionPath, ukExe, 
								UnityPlayer, "discord_game_sdk.bundle", 
								"discord_game_sdk.dll.lib", "discord_game_sdk.dylib", 
								"discord_game_sdk.so", "steam_api64.dylib", 
								"steam_api64.so" };
			foreach (string file in files){
				try{
					File.Delete(file);
					_progress.Text += "[I] Deleted " + file + "\n";
					// _progress.Set("Buffer", "Deleted " + file + "...");
				}
				catch(Exception e){
					_progress.Text += "[I] " + file + " does not exist, skipping\n";
					// _progress.Set("Buffer", file + " does not exist, skipping...");
				}
			
			}
			Directory.CreateDirectory(Managed);
			_progress.Text += "[I] Recreated Managed...\n";
			// _progress.Set("Buffer", "Recreated Managed...");
			CopyFilesRecursively(backup, Managed);
			_progress.Text += "[I] Restored Managed...\n";
			// _progress.Set("Buffer", "Restored Managed...");
		}	
		if (OS.GetName() != "macOS")
		  return;
		string ukApp = Path.Combine(ukPath, "ULTRAKILL.app");
		try{
			Directory.Delete(ukApp, true);
			_progress.Text += "[I] Deleted ULTRAKILL.app\n";
			// _progress.Set("Buffer", "Deleted ULTRAKILL.app...");
		}
		catch (Exception e){
			GD.Print(e.ToString());
		}
	}
	
	public override void _Ready()
	{
		
		_baseZip.FileSelected += Install;
		_uninstall.Pressed += Restore;
		_ukPath.DirSelected += GetUKPath;
		_command.Pressed += Command;
		_launch.Pressed += Launch;
		_save.Pressed += SaveSettings;
		
		if(!File.Exists(frownConfig)){
			string defaultConfig = "[main]\nfrown = false\nukPath = .\nwayland = false\nbepinex = false\nrenderer = 0\nmangohud = false";
			File.WriteAllText(frownConfig, defaultConfig);
			_progress.Text += "[I] Created frown.ini\n";
			// // _progress.Set("Buffer", "Created frown.ini...");
			_ukPath.Show();
		}
		IniData data = new FileIniDataParser().ReadFile(frownConfig);
		ukPath = data["main"]["ukPath"];
		wayland = bool.Parse(data["main"]["wayland"]);
		backend = byte.Parse(data["main"]["renderer"]);
		mangohud = bool.Parse(data["main"]["mangohud"]);
		modStatus = bool.Parse(data["main"]["bepinex"]);
		
		_progress.Text += "[I] Loaded frown.ini\n";
		// // _progress.Set("Buffer", "Loaded frown.ini...");
		
		GetUKInfo();
		_progress.Text += "[I] Parsed ULTRAKILL install info\n";
		// _progress.Set("Buffer", "Parsed ULTRAKILL install info...");
		
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
						_progress.Text += "[W] Failed to get version.txt...\n";
						GD.Print("Failed to get version.txt, defaulting to Unknown");
						GD.Print(e.ToString());
					}
				}
			}
		}
		
		if (!verStr.Equals(ukVersion)){
			GD.Print("versions do not match!");
			_progress.Text += "[W] Versions do not match!\n";
			_progress.Text += "[W] " + verStr + " != " + ukVersion + "\n";
			GD.Print("verStr " + verStr);
			GD.Print(verStr.Length);
			GD.Print("ukVersion " + ukVersion);
			GD.Print(ukVersion.Length);
			// return;
		}
		ZipFile.ExtractToDirectory(baseZip, ukPath, true);
		_progress.Text += "[I] Installed FROWN for Linux\n";
		
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
				_progress.Text += "[E] couldn't find ultrakill.app...\n";
				return;
			}
			
			try{
				File.CreateSymbolicLink(savesLink, saves);
				_progress.Text += "[I] Linked Saves...\n";
				// _progress.Set("Buffer", "Linked Saves...");
				File.CreateSymbolicLink(preferencesLink, preferences);
				_progress.Text += "[I] Linked Preferences...\n";
				// _progress.Set("Buffer", "Linked Preferences...");
				File.CreateSymbolicLink(cybergrindLink, cybergrind);
				_progress.Text += "[I] Linked CyberGrind...\n";
				// _progress.Set("Buffer", "Linked CyberGrind...");
			}
			catch (Exception e)
			{
				_progress.Text += "[E] Failed to create symlinks, check console...\n";
				GD.Print(e.ToString());
			}
			string ukData = Path.Combine(ukPath, "ULTRAKILL_Data");
			string ukAppData = Path.Combine(ukApp, "Contents", "Resources", "Data");
			CopyFilesRecursively(ukData, ukAppData);
			_progress.Text += "[I] Copied ULTRAKILL data to ULTRAKILL.app...\n";
			// _progress.Set("Buffer", "Copied ULTRAKILL data to ULTRAKILL.app...");
			string oldManaged = Path.Combine(ukAppData, "Managed");
			string newManaged = Path.Combine(ukAppData, "NewManaged");
			CopyFilesRecursively(newManaged, oldManaged);
			_progress.Text += "[I] Finalized installation of FROWN for macOS.\n";
			// _progress.Set("Buffer", "Finalized installation of FROWN for macOS.");
		}
	}

	public void GetUKPath(string dir)
	{
		// GD.Print(dir);
		// _progress.Text += "ULTRAKILL path: " + dir;
		ukPath = Path.GetFullPath(dir);
		IniData data = new FileIniDataParser().ReadFile(frownConfig);
		data["main"]["ukPath"] = Path.GetFullPath(dir);
		new FileIniDataParser().WriteFile(frownConfig, data);
		_progress.Text += "[I] Saved changes.\n";
		// _progress.Set("Buffer", "Saved changes.");
		
		Backup();
	}

	public void Command()
	{
		bool mangohud = true; // TODO: Implement an actual variable, this is just here to avoid a compiler error for now
		string sdl;
		string hud;
		string launch;
		string force;
		string filler = "; echo %command%";
		
		// Wayland
		if (wayland)
			sdl = "SDL_VIDEODRIVER=wayland";
		else
			sdl = "SDL_VIDEODRIVER=x11";
			
		// Mangohud
		if (mangohud){
			if (backend == (byte) RenderAPI.Vulkan)
				hud = "MANGOHUD=1";
			else
				hud = "mangohud --dlsym";
		} /* When I wrote the above, I googled how to do an 
			else if statement and Google told me that C# doesn't
			have else if statements. For some reason, I actually
			believed this and wrote the above. Forgive me. */
		else
			hud = "";
			
		// Launch command
		if (modStatus)
			launch = "./run_bepinex.sh ULTRAKILL.x86_64";
		else
			launch = "./ULTRAKILL.x86_64";
			
		// Render API
		if (backend == (byte) RenderAPI.Vulkan)
			force = "-force-vulkan";
		else
			force = "-force-glcore";
		
		string opts = $"{sdl} {hud} {launch} {force}{filler}";
		// var clipboard = DisplayServer.ClipboardGet();
		DisplayServer.ClipboardSet(opts);
		_progress.Text += $"[I] Copied launch command: {opts}\n";
		
	}
	
	public void SaveSettings()
	{
		_progress.Text += "[W] Not yet implemented.\n";
		if (_mods.ButtonPressed)
			modStatus = true;
		else
			modStatus = false;
		if (_api.ButtonPressed)
			backend = (byte) RenderAPI.Vulkan;
		else
			backend = (byte) RenderAPI.OpenGL;
		if (_wayland.ButtonPressed)
			wayland = true;
		else
			wayland = false;
		if (_mangohud.ButtonPressed)
			mangohud = true;
		else
			mangohud = false;
		IniData data = new FileIniDataParser().ReadFile(frownConfig);
		data["main"]["wayland"] = wayland.ToString();
		data["main"]["bepinex"] = modStatus.ToString();
		data["main"]["renderer"] = backend.ToString();
		data["main"]["mangohud"] = mangohud.ToString();
		new FileIniDataParser().WriteFile(frownConfig, data);
		_progress.Text += "[I] Saved changes.\n";
	}
	
	public void Launch()
	{
		// GD.Print("todo: launch game");
		_progress.Text += "[W] Not yet implemented.\n";
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
