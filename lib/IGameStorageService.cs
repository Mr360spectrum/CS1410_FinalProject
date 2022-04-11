namespace lib;

public interface IGameStorageService
{
    public void Save(Game game);
    public Game LoadGame();
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