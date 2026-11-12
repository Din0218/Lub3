using Godot;
[GlobalClass]
public partial class ShootComponent : Node3D //Компонент отвечает за автомтическую стрельбу по выбранной цели
{
    [Export] public PackedScene BulletScene;
    [Export] public float BulletDamage = 10f;
    [Export] public float BulletSpeed = 20f;
    [Export] public float FireRate = 2f;

    private Node3D _target;
    private Timer _shootTimer;

    public override void _Ready() //Создаёт и запускает таймер для стрельбы 
    {
        _shootTimer = new Timer();
        _shootTimer.WaitTime = 1.0 / FireRate;
        _shootTimer.OneShot = false;

        AddChild(_shootTimer);
        _shootTimer.Timeout += Shoot;
        _shootTimer.Start();
    }

    private void Shoot() //Создаёт пулю, если есть цель
    {
        if (_target == null || !IsInstanceValid(_target))
            return;

        if (BulletScene == null)
            return;

        Bullet bullet = BulletScene.Instantiate<Bullet>();

        GetTree().CurrentScene.AddChild(bullet);

        Vector3 direction = (_target.GlobalPosition - GlobalPosition).Normalized();
		bullet.GlobalPosition = GlobalPosition + direction * 2f;
		bullet.LookAt(_target.GlobalPosition, Vector3.Up);

        bullet.SetSpeed(BulletSpeed);
        bullet.SetDamage(BulletDamage);
    }

    public void SetTarget(Node3D target) //Устанавливает цель для стрельбы
    {
        _target = target;
    }
}

