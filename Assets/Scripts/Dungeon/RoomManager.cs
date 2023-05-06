using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager
{
    private readonly List<Room> rooms;

    public RoomManager(Vector2Int startPosition, int minRoomWidth, int minRoomHeight, int offset)
    {
        var sections = ProceduralGenerationAlgorithms.RoomSpacePartitioning(
            startPosition,
            minRoomWidth,
            minRoomHeight,
            offset
        );

        // Create list of rooms from sections
        rooms = sections.Select(section => new Room(section)).ToList();
        
        // Assign room types
        rooms.First().SetRoomType(RoomType.Entrance);
        rooms.Last().SetRoomType(RoomType.BossRoom);
    }

    public List<Room> GetRooms()
    {
        return rooms.ToList();
    }
}