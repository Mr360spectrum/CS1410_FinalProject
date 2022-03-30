namespace lib;

public abstract class Entity
{
    public int Health {get; protected set; }
    public int Defense {get; protected set; }
    public string Type {get; protected set; }
}

public class Enemy : Entity
{
    public Enemy(string inType)
    {
        this.Type = inType;
    }
}