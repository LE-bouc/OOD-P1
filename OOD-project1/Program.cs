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
                    //Movement
                    case ConsoleKey.W: world.player.Move(-1, 0); break;
                    case ConsoleKey.S: world.player.Move(1, 0); break;
                    case ConsoleKey.A: world.player.Move(0, -1); break;
                    case ConsoleKey.D: world.player.Move(0, 1); break;
                    
                    //Inventory Managment
                    case ConsoleKey.E: world.player.PickUp(); break;
                    case ConsoleKey.D0: world.player.Equip(0); break;
                    case ConsoleKey.D1: world.player.Equip(1); break;
                    case ConsoleKey.D2: world.player.Equip(2); break;
                    case ConsoleKey.D3: world.player.Equip(3); break;
                    case ConsoleKey.D4: world.player.Equip(4); break;
                    case ConsoleKey.D5: world.player.Equip(5); break;
                    case ConsoleKey.D6: world.player.Equip(6); break;
                    case ConsoleKey.D7: world.player.Equip(7); break;
                    case ConsoleKey.D8: world.player.Equip(8); break;
                    case ConsoleKey.D9: world.player.Equip(9); break;
                    
                    case ConsoleKey.Escape: return; 
                }
            }
            Thread.Sleep(50);
        }
    }
}