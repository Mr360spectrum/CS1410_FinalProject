namespace lib;

public abstract class Entity
{
    public int Health {get; protected set; }
    public int Defense {get; protected set; }
    public EntityType Type {get; protected set; }
    public enum EntityType
    {
        Player,
        Knight,
        Wolf
    }

    public void TakeDamage(double amount)
    {
        this.Health = (int)Math.Floor(Health - (amount - (Defense * 0.1)));
    }
}

public class Enemy : Entity
{
    public Enemy(EntityType inType)
    {
        this.Type = inType;
        switch (this.Type)
        {
            case EntityType.Knight:
                Health = 20;
                Defense = 20;
                break;
            case EntityType.Wolf:
                Health = 10;
                Defense = 5;
                break;
            default: 
                throw new ArgumentException("Entity type was set to an invalid type.");
        }

    }
}