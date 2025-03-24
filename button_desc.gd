### Yes. This File is written in GDScript.
### Why? Because is a glorified case switch with some strings really worth whipping out C#?
### Are we making files? Editing files? Extracting ZIP files?
### No? Then we're using GDScript. I don't feel like dealing with setting up signals
### on C# today. Is it that hard? No, not really. Do I feel like it? No, not really.

extends RichTextLabel

### Buttons
func _on_launch_command_mouse_entered() -> void:
	text = "Copy launch command to clipboard for Steam. (TODO)"

func _on_uk_path_button_mouse_entered() -> void:
	text = "Select ULTRAKILL folder."

func _on_install_mouse_entered() -> void:
	text = "Install a compatibility zip."

func _on_uninstall_mouse_entered() -> void:
	text = "Uninstall a currently installed compatibility zip."

func _on_acf_mouse_entered() -> void:
	text = "Create an appmanifest file so Steam will download ULTRAKILL. (macOS only.)"

func _on_launch_mouse_entered() -> void:
	text = "Launch the game with your selected settings. (TODO)"

func _on_mods_mouse_entered() -> void:
	text = "Enable mod loading. (TODO)"

### Switches
func _on_api_mouse_entered() -> void:
	text = "Switch between OpenGL and Vulkan. (Linux Only) (TODO)"

func _on_wayland_mouse_entered() -> void:
	text = "Enable native Wayland support. (Linux Only) (TODO)"

func _on_check_button_mouse_entered() -> void:
	text = "Enable Mangohud. (Linux Only) (TODO)"

func _on_log_mouse_entered() -> void:
	text = "Show log and copy to clipboard."
