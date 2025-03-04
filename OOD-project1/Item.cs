namespace OOD_project1;

public abstract class Item : ITileable
{
    public Player_Stats player_effects;
    public bool twohanded;
    protected bool pickupable;
    public bool Pickupable {get;}
    public Item(string itemName,char c)
    { 
        Name = itemName;
        player_effects = new Player_Stats();
        pickupable = true;
        charRepresentation = c;
    }
    public string Name { get; protected set; }
    public virtual string GetName() => Name;
    public char charRepresentation { get; set; }
}


public abstract class Weapon : Item
{
    public int damage_value;

    public Weapon(string _itemName,char c) : base(_itemName,c)
    {
        player_effects = new Player_Stats();
    }

    public override string GetName() => $"{Name} (Damage: {damage_value})";    
}

public abstract class ItemEffect : Item
{
    protected Item _baseItem;
    public ItemEffect(Item baseItem) : base(baseItem.Name,baseItem.charRepresentation)
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

    public WeaponEffect(Weapon baseWeapon) : base(baseWeapon.Name,baseWeapon.charRepresentation)
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
    public OldRabbiScroll() : base("Old Rabbi's Scroll", 'ℛ')
    {
        twohanded = false;
        player_effects.wisdom = 10;
        player_effects.aggression = -5;
        player_effects.luck = 10;
        pickupable = false;
    }
}

public class BigBagONothin : Item 
{
    public BigBagONothin() : base("big bag o' nothin'", 'B')
    {
        twohanded = false;
        pickupable = false;
    }
}

public class TwoPrettyBestFriends : Item
{
    private bool one_of_them_gotta_be_ugly;
    public TwoPrettyBestFriends() : base("TwoPrettyBestFriends", 'P')
    {
        twohanded = false;
        pickupable = false;
        one_of_them_gotta_be_ugly = true;
    }
}



public class GreatAxe : Weapon
{
    public GreatAxe() : base("Great Axe", 'A')
    {
        twohanded = true;
        damage_value = 30;
        player_effects.dexterity = -5;
    }
}
public class Shield : Weapon
{
    public Shield() : base("Shield", 'S')
    {
        twohanded = false;
        damage_value = 10;
        player_effects.health = 10;
        player_effects.dexterity = -10;
    }
}

public class Sword : Weapon
{
    public Sword() : base("Sword", '/')
    {
        twohanded = false;
        damage_value = 10;
    }
}
