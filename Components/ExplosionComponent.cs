using Godot;

[GlobalClass]
public partial class ExplosionComponent : DamagingComponent //Компонент наносит урон нескольким объектам в радиусе взрыва
{
    [Export] public float ExplosionRadius = 3f;
    private void ShowExplosion() //Показывает эффект взрыва и удаляет его через 0,3 секунды
    {
        MeshInstance3D effect =
            GetParent().GetNode<MeshInstance3D>("ExplosionEffect");

        effect.Reparent(GetTree().CurrentScene, true);
        effect.Visible = true;

        GetTree().CreateTimer(0.3).Timeout += () => effect.QueueFree();
    }

    public override void ApplyDamage(Node3D target) //Наносит урон всем обнаруженным объектам в радиусе взрыва
    {
        ShowExplosion();
        foreach (Node3D body in GetOverlappingBodies())
        {
            if (body.GlobalPosition.DistanceTo(GlobalPosition)
                > ExplosionRadius)
                continue;

            base.ApplyDamage(body);
        }
    }
}
