using Godot;
using System;

public partial class Enemy : ColorRect
{
	[Export] public float Speed = 300f;
	private Control _player;
	
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Control;
	}
	
	public override void _Process(double delta)
	{
		if (_player == null) return;
		Vector2 direction = (_player.Position - Position).Normalized();
		Position += direction * Speed * (float)delta;
	}
}
