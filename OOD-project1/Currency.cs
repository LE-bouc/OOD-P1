using OOD_project1;

namespace OOD_project1;

public class Currency : ITileable
{
    public Currency(char c)
    {
        charRepresentation = c;
    }
    public char charRepresentation { get; set; }
}


public class Gold : Currency
{
    public Gold() : base('g')
    {
    }
}

public class Coin : Currency
{
    public Coin() : base('c')
    {
    }
}