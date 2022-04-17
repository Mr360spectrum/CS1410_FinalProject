namespace lib;

public class Game
{
    public Player player { get; set; }

    public Game(string inName)
    {
        this.player = new Player(inName);
    }
    public Game(string inName, List<Item> inventory)
    {
        this.player = new Player(inName, inventory);
    }
    //* Only for JSON deserialization
    public Game()
    {

    }

    public string GetPlayerName()
    {
        return this.player.Name;
    }

    public void Play()
    {
        // Console.Clear();
        // var sword = new Weapon("Sword of Oldness", 3, 1, 1);

        // Console.WriteLine("You found an old sword.");
        // Console.WriteLine("Press a key to pick it up.");
        // Console.ReadKey(true);
        // Console.WriteLine($"You picked up '{sword.Name}'");
        // player.GainItem(sword);

        // player.ShowInventory();
        // GameHelper.Save(this);

        // var enemy = new Enemy(Entity.EntityType.Knight);
        // Console.WriteLine("A knight approaches!");
        // while (enemy.Health > 0)
        // {
        //     Console.WriteLine($"Knight health: {enemy.Health}");
        //     Console.WriteLine("Press a key to attack!");
        //     Console.ReadKey();
        //     player.Attack(enemy);
        // }
        // Console.WriteLine("Knight defeated!");

        // GameHelper.Save(this);

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

        while (true)
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
                    var attackKey = Console.ReadKey(true).Key;
                    switch (attackKey)
                    {
                        case ConsoleKey.R:
                            Console.WriteLine("You ran away into the next area.");
                            fightingEnemy = false;
                            break;
                        case ConsoleKey.I:
                            player.ShowInventory(false);
                            Console.WriteLine($"Press enter to attack the {enemyType.ToString()}");
                            break;
                        case ConsoleKey.Enter:
                            player.Attack(enemy);
                            Console.WriteLine($"You attacked the {enemyType.ToString()}");
                            if (enemy.Health <= 0)
                            {
                                Console.WriteLine($"You defeated the {enemyType.ToString()}!");
                                fightingEnemy = false;
                                break;
                            }
                            player.TakeDamage(enemy.Damage);
                            Console.WriteLine($"The {enemyType.ToString()} attacked you!");
                            Console.WriteLine($"You have {player.Health} HP.");
                            if (player.Health <= 0)
                            {
                                Console.WriteLine("You died.");
                                System.Environment.Exit(0);
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
                Console.WriteLine("Press space to move on or press 'I' to view the inventory.");
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.I:
                        player.ShowInventory(forgeInArea);
                        break;
                    case ConsoleKey.Spacebar:
                        waiting = false;
                        break;
                    default:
                        Console.WriteLine("That is not a valid key.");
                        break;
                }
            }
        }
    }
}