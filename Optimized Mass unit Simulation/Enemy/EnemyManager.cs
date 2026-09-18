using Godot;
using System.Collections.Generic;

public partial class EnemyManager : Node
{
	
	[Export] public NodePath MultiMeshInstancePath;
	[Export] public NodePath SpawnAreasPath;
	[Export] public NodePath PlayerPath;
	
	[Export] public float Speed = 150f;
	[Export] public float SpawnInterval = 0.5f;
	[Export] public int InitialBatchSize = 50;
	[Export] public int BatchGrowth = 50;
	[Export] public int MaxEnemySpawnable = 50000;
	[Export] public int InstanceSize = 16;
	
	private MultiMeshInstance2D _multiMeshInstance;
	private MultiMesh _multiMesh;
	private Node2D _player;
	private readonly List<Area2D> _spawnAreas = new();
	
	private Vector2[] _positions;
	private int _count;
	private float _timer;
	private int _batchSize;
	
	public int TotalSpawned => _count;
	
	public override void _Ready()
	{
		_multiMeshInstance = GetNode<MultiMeshInstance2D>(MultiMeshInstancePath);
		_player = GetNode<Node2D>(PlayerPath);
		_batchSize = InitialBatchSize;
		_positions = new Vector2[MaxEnemySpawnable];

		var areasNode = GetNode(SpawnAreasPath);
		foreach (Node child in areasNode.GetChildren())
		{
			if (child is Area2D area)
				_spawnAreas.Add(area);
		}
		
		SetupMultiMesh();
	}
	
	private void SetupMultiMesh()
	{
		var image = Image.CreateEmpty(InstanceSize, InstanceSize, false, Image.Format.Rgba8);
		image.Fill(Colors.White);	
		_multiMeshInstance.Texture = ImageTexture.CreateFromImage(image);

		_multiMesh = new MultiMesh
		{
			TransformFormat = MultiMesh.TransformFormatEnum.Transform2D,
			UseColors = true,
			Mesh = new QuadMesh
			{
				Size = new Vector2(InstanceSize, InstanceSize)
			},
			InstanceCount = MaxEnemySpawnable
		};
		_multiMeshInstance.Multimesh = _multiMesh;

		var hidden = new Transform2D(Vector2.Zero, Vector2.Zero, Vector2.Zero);
		for (int i = 0; i < MaxEnemySpawnable; i++)
			_multiMesh.SetInstanceTransform2D(i, hidden);
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

		UpdateAgents((float)delta);
	}
	
		private void SpawnBatch(int count)
	{
		for (int i = 0; i < count && _count < MaxEnemySpawnable; i++)
		{
			var area = _spawnAreas[GD.RandRange(0, _spawnAreas.Count - 1)];
			_positions[_count] = RandomPointInArea(area);
			_multiMesh.SetInstanceColor(_count, Colors.Red);
			_count++;
		}
	}
	
	private void UpdateAgents(float delta)
	{
		Vector2 playerPos = _player.GlobalPosition;
		for (int i = 0; i < _count; i++)
		{
			Vector2 direction = (playerPos - _positions[i]).Normalized();
			_positions[i] += direction * Speed * delta;
			_multiMesh.SetInstanceTransform2D(i, new Transform2D(0f, _positions[i]));
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
