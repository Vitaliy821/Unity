public class EnemyBaseController : Interactable
{
    protected string Name { get; set; }
    protected int Health { get; set; }
    protected int Damage { get; set; }

    public EnemyBaseController(string name)
    {
        Name = name;
    }

    protected virtual void Move() { }

    public virtual void Attack() { }

}

public enum EnemiesTypes
{
    //types
}

