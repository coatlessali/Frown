extends Node2D
var ultrakill_path
var ultrakill_platform
var ultrakill_mods
var config = ConfigFile.new()
var err = config.load("user://frown.ini")

@export var choose_path : FileDialog
@export var launch_button : Button
@export var install_button : Button
@export var acf_button : Button
@export var steam_warning : AcceptDialog
@export var symlink : Node

func _ready() -> void:
	if err != OK:
		return
		
	for section in config.get_sections():
		ultrakill_path = config.get_value(section, "ultrakill_path")
		ultrakill_platform = config.get_value(section, "ultrakill_platform", "native")
		ultrakill_mods = config.get_value(section, "ultrakill_mods", "yes")
		print(ultrakill_path)
	
	liar_check()
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _on_path_button_pressed() -> void:
	choose_path.visible = true

func _on_file_dialog_dir_selected(dir: String) -> void:
	ultrakill_path = dir
	config.set_value("Main", "ultrakill_path", dir)
	config.save("user://frown.ini")
	liar_check()

func _on_install_button_pressed() -> void:
	# I am happy godot chose to work on
	# important things like d3d12 instead
	# of giving you a way to access files
	# that isn't borderline unusable,
	# meaning I have to run OS.execute()
	# and this launcher is unix specific
	# at some point I will port this to C#
	var files = [] # Files to be copied
	var chmod = [] # Files to be chmod +x'd
	if OS.get_name() == "macOS":
		files = ["BepInEx", "doorstop_config.ini", "libdoorstop.dylib", "run_bepinex.sh", "ULTRAKILL.app", "winhttp.dll", "liar"]
		chmod = ["run_bepinex.sh", "ULTRAKILL.app"]
	elif OS.get_name() == "Linux":
		files = ["BepInEx", "doorstop_config.ini", "liar", "libdoorstop.so", "LinuxPlayer_s.debug", "run_bepinex.sh", "ULTRAKILL_Data", "ULTRAKILL.x86_64", "UnityPlayer_s.debug", "UnityPlayer.so"]
		chmod = ["ULTRAKILL.x86_64", "LinuxPlayer_s.debug", "UnityPlayer_s.debug", "UnityPlayer.so", "run_bepinex.sh"]
	var os_folder = OS.get_name().to_lower()
	for file in files:
		var output = []
		OS.execute("cp", ["-r", os_folder + "/" + file, ultrakill_path], output, true)
	symlink.CreateSymlinks(str(ultrakill_path), OS.get_name())
	#for file in chmod:
		#var output = []
		## file not found??? how???
		#OS.execute("chmod", ["+x", ultrakill_path + "/", file], output, true)
		#print(output)
	
	liar_check()
	
	pass # Replace with function body.

func liar_check():
	var liar_path = ultrakill_path + "/liar"
	if FileAccess.file_exists(liar_path):
		print("liar installed")
		launch_button.visible = true
		install_button.visible = false
	else:
		launch_button.visible = false
		install_button.visible = true

func get_ultrakill_path():
	return "test"

func _on_acf_pressed() -> void:
	var acf_copy_script = load("res://AcfCopy.cs")
	var acf_copy = acf_copy_script.new()
	#var symlink_script = load("res://Symlink.cs")
	#var symlink = symlink_script.new()
	acf_copy.CopyACF()
	steam_warning.visible = false
	pass

func _on_launch_button_pressed() -> void:
	var liar = false
	var dxvk = true
	var modded = true
	var opengl = false
	var renderer_flag = "-force-d3d11"
	var dxvk_dll = "b,n"
	var modded_dll = "b,n"
	if modded:
		modded_dll = "n,b"
	if dxvk:
		dxvk_dll = "n,b"
	if opengl == true:
		dxvk_dll = "b,n"
		renderer_flag = "-force-glcore"
	if liar == false:
		var output = []
		OS.set_environment("WINEDLLOVERRIDES", "d3d11=" + dxvk_dll + ";winhttp=" + modded_dll)
		OS.execute("/usr/local/bin/wine", [ultrakill_path + "/ULTRAKILL.exe", renderer_flag], output, true)
		print(output)
		print(ultrakill_path + "/ULTRAKILL.exe")
		pass # Replace with function body.
	if liar == true:
		# put native launch stuff here
		pass
