using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 300f;
	
	public override void _Ready()
	{
		AddToGroup("player");
		GlobalPosition = GetViewport().GetVisibleRect().Size / 2f;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 input = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = input * Speed;
		MoveAndSlide();
		GD.Print(input);
	}
}
