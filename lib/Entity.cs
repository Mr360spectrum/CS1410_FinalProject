namespace lib;

public abstract class Entity
{
    public int Health {get; protected set; }
    public int Defense {get; protected set; }
    public int Damage {get; protected set; }
    public EntityType Type {get; protected set; }
    public enum EntityType
    {
        Player,
        Knight,
        Wolf,
        Wizard
    }

    public virtual void TakeDamage(double amount)
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
                Defense = 10;
                Damage = 10;
                break;
            case EntityType.Wolf:
                Health = 5;
                Defense = 3;
                Damage = 5;
                break;
            case EntityType.Wizard:
                Health = 10;
                Defense = 5;
                Damage = 8;
                break;
            default: 
                throw new ArgumentException("Entity type was set to an invalid type.");
        }
    }
}