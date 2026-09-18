using Godot;
using System;

public partial class OptimizedHud : Node
{
	[Export] public NodePath FpsLabelPath;
	[Export] public NodePath CountLabelPath;
	[Export] public NodePath EnemyManagerPath;

	private Label _fpsLabel;
	private Label _countLabel;
	private EnemyManager _enemyManager;

	public override void _Ready()
	{
		_fpsLabel = GetNode<Label>(FpsLabelPath);
		_countLabel = GetNode<Label>(CountLabelPath);
		_enemyManager = GetNode<EnemyManager>(EnemyManagerPath);
	}

	public override void _Process(double delta)
	{
		_fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
		_countLabel.Text = $"Enemies: {_enemyManager.TotalSpawned}";
	}
}
