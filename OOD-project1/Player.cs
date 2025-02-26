using System.Collections.Concurrent;

namespace OOD_project1;

public class Player
{
    public int[] position = {0, 0};
    private World world;

    private int strength;
    private int dexterity;
    private int health;
    private int luck;
    private int aggression;
    private int wisdom;

    private Weapon[] hands = new Weapon[2];
    public Weapon H1 => hands[0];
    public Weapon H2 => hands[1];
    public int H => health;
    public int P => strength;
    public int W => wisdom;
    public int A => aggression;
    public int L => luck;
    public int D => dexterity;

    public Player(World w)
    {
        strength = dexterity = health = aggression = wisdom = 5;
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

            if (world.Map[new_x, new_y] == Chars.wall_char)
            {
                return;
            }
            
            if (world.Map[new_x, new_y] != Chars.empty_char)
            {
                int item_idx = -1;
                for (int itm = 0; itm < Chars.item_chars.Length; itm++)
                {
                    if (world.Map[new_x, new_y] == Chars.item_chars[itm])
                    {
                        item_idx = itm;
                        break;
                    }
                }


                switch (item_idx)
                {
                    case 0:
                        //The old rabbi's scroll +5 to wisdom;
                        wisdom += 5;
                        break;
                    case 1:
                        //The Magi's Spellbook +7 to luck
                        luck += 7;
                        break;
                    case 2:
                        //An oriental puzzle + 4 to dexterity
                        dexterity += 4;
                        break;
                    case 3:
                        //Superfist +3 to strength
                        strength += 3;
                        break;
                    default:
                        return;

                }
            }
            position[0] = new_x;
            position[1] = new_y;
        }
    }
}

