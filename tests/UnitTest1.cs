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
    public void TestEmptyPlayerName()
    {
        var game = new Game("Test");
        Assert.Throws<EmptyNameException>(() => game.player.Name = "");
    }

    [Test]
    public void TestEmptyItemName()
    {
        var game = new Game("Test");
        Assert.Throws<EmptyNameException>(() => new Weapon(null, 0, 0, 0));
        Assert.Throws<EmptyNameException>(() => new CraftingItem(""));
    }

    [Test]
    public void TestSplitInventory()
    {
        var game = GameHelper.GenerateTestGame();
        var (weapons, armor, crafting) = InventoryHelper.SplitInventory(game.player.Inventory);
        Assert.AreEqual("Weapon 1", weapons[0].Name);
        Assert.AreEqual("Weapon 2", weapons[1].Name);
        Assert.AreEqual("Craft 1", crafting[0].Name);
        Assert.AreEqual("Armor 1", armor[0].Name);
    }

    [Test]
    public void TestTakeDamage()
    {
        var game = GameHelper.GenerateTestGame();
        game.player.TakeDamage(3);
        Assert.AreEqual(97, game.player.Health);
    }

    [Test]
    public void TestAttack()
    {
        var game = GameHelper.GenerateTestGame();
        var wolf = new Enemy(Entity.EntityType.Wolf);
        game.player.Attack(wolf);
        Assert.AreEqual(9, wolf.Health);
    }

    [Test]
    public void TestEquipWeapon()
    {
        var game = GameHelper.GenerateTestGame();
        var sword = new Weapon("sword", 5, 1, 1);
        game.player.EquipWeapon(sword);
        Assert.AreEqual("sword", game.player.EquippedWeapon.Name);
        Assert.AreEqual(5, game.player.EquippedWeapon.DamageModifier);
    }

    [Test]
    public void TestCombineWeaponsWithAttackSelected()
    {
        var w1 = new Weapon("sword", 5, 4, 3);
        var w2 = new Weapon("spear", 3, 4, 5);
        var w3 = InventoryHelper.CombineWeapons(w1, w2, Item.WeaponAttributes.Attack);
        Assert.AreEqual("sword", w3.Name);
        Assert.AreEqual(5, w3.DamageModifier);
        Assert.AreEqual(4, w3.CriticalChance);
        Assert.AreEqual(4, w3.CriticalModifier);
    }

    [Test]
    public void TestCombineWeaponsWithCriticalChanceSelected()
    {
        var w1 = new Weapon("sword", 7, 9, 1);
        var w2 = new Weapon("spear", 3, 4, 5);
        var w3 = InventoryHelper.CombineWeapons(w1, w2, Item.WeaponAttributes.CriticalChance);
        Assert.AreEqual("sword", w3.Name);
        Assert.AreEqual(5, w3.DamageModifier);
        Assert.AreEqual(9, w3.CriticalChance);
        Assert.AreEqual(3, w3.CriticalModifier);
    }

    [Test]
    public void TestCombineWeaponsWithCriticalModifierSelected()
    {
        var w1 = new Weapon("sword", 10, 10, 6);
        var w2 = new Weapon("spear", 3, 4, 5);
        var w3 = InventoryHelper.CombineWeapons(w1, w2, Item.WeaponAttributes.CriticalModifier);
        Assert.AreEqual("sword", w3.Name);
        Assert.AreEqual(6, w3.DamageModifier);
        Assert.AreEqual(7, w3.CriticalChance);
        Assert.AreEqual(6, w3.CriticalModifier);
    }
}