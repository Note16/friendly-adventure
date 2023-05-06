using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    public TileBase GetWallTile(Vector2Int tile)
    {
        return wallTilemap.GetTile((Vector3Int)tile);
    }
    public void SetWallTile(TileBase tile, Vector2Int position, Color? color = null)
    {
        SetSingleTile(wallTilemap, tile, position, color);
    }
    public void SetFloorTile(TileBase tile, Vector2Int position, Color? color = null)
    { 
        SetSingleTile(floorTilemap, tile, position, color);
    }

    private void SetSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position, Color? color = null)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);

        if (color.HasValue)
        {
            tilemap.SetTileFlags(tilePosition, TileFlags.None);
            tilemap.SetColor(tilePosition, (Color)color);
        }
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}