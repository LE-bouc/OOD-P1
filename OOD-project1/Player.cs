using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;

namespace OOD_project1;

public class Player
{
    public int[] position = { 0, 0 };
    private World world;
    public List<Item> inventory = new List<Item>();
    
    public Dictionary<System.Type, int> currency_dict;
    public int Coins
    {
        get => currency_dict[typeof(Coin)];
        set => currency_dict[typeof(Coin)] = value;
    }

    public int Gold
    {
        get => currency_dict[typeof(Gold)];
        set => currency_dict[typeof(Gold)] = value;
    }

    private Player_Stats stats;
    public Player_Stats Stats
    {
        get => stats;
        protected set => stats = value;
    }


    private Item[] hands = new Item[2];
    public Item H1 => hands[0];
    public Item H2 => hands[1];
    public int H => stats.health;
    public int P => stats.strength;
    public int W => stats.wisdom;
    public int A => stats.aggression;
    public int L => stats.luck;
    public int D => stats.dexterity;

    public Player(World w)
    {
        stats = new Player_Stats();
        stats.strength = stats.dexterity = stats.health = stats.aggression = stats.wisdom = 5;
        world = w;
        
        currency_dict = utils.GetDerivedTypes<Currency>().ToDictionary(type => type, type => 0);
    }


    public void Move(int delta_x, int delta_y)
    {
        int new_x = position[0] + delta_x;
        int new_y = position[1] + delta_y;

        // Check boundaries
        if (new_x >= 0 && new_x < world.X_boundary &&
            new_y >= 0 && new_y < world.Y_boundary
           )
        {
            if (world.Map[new_x, new_y] is Wall )
            {
                return;
            }
        }
        position[0] = new_x;
        position[1] = new_y;
        if (world.Map[new_x, new_y] is Currency)
        {
            currency_dict[ world.Map[new_x, new_y].GetType() ]++;
        }
    }


    public void PickUp()
    {
        ITileable it = world.Map[position[0], position[1]];
        if ( it is Item )
        {
            inventory.Add((Item)it);
        }
        world.Map[position[0], position[1]] = new Empty();
        
    }

    public void Equip(int idx)
    {
        if (inventory.Count <= idx)
        {
            return;
        }
        Item new_item = inventory[idx];
        if (new_item.twohanded)
        {
            EquipHandler(new_item);
        }
        Console.Clear(); // Temporarily hide map or other displays to allow for user input
        Console.WriteLine("Where do you want to equip this item?");
        Console.WriteLine("[L] - Left Hand");
        Console.WriteLine("[R] - Right Hand");
        Console.WriteLine("[U] - Unequip");
        Console.WriteLine("[N] - Cancel");

        char choice = char.ToUpper(Console.ReadKey(true).KeyChar);
        switch (choice)
        {
            case 'L': // left hand
                EquipHandler(new_item, 0); 
                Console.WriteLine("Item equipped to the Left Hand!");
                break;
            case 'R': // right hand
                EquipHandler(new_item, 1); 
                Console.WriteLine("Item equipped to the Right Hand!");
                break;
            case 'N': // cancel
                Console.WriteLine("Equip action cancelled.");
                return;
            case 'U': //unequip
                if(  world.Map[ position[0], position[1]].GetType() == typeof(Empty)){
                    Console.WriteLine("Unequipping.");
                    UnequipHandler(idx);
                    return;
                }
                Console.WriteLine("Unable to equip, tile not empty.");
                break;
                
            default: 
                Console.WriteLine("Invalid input. Equip action cancelled.");
                return;
        }
        
    }

    private void EquipHandler(Item new_item, int idx = -1)
    {
        //If equipping two-hander
        if (idx == -1)
        {
            if (new_item.twohanded)
            {
                foreach (Item item in hands)
                {
                    if (item != null)
                    {
                        stats.Remove(item.player_effects);
                        inventory.Add(item);
                    }
                }

                hands[0] = hands[1] = new_item;
                stats.Add(new_item.player_effects);
                inventory.Remove(new_item);
                return;
            }
            else
                throw new Exception("One handed items must have corresponding index\n");
        }

        if (hands[idx] == null)
        {
            hands[idx] = new_item;
            inventory.Remove(new_item);
            stats.Add(new_item.player_effects);
        }
        else
        {
            inventory.Add(hands[idx]);
            stats.Remove(hands[idx].player_effects);
            inventory.Remove(new_item);
            stats.Add(new_item.player_effects);

        }
    }

    private void UnequipHandler(int idx)
    {
        inventory.RemoveAt(idx);
    }
    
}

public class Player_Stats
{
    public  int health;
    public  int aggression;
    public  int strength;
    public  int wisdom;
    public  int dexterity;
    public  int luck;


    public Player_Stats()
    {
        health = aggression = strength = wisdom = dexterity = luck= 0;
    }

    public void Add(Player_Stats p)
    {
        this.health += p.health;
        this.aggression += p.aggression;
        this.strength += p.strength;
        this.wisdom += p.wisdom;
        this.dexterity += p.dexterity;
        this.luck += p.luck;
    }

    public void Remove(Player_Stats p)
    {
        this.health -= p.health;
        this.aggression -= p.aggression;
        this.strength -= p.strength;
        this.wisdom -= p.wisdom;
        this.dexterity -= p.dexterity;
        this.luck -= p.luck;
    }

    public static Player_Stats operator +(Player_Stats a, Player_Stats b)
    {
        a.Add(b);
        return a;
    }

    public static Player_Stats operator -(Player_Stats a, Player_Stats b)
    {
        a.Remove(b);
        return a;
    }
    public override string ToString()
    {
        return string.Format(
            "| HP:  {0,-5} | Agg: {1,-5} | Str: {2,-5}   |\n" +
            "| Wis: {3,-5} | Dex: {4,-5} | Luck: {5,-5}  |\n" +
            "|----------------------------------------|",
            health, aggression, strength, wisdom, dexterity, luck
        );
    }


    

}
