using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private readonly DungeonVisualizer dungeonVisualizer;
    private RoomType roomType { get; set; }
    private Color roomColor { get; set; }
    private RectInt roomFloor { get; }
    public Vector2Int Position => roomFloor.position;
    public Vector2Int RoomCenter => Vector2Int.FloorToInt(roomFloor.center);

    public Room(DungeonVisualizer dungeonVisualizer, RectInt roomFloor)
    {
        this.dungeonVisualizer = dungeonVisualizer;
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

        dungeonVisualizer.PaintFloorTiles(roomFloor, roomColor);
    }

    private void SetWalls()
    {
        dungeonVisualizer.PaintTopWall(new RectInt(roomFloor.xMin, roomFloor.yMax - 4, roomFloor.width, 1));
        dungeonVisualizer.PaintBottomWall(new RectInt(roomFloor.xMin, roomFloor.yMin, roomFloor.width, 1));
        dungeonVisualizer.PaintLeftWall(new RectInt(roomFloor.xMax - 1, roomFloor.yMin, 1, roomFloor.height));
        dungeonVisualizer.PaintRightWall(new RectInt(roomFloor.xMin, roomFloor.yMin, 1, roomFloor.height));
    }
}