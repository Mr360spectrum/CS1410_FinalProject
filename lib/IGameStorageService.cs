namespace lib;

public interface IGameStorageService
{
    void Save(Game game);
    Game LoadGame();
}

public class GameStorageService : IGameStorageService
{
    public Game LoadGame()
    {
        throw new NotImplementedException();
    }

    public void Save(Game game)
    {
        throw new NotImplementedException();
    }
}