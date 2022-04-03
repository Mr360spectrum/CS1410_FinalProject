namespace lib;

public class Game
{
    public Player player {get; set;}

    public Game(string inName)
    {
        this.player = new Player(inName);
    }
    public Game(string inName, List<Item> inventory)
    {
        this.player = new Player(inName, inventory);
    }
    //* Only for JSON deserialization
    public Game()
    {

    }

    public string GetPlayerName()
    {
        return this.player.Name;
    }

    public void Play()
    {
        player.Inventory.Add(new Weapon("Weapon 1"));
        player.Inventory.Add(new Weapon("Weapon 2"));
        player.Inventory.Add(new CraftingItem("Craft"));

        player.ShowInventory();
        GameHelper.Save(this);
        System.Environment.Exit(0);
    }
}
