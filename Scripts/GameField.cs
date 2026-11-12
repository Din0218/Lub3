using Godot;

public partial class GameField : Node3D //Управляет поиском целей для врагов на поле
{
    public override void _Ready() //Получает ссылки на врага, башню и компонент движения
    {
        CharacterBody3D enemy = GetNode<CharacterBody3D>("EnemyTower");
        Node3D tower = GetNode<Node3D>("Tower");

        MoveComponent move =
            enemy.GetNode<MoveComponent>("MoveComponent"); 
    }
	public override void _PhysicsProcess(double delta) //Каждый кадр обновляет цели движения врагов
	{
		CharacterBody3D enemy = GetNodeOrNull<CharacterBody3D>("EnemyTower");
		if (enemy == null) return;
		MoveComponent move = enemy.GetNode<MoveComponent>("MoveComponent");

		Node3D nearest = null;
		float distance = float.MaxValue;

		foreach (Node node in GetTree().GetNodesInGroup("towers")) //Находит ближайшую башню для EnemyTower
		{
			if (node is Node3D target)
			{
				float d = enemy.GlobalPosition.DistanceTo(target.GlobalPosition);

				if (d < distance)
				{
					distance = d;
					nearest = target;
				}
			}
		}

		move.SetTarget(nearest);
		CharacterBody3D crawler = GetNodeOrNull<CharacterBody3D>("Crawler");
		if (crawler == null) return;
		MoveComponent crawlerMove = crawler.GetNode<MoveComponent>("MoveComponent");

		Node3D crawlerNearest = null;
		float crawlerDistance = float.MaxValue;

		foreach (Node node in GetTree().GetNodesInGroup("towers")) //Находит ближайшую башню для Crawler
		{
			if (node is Node3D target)
			{
				float d = crawler.GlobalPosition.DistanceTo(target.GlobalPosition);

				if (d < crawlerDistance)
				{
					crawlerDistance = d;
					crawlerNearest = target;
				}
			}
		}

		crawlerMove.SetTarget(crawlerNearest);
	}	
}

