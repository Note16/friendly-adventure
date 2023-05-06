using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private readonly RoomVisualizer roomVisualizer;
    private RoomType roomType { get; set; }
    private Color roomColor { get; set; }
    private RectInt roomFloor { get; }
    public Vector2Int Position => roomFloor.position;
    public Vector2Int RoomCenter => Vector2Int.FloorToInt(roomFloor.center);

    public Room(RoomVisualizer roomVisualizer, RectInt roomFloor)
    {
        this.roomVisualizer = roomVisualizer;
        this.roomFloor = roomFloor;
        SetRoomType(RoomType.Default);
        SetWalls();
    }

    public void SetRoomType(RoomType roomType)
    {
        this.roomType = roomType;
        SetRoomColor();
        SetFloor();
    }

    private void SetRoomColor()
    {
        roomColor = roomType switch
        {
            RoomType.Entrance => Color.cyan,
            RoomType.Shop => Color.yellow,
            RoomType.BossRoom => Color.red,
            _ => Color.gray,
        };
    }

    private void SetFloor()
    {
        var roomFloor = new HashSet<Vector2Int>();
        foreach (var tile in this.roomFloor.allPositionsWithin)
        {
            roomFloor.Add(tile);
        }

        roomVisualizer.PaintFloorTiles(roomFloor, roomColor);
    }

    private void SetWalls()
    {
        roomVisualizer.PaintTopWall(new RectInt(roomFloor.xMin, roomFloor.yMax - 4, roomFloor.width, 1));
        roomVisualizer.PaintBottomWall(new RectInt(roomFloor.xMin, roomFloor.yMin, roomFloor.width, 1));
        roomVisualizer.PaintLeftWall(new RectInt(roomFloor.xMax - 1, roomFloor.yMin, 1, roomFloor.height));
        roomVisualizer.PaintRightWall(new RectInt(roomFloor.xMin, roomFloor.yMin, 1, roomFloor.height));
    }
}