using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField]
    private int roomWidth = 30, roomHeight = 20;

    [SerializeField]
    [Range(0, 10)]
    private int offset = 4;

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        CreateRooms();
    }

    private void CreateRooms()
    {
        // Generate list of Rooms
        var roomManager = new RoomManager(
            startPosition,
            roomWidth,
            roomHeight,
            offset
        );

        var rooms = roomManager.GetRooms();
        rooms.ForEach(room =>
        {
            tilemapVisualizer.PaintFloorTiles(room.GetFloor(), room.RoomColor);

            tilemapVisualizer.PaintTopWall(room.GetWalls().Top);
            tilemapVisualizer.PaintBottomWall(room.GetWalls().Bottom);
            tilemapVisualizer.PaintLeftWall(room.GetWalls().Left);
            tilemapVisualizer.PaintRightWall(room.GetWalls().Right);
        });

        //var corridorManger = new CorridorManager(rooms);
        //var corridors = corridorManger.GetCorridors();

        //var room = rooms.First();
        //tilemapVisualizer.PaintFloorTiles(new List<Vector2Int> { room.RoomCenter }, Color.white);
        //var closestRoom = corridorManger.GetClosestRoom(room);
        //tilemapVisualizer.PaintFloorTiles(new List<Vector2Int> { closestRoom.RoomCenter }, Color.white);

        // Create floor
       // tilemapVisualizer.PaintFloorTiles(corridors, Color.white);

        // Create walls
        //WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }
}