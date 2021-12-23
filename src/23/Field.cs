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

record OccupyableField : Field
{
    public OccupyableField(char? occupant)
    {
        Occupant = occupant;
    }

    public char? Occupant { get; }

    [MemberNotNullWhen(true, "Occupant")]
    public bool IsOccupied => Occupant.HasValue;

    public override char ToChar() => IsOccupied ? Occupant.Value : '.';
}

record HallwayField(char? occupant = null) : OccupyableField(occupant);

record RoomDoor(char? occupant = null) : OccupyableField(occupant);

abstract record RoomField(char? occupant = null) : OccupyableField(occupant);

record RoomAField(char? occupant = null) : RoomField(occupant);
record RoomBField(char? occupant = null) : RoomField(occupant);
record RoomCField(char? occupant = null) : RoomField(occupant);
record RoomDField(char? occupant = null) : RoomField(occupant);
