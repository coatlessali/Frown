### Yes. This File is written in GDScript.
### Why? Because is a glorified case switch with some strings really worth whipping out C#?
### Are we making files? Editing files? Extracting ZIP files?
### No? Then we're using GDScript. I don't feel like dealing with setting up signals
### on C# today. Is it that hard? No, not really. Do I feel like it? No, not really.

extends RichTextLabel

### Buttons
func _on_launch_command_mouse_entered() -> void:
	text = "Copy launch command to clipboard for Steam. Save your settings before clicking this.\n\n(Linux only.)"

func _on_uk_path_button_mouse_entered() -> void:
	text = "Select ULTRAKILL folder."

func _on_install_mouse_entered() -> void:
	text = "Install a compatibility zip."

func _on_uninstall_mouse_entered() -> void:
	text = "Uninstall a currently installed compatibility zip."

func _on_acf_mouse_entered() -> void:
	text = "Create an appmanifest file so Steam will download ULTRAKILL.\n\n(macOS only.)"

func _on_launch_mouse_entered() -> void:
	text = "Launch the game with your selected settings.\n\n(TODO)"

func _on_mods_mouse_entered() -> void:
	text = "Enable mod loading.\n\n(Bepinex must be installed manually.)"

### Switches
func _on_api_mouse_entered() -> void:
	text = "Use Vulkan instead of OpenGL.\n\n(Linux Only)"

func _on_wayland_mouse_entered() -> void:
	text = "Enable native Wayland support.\n\n(Linux Only)"

func _on_check_button_mouse_entered() -> void:
	text = "Enable Mangohud.\n\n(Linux Only)"

func _on_log_mouse_entered() -> void:
	text = "Copy log to clipboard."

func _on_save_mouse_entered() -> void:
	text = "Save settings to frown.ini."


func _on_delete_backup_mouse_entered() -> void:
	text = "Delete backup. (For updates and whatnot.)"
