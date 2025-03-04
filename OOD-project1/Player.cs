using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;

namespace OOD_project1;

public class Player
{
    public int[] position = { 0, 0 };
    private World world;
    List<Item> inventory = new List<Item>();
    
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
    }


    public void PickUp()
    {
        Item it = world.Map[position[0], position[1]];
        if ( ! ( it is Wall || it is Empty) )
        {
            inventory.Add(it);
        }
        
    }

    private void Equip(Item new_item, int idx = -1)
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
        
        if (hands[idx] == null)
        {
            hands[idx] = it;
            stats.Add(it.player_effects);
            return;
        }
        
        if(it.)
        
    }
    private void Equip(Item new_item)
    {

        if (hands[0] == null)
        {
            hands[0] = new_item;
            if (new_item.twohanded)
            {
                hands[1] = new_item;
            }
            stats.Add(new_item.player_effects);
            return;
        }

        
        if (new_item.twohanded)
        {
            if (hands[0].twohanded)
            {
                stats.Remove(hands[0].player_effects);
            }
            else
            {
                stats.Remove(hands[0].player_effects);
                stats.Remove(hands[1].player_effects);
            }
            hands[0] = new_item;
            hands[1] = new_item;
        }
        else
        {
            stats.Remove(hands[0].player_effects);
            if (hands[0].twohanded)
            {
                hands[0] = new_item;
            }
            else
            {
                hands[0] = hands[1];
                hands[1] = new_item;
            }
        }
        stats.Add(new_item.player_effects);
        return;
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
