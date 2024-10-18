extends Node2D
var ultrakill_path
var ultrakill_platform
var ultrakill_mods
var config = ConfigFile.new()
var err = config.load("user://frown.ini")

@export var choose_path : FileDialog
@export var launch_button : Button
@export var install_button : Button

func _ready() -> void:
	if err != OK:
		return
		
	for section in config.get_sections():
		ultrakill_path = config.get_value(section, "ultrakill_path")
		ultrakill_platform = config.get_value(section, "ultrakill_platform")
		ultrakill_mods = config.get_value(section, "ultrakill_mods")
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
	var os = "res://" + "linux/"
	var osdir = DirAccess.open(os)
	if osdir:
		osdir.list_dir_begin()
		var file_name = osdir.get_next()
		while file_name != "":
			#DirAccess.copy_absolute(os + file_name, ultrakill_path + "/" + file_name)
			# RUNNING THE TWO BELOW LINES INSTEAD OF THE LINE ABOVE DELETED ALMOST MY ENTIRE PROJECT
			var dir = DirAccess.open("res://linux")
			dir.copy(file_name, ultrakill_path + "/" + file_name)
			
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
