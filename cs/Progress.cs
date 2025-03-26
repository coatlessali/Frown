using Godot;
using System;

public partial class Progress : RichTextLabel
{
	[Export]
	public Button _log;
	
	public override void _Ready()
	{
		_log.Pressed += Toggle;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void Toggle()
	{
		var clipboard = DisplayServer.ClipboardGet();
		DisplayServer.ClipboardSet(Text);
		
		/*if (Visible){
			Hide();
		}
		else{
			Show();
		}*/
	}
}
