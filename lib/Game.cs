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
        Console.Clear();
        var sword = new Weapon("Sword of Oldness");

        Console.WriteLine("You found an old sword.");
        Console.WriteLine("Press a key to pick it up.");
        Console.ReadKey(true);
        Console.WriteLine($"You picked up '{sword.Name}'");
        player.GainItem(sword);

        player.ShowInventory();
        GameHelper.Save(this);
        System.Environment.Exit(0);
    }
}
