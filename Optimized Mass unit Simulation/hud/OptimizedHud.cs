using Godot;
using System;

public partial class OptimizedHud : Node
{
	[Export] public NodePath FpsLabelPath;
	[Export] public NodePath CountLabelPath;
	[Export] public NodePath HealthLabelPath;
	[Export] public NodePath EnemyManagerPath;
	[Export] public NodePath PlayerPath;

	private Label _fpsLabel;
	private Label _countLabel;
	private Label _healthLabel;
	private EnemyManager _enemyManager;
	private Player _player;

	public override void _Ready()
	{
		_fpsLabel = GetNode<Label>(FpsLabelPath);
		_countLabel = GetNode<Label>(CountLabelPath);
		_healthLabel = GetNode<Label>(HealthLabelPath);
		_enemyManager = GetNode<EnemyManager>(EnemyManagerPath);
		_player = GetNode<Player>(PlayerPath);
	}

	public override void _Process(double delta)
	{
		_fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
		_countLabel.Text = $"Enemies: {_enemyManager.TotalSpawned}";
		_healthLabel.Text = $"Health: {_player.Health}";
	}
}
