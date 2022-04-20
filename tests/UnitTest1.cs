using NUnit.Framework;
using lib;

namespace tests;

public class Tests
{
    public GameHelper helper;
    [SetUp]
    public void Setup()
    {
        helper = new GameHelper(new InMemoryGameStorageService());
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
        var game = helper.GenerateTestGame();
        var (weapons, armor, crafting) = InventoryHelper.SplitInventory(game.player.Inventory);
        Assert.AreEqual("Weapon 1", weapons[0].Name);
        Assert.AreEqual("Weapon 2", weapons[1].Name);
        Assert.AreEqual("Craft 1", crafting[0].Name);
        Assert.AreEqual("Armor 1", armor[0].Name);
    }

    [Test]
    public void TestTakeDamage()
    {
        var game = helper.GenerateTestGame();
        game.player.TakeDamage(3);
        Assert.AreEqual(97, game.player.Health);
    }

    [Test]
    public void TestAttack()
    {
        var game = helper.GenerateTestGame();
        var wolf = new Enemy(Entity.EntityType.Wolf);
        game.player.Attack(wolf);
        Assert.AreEqual(4, wolf.Health);
    }

    [Test]
    public void TestEquipWeapon()
    {
        var game = helper.GenerateTestGame();
        var sword = new Weapon("sword", 5, 1, 1);
        game.player.EquipWeapon(sword);
        Assert.AreEqual("sword", game.player.EquippedWeapon.Name);
        Assert.AreEqual(5, game.player.EquippedWeapon.DamageModifier);
    }

    [Test]
    public void TestEquipArmor()
    {
        var game = helper.GenerateTestGame();
        var plate = new Armor("plate", 5, 2, 3);
        game.player.EquipArmor(plate);
        Assert.AreEqual("plate", game.player.EquippedArmor.Name);
        Assert.AreEqual(5, game.player.EquippedArmor.DefenseModifier);
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

    [Test]
    public void TestCombineArmorWithDefenseSelected()
    {
        var a1 = new Armor("a1", 10, 5, 2);
        var a2 = new Armor("a2", 2, 7, 10);
        var a3 = InventoryHelper.CombineArmor(a1, a2, Item.ArmorAttributes.Defense);
        Assert.AreEqual("a1", a3.Name);
        Assert.AreEqual(10, a3.DefenseModifier);
        Assert.AreEqual(6, a3.DodgeChance);
        Assert.AreEqual(6, a3.AttackBonus);
    }

    [Test]
    public void TestCombineArmorWithDodgeChanceSelected()
    {
        var a1 = new Armor("a1", 11, 5, 3);
        var a2 = new Armor("a2", 2, 7, 13);
        var a3 = InventoryHelper.CombineArmor(a1, a2, Item.ArmorAttributes.DodgeChance);
        Assert.AreEqual("a1", a3.Name);
        Assert.AreEqual(6, a3.DefenseModifier);
        Assert.AreEqual(7, a3.DodgeChance);
        Assert.AreEqual(8, a3.AttackBonus);
    }

    [Test]
    public void TestCombineArmorWithAttackBonusSelected()
    {
        var a1 = new Armor("a1", 8, 4, 3);
        var a2 = new Armor("a2", 2, 3, 6);
        var a3 = InventoryHelper.CombineArmor(a1, a2, Item.ArmorAttributes.AttackBonus);
        Assert.AreEqual("a1", a3.Name);
        Assert.AreEqual(5, a3.DefenseModifier);
        Assert.AreEqual(3, a3.DodgeChance);
        Assert.AreEqual(6, a3.AttackBonus);
    }

    [Test]
    public void TestPlayerTakeDamageWithArmor()
    {
        var game = new Game("test");
        var armor = new Armor("armor", 10, 0, 0);
        game.player.GainItem(armor);
        game.player.EquipArmor(armor);
        game.player.TakeDamage(3);
        Assert.AreEqual(98, game.player.Health);
    }

    [Test]
    public void TestForgeArmor()
    {
        var a = InventoryHelper.ForgeArmor("forged armor");
        Assert.AreEqual("forged armor", a.Name);
        Assert.Less(a.DefenseModifier, 30);
        Assert.Less(a.DodgeChance, 10);
        Assert.Less(a.AttackBonus, 5);
    }

    [Test]
    public void TestForgeWeapon()
    {
        var w = InventoryHelper.ForgeWeapon("forged weapon");
        Assert.AreEqual("forged weapon", w.Name);
        Assert.Less(w.DamageModifier, 10);
        Assert.Less(w.CriticalChance, 20);
        Assert.Less(w.CriticalModifier, 5);
    }
}