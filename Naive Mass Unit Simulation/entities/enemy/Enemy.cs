using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed = 300f;
	private CharacterBody2D _player;
	
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_player == null){
			GD.Print("Enemy Null");
			return;
		} 
		Velocity = (_player.GlobalPosition - GlobalPosition).Normalized() * Speed;
		MoveAndSlide();
	}
}
