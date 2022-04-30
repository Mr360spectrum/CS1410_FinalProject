using System;
using lib;
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

        /// <summary>
        /// Continues to ask the user for an integer until the input is valid
        /// and is within the given range.
        /// </summary>
        /// <param name="min">The lower bound for the integer.</param>
        /// <param name="max">The upper bound for the integer.</param>
        /// <returns>A valid integer within the given range.</returns>
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

        /// <summary>
        /// Continues to ask the user for a string until the input is not an 
        /// empty string or null.
        /// </summary>
        /// <returns>A non-empty or null string.</returns>
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

        /// <summary>
        /// Displays each item in the player's inventory and allows the user to rename,
        /// equip, and combine items (if a forge is available) depending on the item type.
        /// </summary>
        /// <param name="forgeInArea">Represents whether a forge is in the current area. If true,
        /// the options to combine weapons, armor, or crafting items will be available.</param>
        /// <param name="player">The Player object representing the current player and all members
        /// that are part of the object. Used to access the player's inventory.</param>
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

                // int selection = GetSelectionFromNavigableInventory(player);

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
                                int armorCount = 0;
                                foreach (var item in inventory)
                                {
                                    if (item is Armor)
                                    {
                                        armorCount++;
                                    }
                                }
                                //Go back if there is no other armor piece to combine
                                if (armorCount < 2)
                                {
                                    Console.WriteLine("There are no other items to combine.");
                                    break;
                                }
                                Console.WriteLine("Select another armor piece to combine:");
                                int j = 0;
                                Console.WriteLine("");
                                List<int> armorIndices = new();
                                foreach (var item in inventory)
                                {
                                    if (item is Armor && (item as Armor) != selectedItem as Armor)
                                    {
                                        Console.WriteLine((j + 1) + ". " + item.Name);
                                        armorIndices.Add(j);
                                    }
                                    j++;
                                }

                                int otherArmorIndex;
                                bool isAvailableArmorIndex = false;
                                do
                                {
                                    otherArmorIndex = GetInt(1, inventory.Count) - 1;
                                    foreach (var index in armorIndices)
                                    {
                                        if (otherArmorIndex == index)
                                        {
                                            isAvailableArmorIndex = true;
                                            break;
                                        }
                                    }
                                    if (!isAvailableArmorIndex)
                                    {
                                        Console.WriteLine("That is not a valid option.");
                                    }
                                } while (!isAvailableArmorIndex);

                                Console.WriteLine("Select which attribute to keep (the rest will be determined by the average):");
                                var armorAttributes = new Item.ArmorAttributes[] { Item.ArmorAttributes.AttackBonus, Item.ArmorAttributes.Defense, Item.ArmorAttributes.DodgeChance };

                                var armorAttrIndex = 0;
                                foreach (var attribute in armorAttributes)
                                {
                                    Console.WriteLine($"{armorAttrIndex + 1}. {attribute.ToString()}");
                                    armorAttrIndex++;
                                }
                                var armorAttrSelection = GetInt(1, 3) - 1;
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
                                int weaponCount = 0;
                                foreach (var item in inventory)
                                {
                                    if (item is Weapon)
                                    {
                                        weaponCount++;
                                    }
                                }
                                //Go back if there is no other weapon to combine
                                if (weaponCount < 2)
                                {
                                    Console.WriteLine("There are no other items to combine.");
                                    break;
                                }
                                Console.WriteLine("Select another weapon to combine:");
                                int j = 0;
                                Console.WriteLine("");
                                List<int> weaponIndices = new();
                                foreach (var item in inventory)
                                {
                                    if (item is Weapon && (item as Weapon) != selectedItem as Weapon)
                                    {
                                        Console.WriteLine((j + 1) + ". " + item.Name);
                                        weaponIndices.Add(j);
                                    }
                                    j++;
                                }

                                int otherWeaponIndex;
                                bool isAvailableWeaponIndex = false;
                                do
                                {
                                    otherWeaponIndex = GetInt(1, inventory.Count) - 1;
                                    foreach (var index in weaponIndices)
                                    {
                                        if (otherWeaponIndex == index)
                                        {
                                            isAvailableWeaponIndex = true;
                                            break;
                                        }
                                    }
                                    if (!isAvailableWeaponIndex)
                                    {
                                        Console.WriteLine("That is not a valid option.");
                                    }
                                } while (!isAvailableWeaponIndex);

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

        /// <summary>
        /// Handles gameplay. The existence of enemies, chests, and forges in each area is determined
        /// by random number generation.
        /// </summary>
        /// <param name="game">The Game object that represents the game that was loaded in, whether it
        /// is a new game or an existing save.</param>
        /// <param name="helper">The GameHelper object that will handle saving the current game.</param>
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

            //If the player chooses to exit, "shouldContinue" will be set to false and the loop will no longer run.
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
                    Console.WriteLine("Viewing the inventory or entering an invalid input will result in you taking damage!");

                    //Continue running until the enemy dies, the player dies, or if the player chooses to run.
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
                                helper.Save(game);
                                Console.Clear();
                                Console.WriteLine($"Press 'R' to run, 'I' to view the inventory, or enter to attack the {enemyType.ToString()}!");
                                break;
                            case ConsoleKey.Enter:
                                Console.Clear();
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
                                Console.WriteLine($"Press 'R' to run, 'I' to view the inventory, or enter to attack the {enemyType.ToString()}!");
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

                //Continue to run until the player moves on or exits to the main menu.
                var waiting = true;
                while (waiting)
                {
                    Console.WriteLine("Press space to move on, press Escape to return to the main menu, or press 'I' to view the inventory.");
                    var key = Console.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.I:
                            ShowInventory(forgeInArea, player);
                            helper.Save(game);
                            break;
                        case ConsoleKey.Spacebar:
                            waiting = false;
                            break;
                        case ConsoleKey.Escape:
                            waiting = false;
                            shouldContinue = false;
                            helper.Save(game);
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("That is not a valid key.");
                            break;
                    }
                }

                //Allow the player to retry saving. If the player chooses to restart instead, exit
                //back to the main menu.
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

        /// <summary>
        /// Displays the inventory with a menu similar to that used for the main menu
        /// that can be navigated with the arrow keys.
        /// </summary>
        /// <param name="Inventory">The inventory (list of type Item) to be displayed.</param>
        static int GetSelectionFromNavigableInventory(Player player)
        {
            List<Item> inventory = player.Inventory;
            int cursorPos = 0;
            int maxPos = inventory.Count - 1;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = GameHelper.DefaultColor;
                Console.WriteLine("Use the arrow keys to navigate and enter to select.");
                Console.WriteLine("Select which item you'd like to view options for: ");
                foreach (var item in inventory)
                {
                    if (inventory[cursorPos] == item)
                    {
                        Console.ForegroundColor = GameHelper.HighlightColor;
                        Console.WriteLine(" > " + item.Name + (item is Weapon && (item as Weapon).Equals(player.EquippedWeapon) ? " [Equipped]" :
                            (item is Armor && (item as Armor).Equals(player.EquippedArmor) ? " [Equipped]" : "")));
                        Console.ForegroundColor = GameHelper.DefaultColor;
                    }
                    else
                    {
                        Console.WriteLine($"   {item.Name}");
                    }

                    if (item is Weapon)
                    {
                        Console.WriteLine((item as Weapon).ToString());
                    }
                    else if (item is Armor)
                    {
                        Console.WriteLine((item as Armor).ToString());
                    }
                }

                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (cursorPos == maxPos)
                        {
                            cursorPos = 0;
                        }
                        else
                        {
                            cursorPos++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (cursorPos == 0)
                        {
                            cursorPos = maxPos;
                        }
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

        /// <summary>
        /// Displays the saves that were found in the "saves" folder at the project root 
        /// and allows the user to choose which one to load from. If there are none, the
        /// user will be asked to create a new save.
        /// </summary>
        /// <param name="inHelper">The GameHelper object that will handle loading in the 
        /// selected game.</param>
        /// <returns>A Game object with the information retrieved from the save files. If
        /// a new game was created instead, returns a Game object returned by StartNewGame().</returns>
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

        /// <summary>
        /// Creates a new Game object after asking the user for a name. The Player object in this
        /// Game object will be given the name that the player provides.
        /// </summary>
        /// <returns>Returns a new Game object that contains a Player object with the name 
        /// provided by the user.</returns>
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
            name = GetName();
            return new Game(name);
        }

        /// <summary>
        /// Displays a menu with a logo and message at the top of the console. Iterates through 
        /// the list of options provided and allows the user to navigate using the arrow and enter keys.
        /// </summary>
        /// <param name="logo">The logo/design to be displayed at the top of the console.</param>
        /// <param name="options">Each option to be available for selection.</param>
        /// <param name="message">The string to be displayed above the options (empty by default).</param>
        /// <returns>The index of the selected option.</returns>
        static int DisplayMenu(string logo, List<string> options, string message = "")
        {
            int cursorPos = 0;
            int maxPos = options.Count - 1;

            while (true)
            {
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

        /// <summary>
        /// Displays the main menu. Allows the user to start a new save, load an existing save,
        /// learn how to play, or exit the program.
        /// </summary>
        /// <param name="inHelper">The GameHelper object that will handle saving and loading.</param>
        /// <returns>A Game object that is either new or loaded in from an existing save.</returns>
        static Game GetGame(GameHelper inHelper)
        {
            var options = new List<string>() { "New Game", "Load Game", "How To Play", "Exit" };

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
                        DisplayInstructions();
                        Console.WriteLine("\nPress enter to go back to the menu.");
                        Console.ReadLine();
                        continue;
                    case 3:
                        System.Environment.Exit(0);
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the contents of help.txt.
        /// </summary>
        private static void DisplayInstructions()
        {
            Console.Clear();
            try
            {
                Console.WriteLine(File.ReadAllText("help.txt"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an issue loading 'help.txt.':");
                Console.WriteLine(ex.Message);
            }
        }
    }
}