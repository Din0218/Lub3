using Godot;

public partial class Bullet : Area3D //Класс отвечает за движение пули и настройку её урона
{
    [Export] public float Speed = 20f;

    public override void _PhysicsProcess(double delta) //Перемещает пулю 
    {
        GlobalPosition += -GlobalTransform.Basis.Z
                          * Speed * (float)delta;
    }

    public void SetSpeed(float speed) //Устанавливает скоростьт пули
    {
        Speed = speed;
    }

    public void SetDamage(float damage) //Устанавливает урон для обычной, взрывной и замедляющей пули
    {
        DamagingComponent component =
            GetNodeOrNull<DamagingComponent>("DamagingComponent");

        if (component == null)
            component = GetNodeOrNull<DamagingComponent>("ExplosionComponent");

        if (component == null)
            component = GetNodeOrNull<DamagingComponent>("FreezeComponent");

        if (component != null)
            component.Damage = damage;
    }
}
