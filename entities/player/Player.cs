using Godot;
using System;

public partial class Player : ColorRect
{
	[Export] public float Speed = 300f;

	public override void _Ready() => AddToGroup("player");

	public override void _Process(double delta)
	{
		Vector2 input = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Position += input * Speed * (float)delta;
	}
}
