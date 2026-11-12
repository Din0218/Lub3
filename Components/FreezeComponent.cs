using Godot;

[GlobalClass]
public partial class FreezeComponent : DamagingComponent //Компонент наносит урон и временно замедляет противника
{
    [Export] public float FreezeTime = 3f;
    [Export] public float SlowMultiplier = 0.5f;

    public override void ApplyDamage(Node3D target) //Наносит урон цели и замедляет её, если у неё есть MoveComponent
    {
        base.ApplyDamage(target);

        MoveComponent move =
            target.GetNodeOrNull<MoveComponent>("MoveComponent");

        if (move != null)
        {
            SlowDown(move);
        }
    }

    private async void SlowDown(MoveComponent move) //Уменьшает скорость на заданное время, затем восстанавливает её
    {
        float originalSpeed = move.Speed;

        move.Speed *= SlowMultiplier;

        await ToSignal(
            GetTree().CreateTimer(FreezeTime),
            SceneTreeTimer.SignalName.Timeout
        );

        if (GodotObject.IsInstanceValid(move))
        {
            move.Speed = originalSpeed;
        }
    }
}
