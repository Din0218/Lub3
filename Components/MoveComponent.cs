using Godot;

[GlobalClass]
public partial class MoveComponent : Node //Компонент отвечает за движение объекта к выбранной цели
{
    [Export] public float Speed = 5f;

    private CharacterBody3D _parentNode;
    private Node3D _target;

    public override void _Ready() //Получает объект, которым будет управлять компонент
    {
        _parentNode = GetParent<CharacterBody3D>();
    }

    public void SetTarget(Node3D target) //Устанавливает цель движения
    {
        _target = target;
    }

    public override void _PhysicsProcess(double delta) //Двигает объект к цели
    {
        if (_parentNode == null)
            return;

        if (_target == null)
        {
            _parentNode.Velocity = Vector3.Zero;
            return;
        }

        Vector3 direction =
            _target.GlobalPosition - _parentNode.GlobalPosition;

        direction.Y = 0;

        if (direction.Length() > 0.5f)
        {
            _parentNode.Velocity =
                direction.Normalized() * Speed;
        }
        else
        {
            _parentNode.Velocity = Vector3.Zero;
        }

        _parentNode.MoveAndSlide();
    }
}
