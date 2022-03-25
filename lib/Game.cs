namespace lib;

public class Game
{
    private Player player;

    public Game(string inName)
    {
        this.player = new Player(inName);
    }

    public string GetPlayerName()
    {
        return this.player.Name;
    }
}
