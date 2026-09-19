using Godot;
using System.Collections.Generic;

public partial class OrcManager : Node, IEnemyCountProvider
{
	[Export] public NodePath PlayerPath;
	[Export] public NodePath SpawnAreasPath;
	[Export] public NodePath MultiMeshInstancePath;

	[Export] public float MovementSpeed = 150f;
	
	[Export] public int BatchGrowth = 50;
	[Export] public int InstanceSize = 16;
	[Export] public int InitialBatchSize = 50;
	[Export] public float SpawnInterval = 0.5f;
	[Export] public int MaxEnemySpawnable = 50000;
	
	private MultiMeshInstance2D _multiMeshInstance;
	private MultiMesh _multiMesh;
	private Player _player;
	private readonly List<Area2D> _spawnAreas = new();
	
	private Vector2[] _orcPositions;
	private float[] _orcHealths;
	
	private int _activeOrcCount;
	private float _timer;
	private int _batchSize;

	public int TotalSpawned => _activeOrcCount;

	public override void _Ready()
	{
		_multiMeshInstance = GetNode<MultiMeshInstance2D>(MultiMeshInstancePath);
		_player = GetNode<Player>(PlayerPath);
		_batchSize = InitialBatchSize;
		
		_orcPositions = new Vector2[MaxEnemySpawnable];
		_orcHealths = new float[MaxEnemySpawnable];
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
			Mesh = new QuadMesh { Size = new Vector2(InstanceSize, InstanceSize) },
			InstanceCount = MaxEnemySpawnable
		};
		_multiMeshInstance.Multimesh = _multiMesh;

		var unspawned_orcs = new Transform2D(Vector2.Zero, Vector2.Zero, Vector2.Zero);
		for (int i = 0; i < MaxEnemySpawnable; i++)
			_multiMesh.SetInstanceTransform2D(i, unspawned_orcs);
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

		UpdateEnemies((float)delta);
	}
	
	
	private void SpawnBatch(int numOfOrcToSpawn)
	{
		for (int i = 0; i < numOfOrcToSpawn && _activeOrcCount < MaxEnemySpawnable; i++)
		{
			var area = _spawnAreas[GD.RandRange(0, _spawnAreas.Count - 1)];
			_orcPositions[_activeOrcCount] = RandomPointInArea(area);
			_multiMesh.SetInstanceColor(_activeOrcCount, Colors.Red);
			_activeOrcCount++;
		}
	}
	
	private void UpdateEnemies(float delta)
	{
		Vector2 playerPos = _player.GlobalPosition;
		
		for (int i = 0; i < _activeOrcCount; i++)
		{
			Vector2 toPlayer = playerPos - _orcPositions[i];
			float distance = toPlayer.Length();
			_orcPositions[i] += toPlayer.Normalized() * MovementSpeed * delta;
			_multiMesh.SetInstanceTransform2D(i, new Transform2D(0f, _orcPositions[i]));
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
