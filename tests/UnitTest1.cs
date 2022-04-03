using NUnit.Framework;
using lib;

namespace tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestPlayerName()
    {
        var game = new Game("Test");
        Assert.AreEqual("Test", game.GetPlayerName());
    }

    [Test]
    public void TestSaveAndLoad()
    {
        var game = new Game("Test");
        GameHelper.Save(game);
        var loadedGame = GameHelper.DeserializeGame("Test.json");
        Assert.AreEqual("Test", loadedGame.GetPlayerName());
    }
}