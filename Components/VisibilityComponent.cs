using Godot;

[GlobalClass]
public partial class VisibilityComponent : Area3D //Компонент обнаруживает ближайшую цель в радиусе видимости
{
    [Export] public float Radius = 10f;
    [Export] public string TargetGroup = "enemies";

    private ShootComponent _shoot;

    public override void _Ready() //Получает компонент стрельбы родительского объекта
    {
        _shoot = GetParent().GetNode<ShootComponent>("ShootComponent");
    }

    public override void _PhysicsProcess(double delta) //Каждый кадр ищет ближайшую цель нужной группы
    {
        Node3D closest = null;
        float minDistance = Radius;

        //Проверяем объекты в радиусе видимости
        foreach (Node3D body in GetOverlappingBodies())
        {
            if (!body.IsInGroup(TargetGroup))
                continue;

            float distance =
                GlobalPosition.DistanceTo(body.GlobalPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = body;
            }
        }

        _shoot.SetTarget(closest); //Передаёт найденную цель компоненту стрельбы
    }
}
