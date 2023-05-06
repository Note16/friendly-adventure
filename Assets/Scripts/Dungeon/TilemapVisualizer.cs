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

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    [SerializeField]
    private List<NamedTileBase> topWallTiles;
    
    [SerializeField]
    private List<NamedTileBase> leftWallTiles;

    [SerializeField]
    private List<NamedTileBase> rightWallTiles;

    [SerializeField]
    private List<NamedTileBase> bottomWallTiles;


    [SerializeField]
    private TileBase floorTile;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions, Color color)
    {
        foreach (var position in floorPositions)
        {
            ColorSingleTile(floorTilemap, position, color);
        }
    }

    public void PaintTopWall(RectInt wallRect)
    {
        var xAxis = wallRect.x;
        foreach (var tile in wallRect.allPositionsWithin)
        {
            var tileToPaint = tile;

            SetSingleTile(wallTilemap, topWallTiles.First(wallTile => wallTile.Name == "1_ledge_middle").TileBase, tileToPaint);
            
            if (tile.y == wallRect.y && xAxis == tile.x)
            {
                SetSingleTile(wallTilemap, topWallTiles.First(wallTile => wallTile.Name == "1_pillar_middle").TileBase, tileToPaint += Vector2Int.up);
                SetSingleTile(wallTilemap, topWallTiles.First(wallTile => wallTile.Name == "1_pillar_middle").TileBase, tileToPaint += Vector2Int.up);
                SetSingleTile(wallTilemap, topWallTiles.First(wallTile => wallTile.Name == "1_pillar_top").TileBase, tileToPaint += Vector2Int.up);

                // Pillar distance
                xAxis = tile.x + 3;
            } 
            else if (tile.y == wallRect.y && xAxis != tile.x)
            {
                SetSingleTile(wallTilemap, topWallTiles.First(wallTile => wallTile.Name == "1_wall_middle").TileBase, tileToPaint += Vector2Int.up);
                SetSingleTile(wallTilemap, topWallTiles.First(wallTile => wallTile.Name == "1_wall_middle").TileBase, tileToPaint += Vector2Int.up);
                SetSingleTile(wallTilemap, topWallTiles.First(wallTile => wallTile.Name == "1_wall_top").TileBase, tileToPaint += Vector2Int.up);
            }
        }
    }

    public void PaintBottomWall(RectInt wallRect)
    {
        foreach (var tile in wallRect.allPositionsWithin)
        {
            SetSingleTile(wallTilemap, bottomWallTiles.First().TileBase, tile);
            SetSingleTile(wallTilemap, bottomWallTiles.First().TileBase, tile);
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

    public void PaintSideWallLedge(RectInt wallRect, List<NamedTileBase> wallTiles, Vector2Int direction) 
    {
        foreach (var tile in wallRect.allPositionsWithin)
        {
            var adjecentTile = tile + direction;

            if (wallTilemap.HasTile((Vector3Int)adjecentTile))
            {
                var nextTile = wallTilemap.GetTile((Vector3Int)adjecentTile);
                if (topWallTiles.Any(tile => tile.Name.Contains("ledge") && tile.TileBase.name == nextTile.name))
                {
                    SetSingleTile(wallTilemap, wallTiles.First(lw => lw.Name == "1_ledge_top").TileBase, adjecentTile);
                }
            }
            else
            {
                SetSingleTile(wallTilemap, wallTiles.First(lw => lw.Name == "1_ledge_middle").TileBase, adjecentTile);
            }
        }
    }

    public void PaintSideWall(RectInt wallRect, List<NamedTileBase> wallTiles)
    {
        foreach (var tile in wallRect.allPositionsWithin)
        {
            if (tile.y == wallRect.y)
            {
                SetSingleTile(wallTilemap, wallTiles.First(lw => lw.Name == "1_wall_bottom").TileBase, tile);
            }
            else if (tile.y == wallRect.yMax - 1)
            {
                SetSingleTile(wallTilemap, wallTiles.First(lw => lw.Name == "1_wall_top").TileBase, tile);
            }
            else
            {
                SetSingleTile(wallTilemap, wallTiles.First(lw => lw.Name == "1_wall_middle").TileBase, tile);
            }
        }
    }


    private void ColorSingleTile(Tilemap tilemap, Vector2Int position, Color color)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, floorTile);
        tilemap.SetTileFlags(tilePosition, TileFlags.None);
        tilemap.SetColor(tilePosition, color);
    }

    private void SetSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}