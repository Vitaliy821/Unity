public class Enemy : EnemyBaseController
{
    public EnemiesTypes Type { get; private set; }

    public Enemy(string _name, int damage, int health, EnemiesTypes type)
        : base(name)
    {
        this.Name = _name;
        this.Damage = damage;
        this.Health = health;
        this.EnemiesType = type;
    }

    protected override void Move()
    {
        base.Move();
    }

    public override void Interact()
    {
        base.Attack();
    }
}


