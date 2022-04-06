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

    [Test]
    public void TestEmptyPlayerName()
    {
        var game = new Game("Test");
        Assert.Throws<EmptyNameException>(() => game.player.Name = "");
    }

    [Test]
    public void TestEmptyItemName()
    {
        var game = new Game("Test");
        Assert.Throws<EmptyNameException>(() => new Weapon(null));
        Assert.Throws<EmptyNameException>(() => new CraftingItem(""));
    }

    [Test]
    public void TestSplitInventory()
    {
        var game = GameHelper.GenerateTestGame();
        var (weapons, armor, crafting) = GameHelper.SplitInventory(game.player.Inventory);
        Assert.AreEqual("Weapon 1", weapons[0].Name);
        Assert.AreEqual("Weapon 2", weapons[1].Name);
        Assert.AreEqual("Craft 1", crafting[0].Name);
        Assert.AreEqual("Armor 1", armor[0].Name);
    }
}