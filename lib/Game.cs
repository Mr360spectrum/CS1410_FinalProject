namespace lib;

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

    public string GetPlayerName()
    {
        return this.player.Name;
    }
}
