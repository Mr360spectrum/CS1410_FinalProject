namespace lib;
using System.Collections.Generic;

public class Player : Entity
{
    private string name;
    public string Name
    {
        get => name;
        set
        {
            if (value == "" || value is null)
            {
                throw new EmptyNameException("Player name cannot be set as null or an empty string.");
            }
            this.name = value;
        }
    }

    private List<Item> inventory = new List<Item>();
    [System.Text.Json.Serialization.JsonIgnore] //Do not serialize Inventory directly from player
    public List<Item> Inventory
    {
        get => inventory;
        set => inventory = value;
    }

    private Weapon? equippedWeapon;
    public Weapon? EquippedWeapon
    {
        get => equippedWeapon;
        set => equippedWeapon = value;
    }
    private Armor? equippedArmor;
    public Armor? EquippedArmor
    {
        get => equippedArmor;
        set => equippedArmor = value;
    }

    public Player(string inName)
    {
        this.name = inName;
        this.inventory = new List<Item>();
        Health = 100;
        Defense = 0;
    }
    public Player(string inName, List<Item> inInventory)
    {
        this.name = inName;
        inventory = inInventory;
        Health = 100;
        Defense = 0;
    }
    //* Only for JSON deserialization
    public Player()
    {
        //Reset health to 100 if 0 or not set
        if (Health == 0)
        {
            Health = 100;
        }
    }

    public void Attack(Enemy inEnemy)
    {
        if (EquippedWeapon != null)
        {
            inEnemy.TakeDamage(EquippedWeapon.DamageModifier);
        }
        else
        {
            Console.WriteLine("There is no weapon equipped!");
            Console.WriteLine("Hand-to-hand combat is fine, I guess.");
            inEnemy.TakeDamage(1);
        }
    }

    public override void TakeDamage(double amount)
    {
        if (EquippedArmor != null)
        {
            var rand = new Random();
            var randNum = rand.Next(101);
            if (randNum < this.EquippedArmor.DodgeChance)
            {
                return;
            }
            else
            {
                base.TakeDamage(amount);
            }
        }
        else
        {
            base.TakeDamage(amount);
        }
    }

    public void EquipWeapon(Weapon inWeapon)
    {
        this.EquippedWeapon = inWeapon;
    }

    public void EquipArmor(Armor inArmor)
    {
        this.EquippedArmor = inArmor;
        if (this.EquippedArmor != null)
        {
            this.Defense = EquippedArmor.DefenseModifier;
        }
    }

    public void ShowInventory(bool forgeInArea)
    {
        while (true)
        {
            Console.Clear();
            if (this.inventory.Count == 0)
            {
                Console.WriteLine("The inventory is empty.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                return;
            }

            int i = 0;
            Console.WriteLine("");
            foreach (var item in this.inventory)
            {
                Console.WriteLine((i + 1) + ". " + item.Name);
                i++;
            }

            Console.WriteLine("Enter an empty string to exit.");
            Console.WriteLine("Select which item you'd like to view options for: ");
            string input = Console.ReadLine();

            if (input == "")
            {
                //!REMOVE
                return;
            }
            int selection = int.Parse(input) - 1;

            var selectedItem = inventory[selection];
            Console.Clear();
            if (!forgeInArea)
            {
                if (selectedItem.Type == Item.ItemType.Crafting)
                {
                    Console.WriteLine("There are no options available.");
                }
                else if (selectedItem.Type == Item.ItemType.Armor)
                {
                    Console.WriteLine("Select an option:");
                    Console.WriteLine("1. Rename");
                    Console.WriteLine("2. Equip");
                    Console.WriteLine("3. Go back");
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            Console.WriteLine("What would you like to rename the item to?");
                            (selectedItem as Armor).Rename(Console.ReadLine());
                            break;
                        case "2":
                            EquipArmor(selectedItem as Armor);
                            Console.WriteLine($"{selectedItem.Name} equipped.");
                            break;
                        case "3":
                            continue;
                        default:
                            Console.WriteLine("That is not a valid option.");
                            continue;
                    }
                }
                else if (selectedItem.Type == Item.ItemType.Weapon)
                {
                    Console.WriteLine("Select an option:");
                    Console.WriteLine("1. Rename");
                    Console.WriteLine("2. Equip");
                    Console.WriteLine("3. Go back");
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            Console.WriteLine("What would you like to rename the item to?");
                            (selectedItem as Weapon).Rename(Console.ReadLine());
                            break;
                        case "2":
                            EquipWeapon(selectedItem as Weapon);
                            Console.WriteLine($"{selectedItem.Name} equipped.");
                            break;
                        case "3":
                            continue;
                        default:
                            Console.WriteLine("That is not a valid option.");
                            continue;
                    }
                }
            }
            else
            {
                if (selectedItem.Type == Item.ItemType.Crafting)
                {
                    //TODO: Check for number of sticks and remove them
                    Console.WriteLine("Select an option:");
                    Console.WriteLine("1. Forge Weapon (uses two sticks)");
                    Console.WriteLine("2. Forge Armor (uses two sticks)");
                    Console.WriteLine("3. Go back");
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            var stickCountForWeapon = 0;
                            foreach (var item in inventory)
                            {
                                if (item is CraftingItem)
                                {
                                    stickCountForWeapon++;
                                }
                            }
                            if (stickCountForWeapon >= 2)
                            {
                                int removeWeaponStickCount = 0;
                                inventory.RemoveAt(selection);
                                for (int k = 0; k < inventory.Count; k++)
                                {
                                    if (inventory[k] is CraftingItem)
                                    {
                                        inventory.RemoveAt(k);
                                        removeWeaponStickCount++;
                                    }
                                    if (removeWeaponStickCount == 2)
                                    {
                                        break;
                                    }
                                }
                                GainItem(InventoryHelper.ForgeWeapon("Forged Weapon"));
                            }
                            else
                            {
                                Console.WriteLine("You don't have enough sticks!");
                            }
                            break;
                        case "2":
                            var stickCountForArmor = 0;
                            foreach (var item in inventory)
                            {
                                if (item is CraftingItem)
                                {
                                    stickCountForArmor++;
                                }
                            }
                            if (stickCountForArmor >= 2)
                            {
                                int removeArmorStickCount = 0;
                                inventory.RemoveAt(selection);
                                for (int k = 0; k < inventory.Count; k++)
                                {
                                    if (inventory[k] is CraftingItem)
                                    {
                                        inventory.RemoveAt(k);
                                        removeArmorStickCount++;
                                    }
                                    if (removeArmorStickCount == 2)
                                    {
                                        break;
                                    }
                                }
                                GainItem(InventoryHelper.ForgeArmor("Forged Armor"));
                            }
                            else
                            {
                                Console.WriteLine("You don't have enough sticks!");
                            }
                            break;
                        case "3":
                            continue;
                        default:
                            Console.WriteLine("That is not a valid option.");
                            continue;
                    }
                }
                else if (selectedItem.Type == Item.ItemType.Armor)
                {
                    Console.WriteLine("Select an option:");
                    Console.WriteLine("1. Rename");
                    Console.WriteLine("2. Equip");
                    Console.WriteLine("3. Combine");
                    Console.WriteLine("4. Go back");
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            Console.WriteLine("What would you like to rename the item to?");
                            (selectedItem as Armor).Rename(Console.ReadLine());
                            break;
                        case "2":
                            EquipArmor(selectedItem as Armor);
                            Console.WriteLine($"{selectedItem.Name} equipped.");
                            break;
                        case "3":
                            Console.WriteLine("Select another armor piece to combine:");
                            int j = 0;
                            Console.WriteLine("");
                            foreach (var item in this.inventory)
                            {
                                if (item is Armor)
                                {
                                    Console.WriteLine((j + 1) + ". " + item.Name);
                                }
                                j++;
                            }

                            var otherArmor = Console.ReadLine();

                            Console.WriteLine("Select which attribute to keep (the rest will be determined by the average):");
                            var armorAttributes = new Item.ArmorAttributes[] { Item.ArmorAttributes.AttackBonus, Item.ArmorAttributes.Defense, Item.ArmorAttributes.DodgeChance };

                            var armorAttrIndex = 0;
                            foreach (var attribute in armorAttributes)
                            {
                                Console.WriteLine($"{armorAttrIndex + 1}. {attribute.ToString()}");
                            }
                            var armorAttrSelection = int.Parse(Console.ReadLine());
                            var selectedArmorAttr = armorAttributes[armorAttrSelection];

                            GainItem(InventoryHelper.CombineArmor(selectedItem as Armor, inventory[int.Parse(otherArmor) - 1] as Armor, selectedArmorAttr));
                            inventory.Remove(selectedItem);
                            inventory.RemoveAt(int.Parse(otherArmor) - 1);
                            continue;
                        case "4":
                            continue;
                        default:
                            Console.WriteLine("That is not a valid option.");
                            continue;
                    }
                }
                else if (selectedItem.Type == Item.ItemType.Weapon)
                {
                    Console.WriteLine("Select an option:");
                    Console.WriteLine("1. Rename");
                    Console.WriteLine("2. Equip");
                    Console.WriteLine("3. Combine");
                    Console.WriteLine("4. Go back");
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            Console.WriteLine("What would you like to rename the item to?");
                            (selectedItem as Weapon).Rename(Console.ReadLine());
                            break;
                        case "2":
                            EquipWeapon(selectedItem as Weapon);
                            Console.WriteLine($"{selectedItem.Name} equipped.");
                            break;
                        case "3":
                            Console.WriteLine("Select another weapon to combine:");
                            int j = 0;
                            Console.WriteLine("");
                            foreach (var item in this.inventory)
                            {
                                if (item is Weapon)
                                {
                                    Console.WriteLine((j + 1) + ". " + item.Name);
                                }
                                j++;
                            }

                            var otherWeapon = Console.ReadLine();

                            Console.WriteLine("Select which attribute to keep (the rest will be determined by the average):");
                            var weaponAttributes = new Item.WeaponAttributes[] { Item.WeaponAttributes.Attack, Item.WeaponAttributes.CriticalChance, Item.WeaponAttributes.CriticalModifier };

                            var weaponAttrIndex = 0;
                            foreach (var attribute in weaponAttributes)
                            {
                                Console.WriteLine($"{weaponAttrIndex + 1}. {attribute.ToString()}");
                                weaponAttrIndex++;
                            }
                            var weaponAttrSelection = int.Parse(Console.ReadLine());
                            var selectedWeaponAttr = weaponAttributes[weaponAttrSelection - 1];


                            GainItem(InventoryHelper.CombineWeapons(selectedItem as Weapon, inventory[int.Parse(otherWeapon) - 1] as Weapon, selectedWeaponAttr));
                            inventory.Remove(selectedItem);
                            inventory.RemoveAt(int.Parse(otherWeapon) - 1);
                            continue;
                        case "4":
                            continue;
                        default:
                            Console.WriteLine("That is not a valid option.");
                            continue;
                    }
                }
            }

            Console.WriteLine("Press a key to continue.");
            Console.ReadKey();
        }
    }

    public void GainItem(Item item)
    {
        this.inventory.Add(item);
    }
}
