namespace lib;

/// <summary>
/// The abstract class that Player and Enemy inherit from.
/// </summary>
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

    /// <summary>
    /// Subtracts the proper amount of health from the Entity's health after
    /// decreasing the damage amount based on the Entity's defense.
    /// </summary>
    /// <param name="amount">The damage amount to use.</param>
    public virtual void TakeDamage(double amount)
    {
        this.Health = (int)Math.Floor(Health - (amount - (Defense * 0.1)));
    }
}

/// <summary>
/// Represents an enemy of type Wolf, Knight, or Wizard. Each type is given
/// unique stats.
/// </summary>
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