using Godot;

public partial class Player : CharacterBody3D //Управляет движением игрока, стрельбой, размещением башен
{
	[Export] public PackedScene BulletScene;
	[Export] public PackedScene TowerScene;
	[Export] public PackedScene FreezeTowerScene;
	[Export] public PackedScene ExplosionTowerScene;
    [Export] public float Speed = 5f;
	private Node3D previewTower = null;
	[Export] public Material PreviewMaterial;

    public override void _PhysicsProcess(double delta) //Обрабатывает нажатия WASD, поворот к курсору и положение предварительной башни
    {
        Vector3 direction = Vector3.Zero;

        if (Input.IsKeyPressed(Key.W))
            direction.Z -= 1;

        if (Input.IsKeyPressed(Key.S))
            direction.Z += 1;

        if (Input.IsKeyPressed(Key.A))
            direction.X -= 1;

        if (Input.IsKeyPressed(Key.D))
            direction.X += 1;

        Velocity = direction.Normalized() * Speed;
        MoveAndSlide();

		if (previewTower != null)
		{
			previewTower.GlobalPosition = GlobalPosition
				+ (-GlobalTransform.Basis.Z * 3f);
		}

		Camera3D camera = GetViewport().GetCamera3D();

		if (camera != null)
		{
			Vector2 mouse = GetViewport().GetMousePosition();

			Vector3 start = camera.ProjectRayOrigin(mouse);
			Vector3 rayDirection = camera.ProjectRayNormal(mouse);

			Plane floor = new Plane(Vector3.Up, GlobalPosition.Y);
			Vector3? point = floor.IntersectsRay(start, rayDirection);

			if (point != null)
			{
				LookAt(point.Value, Vector3.Up);
			}
		}
    }

	public override void _Input(InputEvent @event) //Обрабатывает нажатия мыши для стрельбы и клавиш 1, 2, 3 для выбора башен
	{
		if (@event is InputEventMouseButton mouse && mouse.ButtonIndex == MouseButton.Left && mouse.Pressed)
		{
			Shoot();
		}

		if (Input.IsKeyPressed(Key.Key1))
			PlaceTower(TowerScene);

		if (Input.IsKeyPressed(Key.Key2))
			PlaceTower(FreezeTowerScene);

		if (Input.IsKeyPressed(Key.Key3))
			PlaceTower(ExplosionTowerScene);

	}

	
	private void PlaceTower(PackedScene scene) //Показывает предварительную башню или подтверждает её размещение
	{
		if (scene == null)
			return;

		if (previewTower != null)
		{
			foreach (Node child in previewTower.GetChildren())
			{
				if (child is MeshInstance3D mesh)
				{
					mesh.MaterialOverlay = null;
				}
			}

			previewTower = null;
			return;
		}

		//Если башни ещё нет — создаём её перед игроком
		previewTower = scene.Instantiate<Node3D>(); 
		foreach (Node child in previewTower.GetChildren())
		{
			if (child is MeshInstance3D mesh)
			{
				mesh.MaterialOverlay = PreviewMaterial;
			}
		}
		GetTree().CurrentScene.AddChild(previewTower);

		previewTower.GlobalPosition = GlobalPosition
			+ (-GlobalTransform.Basis.Z * 3f);
	}


	private void Shoot() //Создаёт пулю перед игроком и направляет её в сторону взгляда
	{
		if (BulletScene == null)
			return;

		Bullet bullet = BulletScene.Instantiate<Bullet>();
		GetTree().CurrentScene.AddChild(bullet);

		Vector3 direction = -GlobalTransform.Basis.Z;
		bullet.GlobalPosition = GlobalPosition + direction * 2f + Vector3.Up * 0.5f;
		bullet.GlobalRotation = GlobalRotation;
	}

}

