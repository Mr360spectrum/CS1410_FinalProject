namespace lib;
using System.Collections.Generic;

public class Player : Entity
{
    private string name;
    public string Name
    {
        get => name;
        set
        {
            if (value == "" || value is null)
            {
                throw new EmptyNameException("Player name cannot be set as null or an empty string.");
            }
            this.name = value;
        }
    }

    private List<Item> inventory = new List<Item>();
    [System.Text.Json.Serialization.JsonIgnore] //Do not serialize Inventory directly from player
    public List<Item> Inventory
    {
        get => inventory;
        set => inventory = value;
    }

    private Weapon? equippedWeapon;
    public Weapon? EquippedWeapon
    {
        get => equippedWeapon;
        set => equippedWeapon = value;
    }
    private Armor? equippedArmor;
    public Armor? EquippedArmor
    {
        get => equippedArmor;
        set => equippedArmor = value;
    }

    public Player(string inName)
    {
        this.name = inName;
        this.inventory = new List<Item>();
        Health = 100;
        Defense = 0;
    }
    public Player(string inName, List<Item> inInventory)
    {
        this.name = inName;
        inventory = inInventory;
        Health = 100;
        Defense = 0;
    }
    //* Only for JSON deserialization
    public Player()
    {
        //Reset health to 100 if 0 or not set
        if (Health == 0)
        {
            Health = 100;
        }
    }

    public void Attack(Enemy inEnemy)
    {
        if (EquippedWeapon != null)
        {
            inEnemy.TakeDamage(EquippedWeapon.DamageModifier);
        }
        else
        {
            inEnemy.TakeDamage(1);
        }
    }

    public override void TakeDamage(double amount)
    {
        if (EquippedArmor != null)
        {
            var rand = new Random();
            var randNum = rand.Next(101);
            if (randNum < this.EquippedArmor.DodgeChance)
            {
                return;
            }
            else
            {
                base.TakeDamage(amount);
            }
        }
        else
        {
            base.TakeDamage(amount);
        }
    }

    public void EquipWeapon(Weapon inWeapon)
    {
        this.EquippedWeapon = inWeapon;
    }

    public void EquipArmor(Armor inArmor)
    {
        this.EquippedArmor = inArmor;
        if (this.EquippedArmor != null)
        {
            this.Defense = EquippedArmor.DefenseModifier;
        }
    }    

    public void GainItem(Item item)
    {
        this.inventory.Add(item);
    }
}
