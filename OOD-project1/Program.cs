namespace OOD_project1;

public class Program
{
    public static void Main()
    {
        World world = new World(20, 40); // Create a 20x40 world

        Console.CursorVisible = false;

        while (true)
        {
            world.Display();
            
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.W: world.player.Move(-1, 0); break;
                    case ConsoleKey.S: world.player.Move(1, 0); break;
                    case ConsoleKey.A: world.player.Move(0, -1); break;
                    case ConsoleKey.D: world.player.Move(0, 1); break;
                    case ConsoleKey.Escape: return; 
                }
            }
            Thread.Sleep(50);
        }
    }
}