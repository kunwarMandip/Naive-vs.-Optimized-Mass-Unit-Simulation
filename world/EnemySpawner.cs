using Godot;
using System.Collections.Generic;

public partial class EnemySpawner : Node
{
	[Export] public PackedScene EnemyScene;
	[Export] public NodePath EnemyContainerPath;
	[Export] public NodePath SpawnAreasPath;
	[Export] public float SpawnInterval = 0.5f;
	[Export] public int InitialBatchSize = 5;
	[Export] public int BatchGrowth = 5;
	
	private Node _enemyContainer;
	private readonly List<Area2D> _spawnAreas = new();
	private float _timer;
	private int _batchSize;
	public int TotalSpawned { get; private set; }
	
	public override void _Ready()
	{
		_enemyContainer = GetNode<Node2D>(EnemyContainerPath);
		_batchSize = InitialBatchSize;
		
		var areasNode = GetNode(SpawnAreasPath);
		foreach (Node child in areasNode.GetChildren())
		{
			if (child is Area2D area)
				_spawnAreas.Add(area);
		}
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
	   for (int i = 0; i < count; i++)
		{
			var area = _spawnAreas[GD.RandRange(0, _spawnAreas.Count - 1)];
			var enemy = EnemyScene.Instantiate<Node2D>();
			enemy.GlobalPosition = RandomPointInArea(area);
			_enemyContainer.AddChild(enemy);
			TotalSpawned++;
		}
	}
	
	
	private Vector2 RandomPointInArea(Area2D area)
	{
		var shape = area.GetNode<CollisionShape2D>("CollisionShape2D");
		var rectShape = (RectangleShape2D)shape.Shape;
		Vector2 extents = rectShape.Size / 2f;
		Vector2 center = area.GlobalPosition + shape.Position;

		float x = (float)GD.RandRange(center.X - extents.X, center.X + extents.X);
		float y = (float)GD.RandRange(center.Y - extents.Y, center.Y + extents.Y);
		return new Vector2(x, y);
	}
}
