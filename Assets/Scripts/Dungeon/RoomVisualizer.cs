using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class NamedTileBase
{
    public string Name;
    public TileBase TileBase;
}

public class RoomVisualizer : TilemapVisualizer
{
    [SerializeField]
    private TileBase floorTile;

    [SerializeField]
    private List<NamedTileBase> topWallTiles;
    
    [SerializeField]
    private List<NamedTileBase> leftWallTiles;

    [SerializeField]
    private List<NamedTileBase> rightWallTiles;

    [SerializeField]
    private List<NamedTileBase> bottomWallTiles;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions, Color color)
    {
        foreach (var position in floorPositions)
        {
            SetFloorTile(floorTile, position, color);
        }
    }

    public void PaintTopWall(RectInt wallRect)
    {
        var xAxis = wallRect.x;
        foreach (var tile in wallRect.allPositionsWithin)
        {
            var tileToPaint = tile;

            SetWallTile(topWallTiles.First(wallTile => wallTile.Name == "1_ledge_middle").TileBase, tileToPaint);
            
            if (tile.y == wallRect.y && xAxis == tile.x)
            {
                SetWallTile(topWallTiles.First(wallTile => wallTile.Name == "1_pillar_middle").TileBase, tileToPaint += Vector2Int.up);
                SetWallTile(topWallTiles.First(wallTile => wallTile.Name == "1_pillar_middle").TileBase, tileToPaint += Vector2Int.up);
                SetWallTile(topWallTiles.First(wallTile => wallTile.Name == "1_pillar_top").TileBase, tileToPaint += Vector2Int.up);

                // Pillar distance
                xAxis = tile.x + 3;
            } 
            else if (tile.y == wallRect.y && xAxis != tile.x)
            {
                SetWallTile(topWallTiles.First(wallTile => wallTile.Name == "1_wall_middle").TileBase, tileToPaint += Vector2Int.up);
                SetWallTile(topWallTiles.First(wallTile => wallTile.Name == "1_wall_middle").TileBase, tileToPaint += Vector2Int.up);
                SetWallTile(topWallTiles.First(wallTile => wallTile.Name == "1_wall_top").TileBase, tileToPaint += Vector2Int.up);
            }
        }
    }

    public void PaintBottomWall(RectInt wallRect)
    {
        foreach (var tile in wallRect.allPositionsWithin)
        {
            SetWallTile(bottomWallTiles.First().TileBase, tile);
            SetWallTile(bottomWallTiles.First().TileBase, tile);
        }
    }

    public void PaintLeftWall(RectInt wallRect)
    {
        PaintSideWall(wallRect, leftWallTiles);
        PaintSideWallLedge(wallRect, leftWallTiles, Vector2Int.right);
    }

    public void PaintRightWall(RectInt wallRect)
    {
        PaintSideWall(wallRect, rightWallTiles);
        PaintSideWallLedge(wallRect, rightWallTiles, Vector2Int.left);
    }

    private void PaintSideWallLedge(RectInt wallRect, List<NamedTileBase> wallTiles, Vector2Int direction) 
    {
        foreach (var tile in wallRect.allPositionsWithin)
        {
            var adjecentTile = tile + direction;
            var targetTile = GetWallTile(adjecentTile);

            if (targetTile != null)
            {
                if (topWallTiles.Any(tile => tile.Name.Contains("ledge") && tile.TileBase.name == targetTile.name))
                {
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_ledge_top").TileBase, adjecentTile);
                }
            }
            else
            {
                SetWallTile(wallTiles.First(lw => lw.Name == "1_ledge_middle").TileBase, adjecentTile);
            }
        }
    }

    private void PaintSideWall(RectInt wallRect, List<NamedTileBase> wallTiles)
    {
        foreach (var tile in wallRect.allPositionsWithin)
        {
            if (tile.y == wallRect.y)
            {
                SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_bottom").TileBase, tile);
            }
            else if (tile.y == wallRect.yMax - 1)
            {
                SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_top").TileBase, tile);
            }
            else
            {
                SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_middle").TileBase, tile);
            }
        }
    }
}