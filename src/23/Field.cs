using System.Diagnostics.CodeAnalysis;

namespace _23;

[Flags]
enum FieldType
{
    Empty = 0,
    Wall = 0x1,
    Hallway = 0x10,
    Room = 0x100,
    RoomA = Room | 0x1000,
    RoomB = Room | 0x2000,
    RoomC = Room | 0x4000,
    RoomD = Room | 0x8000,
    RoomDoor = Hallway | 0x10000,
}

abstract record Field(FieldType Type)
{
    public abstract char ToChar();
}

record WallField() : Field(FieldType.Wall)
{
    public override char ToChar() => '#';
}

record EmptyField() : Field(FieldType.Empty)
{
    public override char ToChar() => ' ';
}

record OccupyableField : Field
{
    public OccupyableField(FieldType type, char? occupant) : base(type)
    {
        Occupant = occupant;
    }

    public char? Occupant { get; }

    [MemberNotNullWhen(true, "Occupant")]
    public bool IsOccupied => Occupant.HasValue;

    public override char ToChar() => IsOccupied ? Occupant.Value : '.';
}

record HallwayField(char? occupant = null) : OccupyableField(FieldType.Hallway, occupant);

record RoomDoor(char? occupant = null) : OccupyableField(FieldType.RoomDoor, occupant);

abstract record RoomField(FieldType type, char? occupant = null) : OccupyableField(
    type.HasFlag(FieldType.Room) && type != FieldType.Room ? type : throw new Exception($"Invalid room {type}!"),
    occupant);

record RoomAField(char? occupant = null) : RoomField(FieldType.RoomA, occupant);
record RoomBField(char? occupant = null) : RoomField(FieldType.RoomB, occupant);
record RoomCField(char? occupant = null) : RoomField(FieldType.RoomC, occupant);
record RoomDField(char? occupant = null) : RoomField(FieldType.RoomD, occupant);
