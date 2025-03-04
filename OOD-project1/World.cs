using System.Reflection;

namespace OOD_project1;

public static class Constants
{
    public static readonly int number_of_item_effects = 3; //1. Fire Aspect, 2. Rusty, 3. Enchanted
    public static readonly int player_stat_count = 6; //1. Health, 2. Aggresion, 3. Strength, 4. Wisdom, 5. Dexterity, 6. Luck
    public static readonly int item_stat_count = 1; //1. Damage Points
    public static readonly int gold_amount = 5; // Amount of Gold items to be placed on the map
    public static readonly int coin_amount = 10; // Amount of Coin items to be placed on the map
}



public static class Chars
{

    public static readonly char player_char = '¶';

    public static readonly Dictionary<System.Type, char> type_to_char_dict = utils.GetDerivedTypes<ITileable>()
        .Select(type =>
        {
            try
            {
                // Create an instance of the type in order to be able to access the character which is set in constructor of specific type
                var instance = Activator.CreateInstance(type);
                if (instance == null)
                    return null;

                var property = type.GetProperty("charRepresentation", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (property == null)
                    return null;

                var charValue = (char)property.GetValue(instance);
                return new { Type = type, Char = charValue };
            }
            catch
            {
                return null; 
            }
        })
        .Where(x => x != null) 
        .ToDictionary(x => x.Type, x => x.Char);
    

    public static readonly Dictionary< char,System.Type> char_to_type_dict = type_to_char_dict
        .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    
    public static readonly List<Type> weapon_effects = utils.GetDerivedTypes<WeaponEffect>();
    public static readonly char[] item_chars = type_to_char_dict
        .Where(kvp => typeof(Item).IsAssignableFrom(kvp.Key)) //Ensure it's Item
        .Select(kvp => kvp.Value) 
        .ToArray();


}




public class World
{
    private int no_items = 10;
    private ITileable[,] map;
    public int x_boundary;
    public int y_boundary;
    
    public ITileable[,] Map => map;
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


    private ITileable[,] build_map(int x, int y)
    {
        ITileable[,] grid = new ITileable[x, y];
    
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
    
        // Place Gold
        for (int i = 0; i < Constants.gold_amount; i++)
        {
            PlaceAtRandomUnoccupied(grid, new Gold(), rnd);
        }
    
        // Place Coins
        for (int i = 0; i < Constants.coin_amount; i++)
        {
            PlaceAtRandomUnoccupied(grid, new Coin(), rnd);
        }
    
        return grid;
    }
    
    // Helper Method: Places an item at a random empty tile
    private void PlaceAtRandomUnoccupied(ITileable[,] grid, ITileable cur, Random rnd)
    {
        int x, y;
        do
        {
            x = rnd.Next(1, x_boundary - 1);
            y = rnd.Next(1, y_boundary - 1);
        } while (!(grid[x, y] is Empty));
    
        grid[x, y] = cur;
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
            $"|Coins: {player.Coins} | Gold: {player.Gold}\n"+
            "|----------------------------------------|\n" +
            "|Inventory:\n" +
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
                Console.WriteLine($"  {i}. {utils.FormatItem(player.inventory[i])}");
            }
        }

        if (map[player.position[0], player.position[1]].GetType() is Item && ((Item)map[player.position[0], player.position[1]]).Pickupable)
        {
            Console.WriteLine($"To pick up {map[player.position[0], player.position[1]].ToString()} press E");
        }
        Console.WriteLine("|----------------------------------------|\n" + player.Stats.ToString());
    


        //Display Map
        for (int i = 0; i < x_boundary; i++)
        {
            for (int j = 0; j < y_boundary; j++)
            {
                ITileable currentItem = map[i, j];
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