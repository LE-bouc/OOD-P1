namespace OOD_project1;

public interface ITileable
{
    public char charRepresentation { get; set; }
    
    
}

public class Tile : ITileable
{
    public Tile()
    {
    }

    public Tile(char c)
    {
        charRepresentation = c;
    }

    public char charRepresentation { get; set; }
}

public class Wall : Tile
{
    public Wall() : base('\u2588')
    {
    }
}

public class Empty : Tile
{
    public Empty() : base(' ')
    {
    }
}