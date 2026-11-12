using Godot;

[GlobalClass]
public partial class DamagingComponent : Area3D //Компонент отвечает за нанесение урона объектам при столкновении
{
    [Export] public float Damage = 10f;

    public override void _Ready() //Подключает обработчик столкновения с объектом
    {
        BodyEntered += OnBodyEntered;
    }

    protected virtual void OnBodyEntered(Node3D body) //При столкновении наносит урон и удаляет снаряд, если цель имеет здоровье
    {
		GD.Print("Источник: " + GetParent().Name + ", цель: " + body.Name);
        ApplyDamage(body);
        if (body.GetNodeOrNull<HealthComponent>("HealthComponent") != null)
        	GetParent().QueueFree();
    }

    public virtual void ApplyDamage(Node3D target) //Находит компонент здоровья цели и уменьшает её здоровье
    {
        HealthComponent health =
            target.GetNodeOrNull<HealthComponent>("HealthComponent");

        if (health != null)
        {
            health.TakeDamage(Damage);
        }
    }
}

