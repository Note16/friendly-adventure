using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    protected RoomVisualizer roomVisualizer;

    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField]
    private int roomWidth = 30, roomHeight = 20;

    [SerializeField]
    [Range(0, 10)]
    private int offset = 4;

    public void GenerateDungeon()
    {
        roomVisualizer = GetComponent<RoomVisualizer>();
        roomVisualizer.Clear();
        CreateRooms();
    }

    private void CreateRooms()
    {
        // Generate list of Rooms
        var roomManager = new RoomManager(
            roomVisualizer,
            startPosition,
            new Vector2Int(roomWidth, roomHeight),
            offset
        );

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