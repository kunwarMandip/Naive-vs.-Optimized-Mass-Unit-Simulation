using Godot;
using System;

public partial class EnemySpawner : Node
{
	[Export] public PackedScene EnemyScene;
	[Export] public NodePath EnemyContainerPath;
	[Export] public float SpawnInterval = 0.5f;
	[Export] public int InitialBatchSize = 5;
	[Export] public int BatchGrowth = 5;
	
	private Node _enemyContainer;
	private float _timer;
	private int _batchSize;
	public int TotalSpawned { get; private set; }

	public override void _Ready()
	{
		_enemyContainer = GetNode(EnemyContainerPath);
		_batchSize = InitialBatchSize;
	}
	
	public override void _Process(double delta)
	{
		_timer += (float)delta;
		if (_timer >= SpawnInterval)
		{
			_timer = 0f;
			SpawnBatch(_batchSize);
			_batchSize += BatchGrowth;
		}
	}
	
	private void SpawnBatch(int count)
	{
		var viewportSize = GetViewport().GetVisibleRect().Size;
		for (int i = 0; i < count; i++)
		{
			var enemy = EnemyScene.Instantiate<CharacterBody2D>();
			enemy.Position = new Vector2(
				(float)GD.RandRange(0, viewportSize.X),
				(float)GD.RandRange(0, viewportSize.Y));
			_enemyContainer.AddChild(enemy);
			TotalSpawned++;
		}
	}
	
}
