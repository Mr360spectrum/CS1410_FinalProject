namespace lib;

/// <summary>
/// Represents the object that contains the Player object and handles assigning 
/// property values to the Player object when existing saves are used.
/// </summary>
public class Game
{
    public Player player { get; set; }

    public Game(string inName)
    {
        this.player = new Player(inName);
    }
    public Game(string inName, List<Item> inventory, Weapon equippedWeapon, Armor equipppedArmor)
    {
        this.player = new Player(inName, inventory);
        player.EquipArmor(equipppedArmor);
        player.EquipWeapon(equippedWeapon);
    }
    //* Only for JSON deserialization
    public Game()
    {

    }

    /// <summary>
    /// Gets the Name property from the Player object.
    /// </summary>
    /// <returns>A string representing the Player object's Name value.</returns>
    public string GetPlayerName()
    {
        return this.player.Name;
    }
}
