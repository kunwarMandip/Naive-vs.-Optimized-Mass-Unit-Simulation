using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed = 300f;
	private Node2D _player;
	
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Node2D;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_player == null) return;
		Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = direction * Speed;
		MoveAndSlide();
	}
}
