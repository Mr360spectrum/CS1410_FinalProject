using System;
using lib;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) //Allows the player to restart in case saving fails
            {
                GameHelper helper = new GameHelper(new OnDiskGameStorageService());
                try
                {
                    Game game = GetGame(helper);
                    Play(game, helper);
                }
                catch
                {
                    Console.WriteLine("There was an error loading the game.");
                    Console.WriteLine("Press enter to go back to the main menu.");
                    Console.ReadLine();
                }
            }
        }

        static int GetInt(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine();
                try
                {
                    int parsedInput = int.Parse(input);
                    if (parsedInput > max || parsedInput < min)
                    {
                        Console.WriteLine("That input is out of range.");
                        continue;
                    }
                    return parsedInput;
                }
                catch
                {
                    Console.WriteLine("That is not a valid input.");
                    continue;
                }
            }
        }

        static string GetName()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "" || input is null)
                {
                    Console.WriteLine("Please enter a valid name.");
                    continue;
                }

                return input;
            }
        }

        static void ShowInventory(bool forgeInArea, Player player)
        {
            var inventory = player.Inventory;
            while (true)
            {
                Console.Clear();
                if (player.Inventory.Count == 0)
                {
                    Console.WriteLine("The inventory is empty.");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    return;
                }

                int i = 0;
                Console.WriteLine("");
                foreach (var item in player.Inventory)
                {
                    Console.WriteLine((i + 1) + ". " + item.Name + (item is Weapon && (item as Weapon).Equals(player.EquippedWeapon) ? " [Equipped]" :
                        (item is Armor && (item as Armor).Equals(player.EquippedArmor) ? " [Equipped]" : "")));
                    if (item is Weapon)
                    {
                        Console.WriteLine((item as Weapon).ToString());
                    }
                    else if (item is Armor)
                    {
                        Console.WriteLine((item as Armor).ToString());
                    }

                    i++;
                }

                Console.WriteLine("\nEnter an empty string to exit.");
                Console.WriteLine("Select which item you'd like to view options for: ");
                int selection;
                //Ensure that input can be parsed and is between 1 and inventory.Count
                while (true)
                {
                    string input = Console.ReadLine();

                    if (input == "")
                    {
                        return;
                    }

                    try
                    {
                        selection = int.Parse(input);
                        if (selection > inventory.Count || selection < 1)
                        {
                            Console.WriteLine("That input is out of range.");
                            continue;
                        }
                        selection -= 1;
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("That is not a valid input.");
                        continue;
                    }
                }

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
                        int option = GetInt(1, 3);
                        switch (option)
                        {
                            case 1:
                                Console.WriteLine("What would you like to rename the item to?");
                                (selectedItem as Armor).Rename(GetName());
                                break;
                            case 2:
                                player.EquipArmor(selectedItem as Armor);
                                Console.WriteLine($"{selectedItem.Name} equipped.");
                                break;
                            case 3:
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
                        var option = GetInt(1, 3);
                        switch (option)
                        {
                            case 1:
                                Console.WriteLine("What would you like to rename the item to?");
                                (selectedItem as Weapon).Rename(GetName());
                                break;
                            case 2:
                                player.EquipWeapon(selectedItem as Weapon);
                                Console.WriteLine($"{selectedItem.Name} equipped.");
                                break;
                            case 3:
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
                        Console.WriteLine("Select an option:");
                        Console.WriteLine("1. Forge Weapon (uses two sticks)");
                        Console.WriteLine("2. Forge Armor (uses two sticks)");
                        Console.WriteLine("3. Go back");
                        int option = GetInt(1, 3);
                        switch (option)
                        {
                            case 1:
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
                                    player.GainItem(InventoryHelper.ForgeWeapon("Forged Weapon"));
                                }
                                else
                                {
                                    Console.WriteLine("You don't have enough sticks!");
                                }
                                break;
                            case 2:
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
                                    player.GainItem(InventoryHelper.ForgeArmor("Forged Armor"));
                                }
                                else
                                {
                                    Console.WriteLine("You don't have enough sticks!");
                                }
                                break;
                            case 3:
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
                        var option = GetInt(1, 4);
                        switch (option)
                        {
                            case 1:
                                Console.WriteLine("What would you like to rename the item to?");
                                (selectedItem as Armor).Rename(GetName());
                                break;
                            case 2:
                                player.EquipArmor(selectedItem as Armor);
                                Console.WriteLine($"{selectedItem.Name} equipped.");
                                break;
                            case 3:
                                Console.WriteLine("Select another armor piece to combine:");
                                int j = 0;
                                Console.WriteLine("");
                                foreach (var item in inventory)
                                {
                                    if (item is Armor && (item as Armor) != selectedItem as Armor)
                                    {
                                        Console.WriteLine((j + 1) + ". " + item.Name);
                                    }
                                    j++;
                                }

                                int otherArmorIndex = GetInt(1, inventory.Count) - 1;

                                Console.WriteLine("Select which attribute to keep (the rest will be determined by the average):");
                                var armorAttributes = new Item.ArmorAttributes[] { Item.ArmorAttributes.AttackBonus, Item.ArmorAttributes.Defense, Item.ArmorAttributes.DodgeChance };

                                var armorAttrIndex = 0;
                                foreach (var attribute in armorAttributes)
                                {
                                    Console.WriteLine($"{armorAttrIndex + 1}. {attribute.ToString()}");
                                    armorAttrIndex++;
                                }
                                var armorAttrSelection = GetInt(1, 3);
                                var selectedArmorAttr = armorAttributes[armorAttrSelection];

                                var otherArmor = inventory[otherArmorIndex] as Armor;

                                player.GainItem(InventoryHelper.CombineArmor(selectedItem as Armor, otherArmor, selectedArmorAttr));
                                inventory.Remove(selectedItem);
                                inventory.Remove(otherArmor);
                                continue;
                            case 4:
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
                        int option = GetInt(1, 4);
                        switch (option)
                        {
                            case 1:
                                Console.WriteLine("What would you like to rename the item to?");
                                (selectedItem as Weapon).Rename(GetName());
                                break;
                            case 2:
                                player.EquipWeapon(selectedItem as Weapon);
                                Console.WriteLine($"{selectedItem.Name} equipped.");
                                break;
                            case 3:
                                Console.WriteLine("Select another weapon to combine:");
                                int j = 0;
                                Console.WriteLine("");
                                foreach (var item in inventory)
                                {
                                    if (item is Weapon && (item as Weapon) != selectedItem as Weapon)
                                    {
                                        Console.WriteLine((j + 1) + ". " + item.Name);
                                    }
                                    j++;
                                }

                                int otherWeaponIndex = GetInt(1, inventory.Count) - 1;

                                Console.WriteLine("Select which attribute to keep (the rest will be determined by the average):");
                                var weaponAttributes = new Item.WeaponAttributes[] { Item.WeaponAttributes.Attack, Item.WeaponAttributes.CriticalChance, Item.WeaponAttributes.CriticalModifier };

                                var weaponAttrIndex = 0;
                                foreach (var attribute in weaponAttributes)
                                {
                                    Console.WriteLine($"{weaponAttrIndex + 1}. {attribute.ToString()}");
                                    weaponAttrIndex++;
                                }
                                var weaponAttrSelection = GetInt(1, 3);
                                var selectedWeaponAttr = weaponAttributes[weaponAttrSelection - 1];

                                var otherWeapon = inventory[otherWeaponIndex];

                                player.GainItem(InventoryHelper.CombineWeapons(selectedItem as Weapon, otherWeapon as Weapon, selectedWeaponAttr));
                                inventory.Remove(selectedItem);
                                inventory.Remove(otherWeapon);
                                continue;
                            case 4:
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

        static void Play(Game game, GameHelper helper)
        {
            Player player = game.player;
            if (player.Inventory.Count == 0)
            {
                Console.Clear();
                //First area
                var startingSword = new Weapon("Sword of Oldness", 3, 2, 1);
                Console.WriteLine("You wake up in a forest.");
                Console.WriteLine("You have no idea where you are, but you see a sword laying on the ground nearby.");
                Console.WriteLine("Press a key to pick it up.");
                Console.ReadKey(true);
                Console.WriteLine($"You picked up '{startingSword.Name}'.");
                player.GainItem(startingSword);
                player.EquipWeapon(startingSword);
                Console.WriteLine("Press a key to move forward.");
                Console.ReadKey(true);
            }

            bool shouldContinue = true;
            while (shouldContinue)
            {
                Console.Clear();
                Console.WriteLine("You moved to a new area.");
                var rand = new Random();
                var chestChance = rand.Next(1, 101);
                var forgeChance = rand.Next(1, 101);
                var enemyChance = rand.Next(1, 101);

                bool enemyInArea = enemyChance <= 30;
                bool chestInArea = chestChance <= 40;
                bool forgeInArea = forgeChance <= 60;

                var enemyTypes = new Entity.EntityType[] { Entity.EntityType.Wolf, Entity.EntityType.Knight, Entity.EntityType.Wizard };

                if (enemyInArea)
                {
                    var enemyType = enemyTypes[rand.Next(0, enemyTypes.Length)];
                    var enemy = new Enemy(enemyType);
                    Console.WriteLine($"There's a {enemyType.ToString()} in the area!");
                    Console.WriteLine("Press 'R' to run, 'I' to view the inventory, or 'enter' to attack!");

                    bool fightingEnemy = true;
                    while (fightingEnemy)
                    {
                        player.TakeDamage(enemy.Damage);
                        Console.WriteLine($"The {enemyType.ToString()} attacked you!");
                        Console.ForegroundColor = GameHelper.HighlightColor;
                        int playerHealth = player.Health >= 0 ? player.Health : 0;
                        Console.WriteLine($"You have {playerHealth} HP.");
                        Console.ForegroundColor = GameHelper.DefaultColor;
                        if (player.Health <= 0)
                        {
                            Console.WriteLine("You died.");
                            System.Environment.Exit(0);
                        }
                        var attackKey = Console.ReadKey(true).Key;
                        switch (attackKey)
                        {
                            case ConsoleKey.R:
                                Console.WriteLine("You ran away into the next area.");
                                fightingEnemy = false;
                                break;
                            case ConsoleKey.I:
                                ShowInventory(false, player);
                                Console.WriteLine($"Press enter to attack the {enemyType.ToString()}");
                                break;
                            case ConsoleKey.Enter:
                                if (player.EquippedWeapon is null)
                                {
                                    Console.ForegroundColor = GameHelper.HighlightColor;
                                    Console.WriteLine("You don't have a weapon equipped!");
                                    Console.ForegroundColor = GameHelper.DefaultColor;
                                }
                                player.Attack(enemy);
                                Console.WriteLine($"You attacked the {enemyType.ToString()}");
                                if (enemy.Health <= 0)
                                {
                                    Console.WriteLine($"You defeated the {enemyType.ToString()}!");
                                    fightingEnemy = false;
                                    break;
                                }
                                break;
                        }
                    }
                }

                if (chestInArea)
                {
                    Console.WriteLine("There's a chest in this area.");
                    Console.WriteLine("Press enter to open it.");
                    Console.ReadKey(true);
                    var itemChance = rand.Next(1, 101);
                    if (itemChance <= 33)
                    {
                        var weaponInChest = InventoryHelper.CreateNewWeaponInChest();
                        Console.WriteLine("There's a weapon in the chest!");
                        Console.WriteLine("Press enter to take it.");
                        Console.ReadKey(true);
                        player.GainItem(weaponInChest);
                    }
                    else if (itemChance > 33 && itemChance <= 66)
                    {
                        Console.WriteLine("There's a stick in the chest!");
                        Console.WriteLine("Press enter to take it.");
                        Console.ReadKey(true);
                        player.GainItem(new CraftingItem("Stick"));
                    }
                    else
                    {
                        var armorInChest = InventoryHelper.CreateNewArmorInChest();
                        Console.WriteLine("There's an armor piece in the chest!");
                        Console.WriteLine("Press enter to take it.");
                        Console.ReadKey(true);
                        player.GainItem(armorInChest);
                    }
                }

                if (forgeInArea)
                {
                    Console.WriteLine("There's a forge in the area!");
                    Console.WriteLine("While here, you can combine and forge weapons in the inventory.");
                }

                var waiting = true;
                while (waiting)
                {
                    Console.WriteLine("Press space to move on, press Escape to return to the main menu, or press 'I' to view the inventory.");
                    var key = Console.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.I:
                            ShowInventory(forgeInArea, player);
                            break;
                        case ConsoleKey.Spacebar:
                            waiting = false;
                            break;
                        case ConsoleKey.Escape:
                            waiting = false;
                            shouldContinue = false;
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("That is not a valid key.");
                            break;
                    }
                }

                while (true)
                {
                    try
                    {
                        helper.Save(game);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Console.WriteLine("An error occurred while saving.");
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Press R to restart the program, or press another key to retry saving.");

                        var restartKey = Console.ReadKey().Key;
                        switch (restartKey)
                        {
                            case (ConsoleKey.R):
                                return;
                            default:
                                continue;
                        }
                    }
                }
            }
        }

        static Game LoadGameFromMenu(GameHelper inHelper)
        {
            var loadLogo = @"
  _      ____          _____  
 | |    / __ \   /\   |  __ \ 
 | |   | |  | | /  \  | |  | |
 | |   | |  | |/ /\ \ | |  | |
 | |___| |__| / ____ \| |__| |
 |______\____/_/    \_\_____/ 
                              
";
            var fileOptions = GameHelper.GetSaves();
            if (fileOptions.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("There are no saved games.");
                Console.WriteLine("Press a key to create a new game.");
                Console.ReadKey();
                return StartNewGame();
            }
            var nameSelection = DisplayMenu(loadLogo, fileOptions, "Select a saved game.");

            return inHelper.Load(fileOptions[nameSelection]);
        }

        static Game StartNewGame()
        {
            Console.Clear();
            Console.WriteLine(@"
  _   _ ________          __
 | \ | |  ____\ \        / /
 |  \| | |__   \ \  /\  / / 
 | . ` |  __|   \ \/  \/ /  
 | |\  | |____   \  /\  /   
 |_| \_|______|   \/  \/    

");
            Console.WriteLine("Starting a new game...");
            Console.WriteLine("What's your name?");
            string name;
            while (true)
            {
                name = GetName();
                if (name == null || name == "")
                {
                    Console.WriteLine("That is an invalid name.");
                    continue;
                }
                return new Game(name);
            }
        }

        static int DisplayMenu(string logo, List<string> options, string message = "")
        {
            int cursorPos = 0;
            int maxPos = options.Count - 1;

            while (true)
            {
                //line instead of clearing the whole screen to prevent flickering
                Console.Clear();
                Console.ForegroundColor = GameHelper.DefaultColor;
                Console.WriteLine(logo);
                Console.WriteLine("Use the arrow keys to navigate and enter to select.");
                if (message != "")
                {
                    Console.WriteLine(message);
                }
                foreach (var option in options)
                {
                    if (options[cursorPos] == option)
                    {
                        Console.ForegroundColor = GameHelper.HighlightColor;
                        Console.WriteLine($" > {option} <");
                        Console.ForegroundColor = GameHelper.DefaultColor;
                    }
                    else
                    {
                        Console.WriteLine($"   {option}");
                    }
                }

                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        //Loop back to top if "down" is pressed while cursor is at bottom
                        if (cursorPos == maxPos)
                        {
                            cursorPos = 0;
                        }
                        //Move down
                        else
                        {
                            cursorPos++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        //Loop back to bottom if "up" is pressed while cursor is at top
                        if (cursorPos == 0)
                        {
                            cursorPos = maxPos;
                        }
                        //Move up
                        else
                        {
                            cursorPos--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        return cursorPos;
                }
            }
        }

        static Game GetGame(GameHelper inHelper)
        {
            var options = new List<string>() { "New Game", "Load Game", "Exit" };

            while (true)
            {
                Console.Clear();
                string logo = @"
      :::    :::  ::::::::   :::     :::      :::      :::::::::   ::::    :::      :::  
     :+:   :+:  :+:    :+:  :+:     :+:    :+: :+:    :+:    :+:  :+:+:   :+:    :+: :+: 
    +:+  +:+   +:+    +:+  +:+     +:+   +:+   +:+   +:+    +:+  :+:+:+  +:+   +:+   +:+ 
   +#++:++    +#+    +:+  +#+     +:+  +#++:++#++:  +#++:++#:   +#+ +:+ +#+  +#++:++#++: 
  +#+  +#+   +#+    +#+   +#+   +#+   +#+     +#+  +#+    +#+  +#+  +#+#+#  +#+     +#+  
 #+#   #+#  #+#    #+#    #+#+#+#    #+#     #+#  #+#    #+#  #+#   #+#+#  #+#     #+#   
###    ###  ########       ###      ###     ###  ###    ###  ###    ####  ###     ###   

";
                int selection = DisplayMenu(logo, options);
                switch (selection)
                {
                    case 0:
                        return StartNewGame();
                    case 1:
                        return LoadGameFromMenu(inHelper);
                    case 2:
                        System.Environment.Exit(0);
                        break;
                }
            }
        }
    }


}