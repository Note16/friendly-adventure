using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new()
    {
        new Vector2Int(0,1),  // UP
        new Vector2Int(1,0),  // RIGHT
        new Vector2Int(0,-1), // DOWN
        new Vector2Int(-1,0)  // LEFT
    };
}