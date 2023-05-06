using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Default,
    Entrance,
    Shop,
    BossRoom
}

public class RoomWalls
{
    public RoomWalls(RectInt Top, RectInt Bottom, RectInt Left, RectInt Right)
    {
        this.Top = Top;
        this.Bottom = Bottom;
        this.Left = Left;
        this.Right = Right;
    }
    public RectInt Top { get; }
    public RectInt Bottom { get; }
    public RectInt Left { get; }
    public RectInt Right { get; }
}

public class Room
{
    public RoomType RoomType { get; private set; }
    public RectInt RoomFloor { get; }
    public Vector2Int RoomCenter => Vector2Int.FloorToInt(RoomFloor.center);
    public Color RoomColor { get; private set; }

    public Room(RectInt roomFloor)
    {
        RoomFloor = roomFloor;
        SetRoomType(RoomType.Default);
    }

    public void SetRoomType(RoomType roomType)
    {
        RoomType = roomType;
        SetRoomColor();
    }

    private void SetRoomColor()
    {
        RoomColor = RoomType switch
        {
            RoomType.Entrance => Color.cyan,
            RoomType.Shop => Color.yellow,
            RoomType.BossRoom => Color.red,
            _ => Color.gray,
        };
    }

    public HashSet<Vector2Int> GetFloor()
    {
        var roomFloor = new HashSet<Vector2Int>();
        foreach (var tile in RoomFloor.allPositionsWithin)
        {
            roomFloor.Add(tile);
        }
        return roomFloor;
    }

    public RoomWalls GetWalls()
    {
        return new RoomWalls(
            new RectInt(RoomFloor.xMin, RoomFloor.yMax - 4, RoomFloor.width, 1),
            new RectInt(RoomFloor.xMin, RoomFloor.yMin, RoomFloor.width, 1),
            new RectInt(RoomFloor.xMax - 1, RoomFloor.yMin, 1, RoomFloor.height),
            new RectInt(RoomFloor.xMin, RoomFloor.yMin, 1, RoomFloor.height)
        );
    }
}