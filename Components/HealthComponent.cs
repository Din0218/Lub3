using Godot;

[GlobalClass]
public partial class HealthComponent : Node //Компонент хранит здоровье объекта и обрабатывает его уничтожение
{
    [Export] public float MaxHealth = 100f;
    public float CurrentHealth;

    public override void _Ready() //Устанавливает начальное здоровье объекта
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage) //Уменьшает здоровье на величину урона и проверяет, уничтожен ли объект

    {
        CurrentHealth -= damage;
		GD.Print("Осталось здоровья: " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die() //Удаляет уничтоженный объект, а при уничтожении ядра останавливает игру
	{
		if (GetParent().Name == "Core")
		{
			GD.Print("Игра окончена! Ядро уничтожено!");
			GetTree().Paused = true;
		}
		else
		{
			GetParent().QueueFree();
		}
	}
}

