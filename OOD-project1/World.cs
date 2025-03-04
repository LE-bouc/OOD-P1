using System.Reflection;

namespace OOD_project1;

public static class Constants
{
    public static readonly int number_of_item_effects = 3; //1. Fire Aspect, 2. Rusty, 3. Enchanted
    public static readonly int player_stat_count = 6; //1. Health, 2. Aggresion, 3. Strength, 4. Wisdom, 5. Dexterity, 6. Luck
    public static readonly int item_stat_count = 1; //1. Damage Points
}



public static class Chars
{
    public static readonly char  wall_char = '\u2588';
    public static readonly char empty_char = ' ';
    public static readonly char player_char = '¶';
    public static readonly char[] item_chars = { 'ℛ', 'ℳ', '\u22a1', '\u270a'};

    public static readonly Dictionary<char, System.Type> char_to_type_dict = new Dictionary<char, System.Type>()
    {
        { 'ℛ', typeof(OldRabbiScroll) },
        { 'ℳ', typeof(GreatAxe) },
        { '\u22a1', typeof(Shield) },
        { '\u270a', typeof(Sword) },
        {' ',typeof(Empty)},
        {'\u2588',typeof(Wall)}
    };
    public static readonly Dictionary<System.Type, char> type_to_char_dict = char_to_type_dict
        .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    
    
    
    
    public static readonly List<Type> weapon_effects = utils.GetDerivedTypes<WeaponEffect>();
    
}




public class World
{
    private int no_items = 10;
    private Item[,] map;
    public int x_boundary;
    public int y_boundary;
    
    public Item[,] Map => map;
    public int X_boundary => x_boundary;
    public int Y_boundary => y_boundary;
    public Player player;
    
    
    
    public World(int x_boundary, int y_boundary)
    {
        this.x_boundary = x_boundary;
        this.y_boundary = y_boundary;
        map = build_map(x_boundary, y_boundary);
        player = new Player(this);
    }


    private Item[,] build_map(int x, int y)
    {
        Item[,] grid = new Item[x, y];

        // Set Empty Spaces
        for (int i = 1; i < x - 1; i++)
        {
            for (int j = 1; j < y - 1; j++)
            {
                grid[i, j] = new Empty();
            }
        }

        // Set vertical walls
        for (int j = 0; j < y; j++)
        {
            grid[0, j] = new Wall();        
            grid[x - 1, j] = new Wall();    
        }

        // Set horizontal walls
        for (int i = 0; i < x; i++)
        {
            grid[i, 0] = new Wall();        
            grid[i, y - 1] = new Wall();    
        }

        // Create Entrance
        grid[0, 0] = new Empty();
        grid[1, 0] = new Empty();

        // Placing Items
        Random rnd = new Random();
        for (int i = 0; i < this.no_items; i++)
        {   
            // Get Some random char at some place x,y
            int type_it = rnd.Next(0, Chars.item_chars.Length); 
            int x_it = rnd.Next(1, x_boundary - 1);
            int y_it = rnd.Next(1, y_boundary - 1);

            // Get the Type corresponding to the selected char
            Type itemType = Chars.char_to_type_dict[Chars.item_chars[type_it]];
        
            // Create an instance of the selected item, while navigating the parameterless vs parametered constructors of Item and its derived classes
            Item newItem;
            ConstructorInfo constructor = itemType.GetConstructor(new Type[] { typeof(string) });

            if (constructor != null)
            {
                newItem = (Item)Activator.CreateInstance(itemType, "Generated Item");
            }
            else
            {
                newItem = (Item)Activator.CreateInstance(itemType);
            }

            
            
            
            
            // Apply from 0 to 3 effects iff item is a weapon
            if (newItem is Weapon)
            {
                int decorator_count = 0;
                int count_val = rnd.Next(1, 11);
                if (count_val > 4)
                {
                    decorator_count = count_val <= 7 ? 1 : (count_val == 10 ? 3 : 2);
                }
                //Track which decorators are already applied to eliminate duplicates
                List<Type> decorators = new List<Type>(Chars.weapon_effects);
                for (int d = 0; d < decorator_count; d++)
                {
                    if (decorators.Count == 0) break;

                    Type decorator = decorators[rnd.Next(0, decorators.Count)];
                    decorators.Remove(decorator);
                    newItem = (Item)Activator.CreateInstance(decorator, newItem);
                }
            }
            
            
            // Place the item on the map
            grid[x_it, y_it] = newItem;
        }

        return grid;
    }



    
    public void Display()
    {
        Console.Clear();
        //Display player info
        
        Console.WriteLine(
            "Information about the player:\n" +
            "|----------------------------------------|\n" +  
            $"|   Position: ({player.position[0],3}, {player.position[1],3})                 |\n" +  
            "|----------------------------------------|\n" +
            $"| Hand 1: {utils.FormatItem(player.H1),-10} | Hand 2: {utils.FormatItem(player.H2),-10}|\n" +
            "|----------------------------------------|\n" +
            "Inventory:\n" +
            "|----------------------------------------|\n"
        );

        // Loop through the inventory and print each item
        if (player.inventory.Count == 0)
        {
            Console.WriteLine("  (Empty Inventory)");
        }
        else
        {
            for (int i = 0; i < player.inventory.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {utils.FormatItem(player.inventory[i])}");
            }
        }

        Console.WriteLine("|----------------------------------------|\n" + player.Stats.ToString());
    


        //Display Map
        for (int i = 0; i < x_boundary; i++)
        {
            for (int j = 0; j < y_boundary; j++)
            {
                Item currentItem = map[i, j];
                //If item is weapon with decorators, undress it
                while (currentItem is WeaponEffect weaponEffect)
                {
                    currentItem = ((WeaponEffect)weaponEffect)._baseWeapon;
                }

                char c = Chars.type_to_char_dict[ currentItem.GetType() ];
                Console.Write(player.position[0] == i && player.position[1] == j ? Chars.player_char : c);
            }
            Console.WriteLine();
        }
    }
    
    
}