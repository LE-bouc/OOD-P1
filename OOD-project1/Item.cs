namespace OOD_project1;

public abstract class Item
{
    public Player_Stats player_effects;
    public bool twohanded;
    public Item(string itemName)
    { 
        Name = itemName;
        player_effects = new Player_Stats();
    }
    public string Name { get; protected set; }
    public virtual string GetName() => Name;
}

public class Empty : Item
{
    public Empty() : base("Empty Space")
    {
    }
}

public class Wall : Item
{
    public Wall() : base("Wall")
    {
    }
}

public class Coin : Item
{
    public Coin() : base("Coin"){}
}


public abstract class Weapon : Item
{
    public int damage_value;

    public Weapon(string _itemName) : base(_itemName)
    {
        player_effects = new Player_Stats();
    }

    public override string GetName() => $"{Name} (Damage: {damage_value})";    
}

public abstract class ItemEffect : Item
{
    protected Item _baseItem;
    public ItemEffect(Item baseItem) : base(baseItem.Name)
    {
        _baseItem = baseItem;
        player_effects = new Player_Stats();
    }

    public override string GetName() => _baseItem.GetName();
}

public abstract class WeaponEffect : Weapon
{
    public Weapon _baseWeapon;
    public int damage_value;

    public WeaponEffect(Weapon baseWeapon) : base(baseWeapon.Name)
    {
        _baseWeapon = baseWeapon;
        player_effects = new Player_Stats();
    }

    public override string GetName() => _baseWeapon.GetName();
}



public class FireAspect : WeaponEffect
{
    public FireAspect(Weapon _baseWeapon) : base(_baseWeapon)
    {
        Name = $"{_baseWeapon.GetName()} (Fire Aspect)";
        player_effects.dexterity = 3;
        damage_value = 10;
        damage_value += _baseWeapon.damage_value;
        player_effects.Add(_baseWeapon.player_effects);
    }
    public override string GetName() => Name;
    
}

public class Rusty : WeaponEffect
{
    public Rusty(Weapon _baseWeapon) : base(_baseWeapon)
    {
        Name = $"{_baseWeapon.GetName()}  (Rusty)";
        damage_value = -6;
        damage_value += _baseWeapon.damage_value;
        player_effects.Add(_baseWeapon.player_effects);
    }
    public override string GetName() => Name;
}

public class Gilded : WeaponEffect
{
    public Gilded(Weapon _baseWeapon) : base(_baseWeapon)
    {
        Name = $"{_baseWeapon.GetName()} (Gilded)";
        player_effects.wisdom = 10;
        damage_value = 10;
        damage_value += _baseWeapon.damage_value;
        player_effects.Add(_baseWeapon.player_effects);
    }
    public override string GetName() => Name;
}


public class OldRabbiScroll : Item 
{
    public OldRabbiScroll() : base("Old Rabbi's Scroll")
    {
        twohanded = false;
        player_effects.wisdom = 10;
        player_effects.aggression = -5;
        player_effects.luck = 10;
    }
}

public class GreatAxe : Weapon
{
    public GreatAxe() : base("Great Axe")
    {
        twohanded = true;
        damage_value = 30;
        player_effects.dexterity = -5;
    }
}
public class Shield : Weapon
{
    public Shield() : base("Shield")
    {
        twohanded = false;
        damage_value = 10;
        player_effects.health = 10;
        player_effects.dexterity = -10;
    }
}

public class Sword : Weapon
{
    public Sword() : base("Sword")
    {
        twohanded = false;
        damage_value = 10;
    }
}
