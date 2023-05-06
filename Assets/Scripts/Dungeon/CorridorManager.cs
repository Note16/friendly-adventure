using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CorridorManager
{
    private HashSet<Vector2Int> Corridors { get; }
    private List<Room> Rooms { get; }

    public CorridorManager(List<Room> rooms)
    {
        Rooms = rooms;
        var roomCenters = rooms.Select(x => x.RoomCenter).ToList();
        Corridors = ConnectRooms(roomCenters);
    }

    public HashSet<Vector2Int> GetCorridors()
    {
        return Corridors;
    }

    public Room GetClosestRoom(Room room)
    {
        return Rooms.Select(_room => new
        {
            Room = _room,
            Distance = Vector2.Distance(_room.RoomCenter, room.RoomCenter)
        })
        .OrderBy(obj => obj.Distance)
        .ElementAt(1).Room;
    }


    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        var corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            corridors.Add(closest);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new();
        var position = currentRoomCenter;

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }

            var y = new RectInt(position, new Vector2Int(1, 1));
            foreach (var t in y.allPositionsWithin)
            {
                corridor.Add(t);
            }
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }

            var y = new RectInt(position, new Vector2Int(1, 1));
            foreach (var t in y.allPositionsWithin)
            {
                corridor.Add(t);
            }
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }
}