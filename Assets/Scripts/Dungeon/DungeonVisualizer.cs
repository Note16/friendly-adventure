using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Dungeon
{
    [Serializable]
    public class NamedTileBase
    {
        public string Name;
        public TileBase TileBase;
    }

    public class DungeonVisualizer : MonoBehaviour
    {
        [SerializeField]
        private TileBase floorTile;

        [SerializeField]
        private Tilemap floorTilemap, wallTilemap;

        [SerializeField]
        private List<NamedTileBase> wallTiles;

        [SerializeField]
        private List<NamedTileBase> ledgeTiles;

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

        public void SetFloorTile(Vector2Int position, Color? color = null)
        {
            SetSingleTile(floorTilemap, floorTile, position, color);
        }

        public void SetFloorTiles(IEnumerable<Vector2Int> floorPositions, Color? color)
        {
            foreach (var position in floorPositions)
            {
                SetFloorTile(position, color);
            }
        }

        public void SetWallTile(string tileName, Vector2Int position, Color? color = null)
        {
            var tileBase = wallTiles.FirstOrDefault(wallTile => wallTile.Name == tileName)?.TileBase;
            if (tileBase == null)
                Debug.Log($"Tile with name '{tileName}' not found in wallTiles List");

            SetSingleTile(wallTilemap, tileBase, position, color);
        }

        public void SetWallLedgeTile(string tileName, Vector2Int position, Color? color = null)
        {
            var tileBase = ledgeTiles.FirstOrDefault(wallTile => wallTile.Name == tileName)?.TileBase;
            if (tileBase == null)
                Debug.Log($"Tile with name '{tileName}' not found in ledgeTiles List");

            SetSingleTile(wallTilemap, tileBase, position, color);
        }

        public void ClearWallTile(Vector2Int position)
        {
            wallTilemap.SetTile((Vector3Int)position, null);
        }

        public void ClearWallTiles(IEnumerable<Vector2Int> wallPositions)
        {
            foreach (var position in wallPositions)
            {
                ClearWallTile(position);
            }
        }

        public void Clear()
        {
            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();
        }
    }
}