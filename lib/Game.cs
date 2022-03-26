namespace lib;

public class Game
{
    private Player player;

    public Game(string inName)
    {
        this.player = new Player(inName);
    }
    public Game(string inName, List<Item> inventory)
    {
        this.player = new Player(inName, inventory);
    }

    public string GetPlayerName()
    {
        return this.player.Name;
    }

    public void Play()
    {
        player.ShowInventory();
    }

}
