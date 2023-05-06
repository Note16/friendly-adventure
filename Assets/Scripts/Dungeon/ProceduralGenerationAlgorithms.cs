using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgorithms
{
    enum Direction
    { 
        Up,
        Down,
        Left,
        Right
    }

    public static HashSet<RectInt> RoomSpacePartitioning(Vector2Int startPostion, int roomWidth, int roomHeight, int offset)
    {
        var roomsList = new HashSet<RectInt>();
        var roomSize = new Vector2Int(roomWidth, roomHeight);
        var currentPosition = startPostion;

        // Generate start room;
        roomsList.Add(GenerateRoom(currentPosition, roomSize, offset, null));

        var minRoomCount = 8;
        var maxRoomCount = 15;

        while ((Random.value > 0.1f || roomsList.Count < minRoomCount) && roomsList.Count != maxRoomCount)
        {
            var room = GenerateRoom(currentPosition, roomSize, offset, GetRandomDirection());
            currentPosition = room.position;
            roomsList.Add(room);
        }

        return roomsList;
    }

    private static Direction GetRandomDirection()
    {
        var values = Enum.GetValues(typeof(Direction));
        var random = new System.Random();
        return (Direction)values.GetValue(random.Next(values.Length));
    }

    private static RectInt GenerateRoom(Vector2Int currentPosition, Vector2Int roomSize, int offset, Direction? direction)
    {
        if (direction == Direction.Up)
            currentPosition += new Vector2Int(0, roomSize.y + offset);
        if (direction == Direction.Down)
            currentPosition += new Vector2Int(0, -(roomSize.y + offset));
        if (direction == Direction.Left)
            currentPosition += new Vector2Int(-(roomSize.x + offset), 0);
        if (direction == Direction.Right)
            currentPosition += new Vector2Int(roomSize.x + offset, 0);

        return new RectInt(currentPosition, roomSize);
    }
}