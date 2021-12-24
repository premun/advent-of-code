using System.Diagnostics.CodeAnalysis;

namespace _23;

abstract record Field
{
    public abstract char ToChar();
}

record WallField() : Field
{
    public override char ToChar() => '#';
}

record EmptyField() : Field
{
    public override char ToChar() => ' ';
}

record OccupyableField(char? Occupant) : Field
{
    [MemberNotNullWhen(true, "Occupant")]
    public bool IsOccupied => Occupant.HasValue;

    public override char ToChar() => IsOccupied ? Occupant.Value : '.';
}

record HallwayField : OccupyableField
{
    public HallwayField(char? occupant = null) : base(occupant)
    {
    }
}

record RoomDoor : OccupyableField
{
    public RoomDoor(char? occupant = null) : base(occupant)
    {
    }
}

record RoomField : OccupyableField
{
    public RoomField(char name, char? occupant = null) : base(occupant)
    {
        Name = name;
    }

    public char Name { get; }

    public bool OccupantIsHome => Name == Occupant;
}
