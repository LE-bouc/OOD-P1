namespace OOD_project1;

public static class Chars
{
    public static readonly char  wall_char = '\u2588';
    public static readonly char empty_char = ' ';
    public static readonly char player_char = '¶';
    public static readonly char[] item_chars = { 'ℛ', 'ℳ', '\u22a1', '\u270a'};
}

public class World
{
    private int no_items = 10;
    private char[,] map;
    public int x_boundary;
    public int y_boundary;
    
    public char[,] Map => map;
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


    private char[,] build_map(int x, int y)
    {
        char[,] grid = new char[x, y];
        //Set Empty Spaces
        for (int i = 1; i < x - 1; i++)
        {
            for (int j = 1; j < y - 1; j++)
            {
                grid[i, j] = Chars.empty_char;
            }
        }
        //Set vertical walls
        for (int j = 0; j < y; j++)
        {
            grid[0, j] = Chars.wall_char;        
            grid[x - 1, j] = Chars.wall_char;    
        }
        //Set horizontal walls
        for (int i = 0; i < x; i++)
        {
            grid[i, 0] = Chars.wall_char;        
            grid[i, y - 1] = Chars.wall_char;    
        }
        //Creating Entrance
        grid[0, 0] = Chars.empty_char;
        grid[1, 0] = Chars.empty_char;
        
        //Placing Items
        Random rnd = new Random();
        for (int i = 0; i < this.no_items; i++)
        {
            int type_it = rnd.Next(0, 4);
            int x_it = rnd.Next(1, x_boundary-1);
            int y_it = rnd.Next(1, y_boundary-1);  
            grid[x_it, y_it] = Chars.item_chars[type_it];
        }
            
        
        
        
        
        
        return grid;
    }
    
    public void Display()
    {
        Console.Clear();
        Console.WriteLine($"Information about the player:\n | Health:{player.H} | Aggresion:{player.A} | Position:({player.position[0]},{player.position[1]}) |\n | Strength:{player.P} |Wisdom:{player.W} | Dexterity:{player.D} |\n | Hand 1: {player.H1} | Hand 2: {player.H2}");
        for (int i = 0; i < x_boundary; i++)
        {
            for (int j = 0; j < y_boundary; j++)
            {
                //Display the Map, replacing the tile on which the player stands with the player symbol
                Console.Write(player.position[0] == i && player.position[1] == j ? Chars.player_char : map[i, j]);
            }
            Console.WriteLine();
        }
    }
    
    
}