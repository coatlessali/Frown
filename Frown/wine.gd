extends CheckButton
var wine = "WINE"

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	if OS.get_name() == "Windows":
		visible = false
	elif OS.get_name() == "Linux":
		wine = "WINE"
	elif OS.get_name() == "MacOS":
		wine = "Whisky"
	text = "Force " + wine


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
