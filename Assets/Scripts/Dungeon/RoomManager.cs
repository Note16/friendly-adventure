using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager
{
    private readonly int minRoomCount = 8;
    private readonly int maxRoomCount = 15;
    private readonly List<Room> rooms;
    private readonly RoomVisualizer roomVisualizer;

    public RoomManager(RoomVisualizer roomVisualizer, Vector2Int startPosition, Vector2Int roomSize, int offset)
    {
        this.roomVisualizer = roomVisualizer;
        rooms = GenerateRooms(startPosition, roomSize, offset);
        AssignRoomTypes();
    }

    private List<Room> GenerateRooms(Vector2Int startPosition, Vector2Int roomSize, int offset)
    {
        var roomsList = new HashSet<Room>
        {
            GenerateRoom(startPosition, roomSize, offset, null)
        };
        while ((Random.value > 0.1f || roomsList.Count < minRoomCount) && roomsList.Count != maxRoomCount)
        {
            var room = GenerateRoom(startPosition, roomSize, offset, GetRandomDirection());
            startPosition = room.Position;
            roomsList.Add(room);
        }
        return roomsList.ToList();
    }

    public List<Room> GetRooms()
    {
        return rooms.ToList();
    }

    private void AssignRoomTypes()
    {
        rooms.First().SetRoomType(RoomType.Entrance);
        rooms.Last().SetRoomType(RoomType.BossRoom);
    }

    private Room GenerateRoom(Vector2Int currentPosition, Vector2Int roomSize, int offset, Direction? direction)
    {
        if (direction == Direction.Up)
            currentPosition += new Vector2Int(0, roomSize.y + offset);
        if (direction == Direction.Down)
            currentPosition += new Vector2Int(0, -(roomSize.y + offset));
        if (direction == Direction.Left)
            currentPosition += new Vector2Int(-(roomSize.x + offset), 0);
        if (direction == Direction.Right)
            currentPosition += new Vector2Int(roomSize.x + offset, 0);

        return new Room(roomVisualizer, new RectInt(currentPosition, roomSize));
    }

    private static Direction GetRandomDirection()
    {
        var values = Enum.GetValues(typeof(Direction));
        var random = new System.Random();
        return (Direction)values.GetValue(random.Next(values.Length));
    }
}