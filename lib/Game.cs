namespace lib;

public class Game
{
    public Player player { get; set; }

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
        var sword = new Weapon("Sword of Oldness", 3, 1, 1);

        Console.WriteLine("You found an old sword.");
        Console.WriteLine("Press a key to pick it up.");
        Console.ReadKey(true);
        Console.WriteLine($"You picked up '{sword.Name}'");
        player.GainItem(sword);


            player.ShowInventory();
            GameHelper.Save(this);
        

        var enemy = new Enemy(Entity.EntityType.Knight);
        Console.WriteLine("A knight approaches!");
        while (enemy.Health > 0)
        {
            Console.WriteLine($"Knight health: {enemy.Health}");
            Console.WriteLine("Press a key to attack!");
            Console.ReadKey();
            player.Attack(enemy);
        }
        Console.WriteLine("Knight defeated!");
        
        GameHelper.Save(this);
    }
}
