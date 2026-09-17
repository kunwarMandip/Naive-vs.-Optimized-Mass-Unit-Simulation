using Godot;
using System;

public partial class Hud : Node
{
	[Export] public NodePath FpsLabelPath;
	[Export] public NodePath CountLabelPath;
	[Export] public NodePath SpawnerPath;
	
	private Label _fpsLabel;
	private Label _countLabel;
	//private Spawner _spawner;
	
	
	public override void _Ready()
	{
		_fpsLabel = GetNode<Label>(FpsLabelPath);
		_countLabel = GetNode<Label>(CountLabelPath);
		//_spawner = GetNode<Spawner>(SpawnerPath);
	}
	
	public override void _Process(double delta)
	{
		_fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
		//_countLabel.Text = $"Enemies: {_spawner.TotalSpawned}";
	}
	
}
