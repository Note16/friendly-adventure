using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Dungeon
{
    [Serializable]
    public class NamedTileBase
    {
        public string Name;
        public List<TileBase> Tiles;
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

        public void SetRandomWallTile(string tileName, Vector2Int position, Color? color = null)
        {
            SetRandomTile(wallTilemap, wallTiles, tileName, position, color);
        }

        public void SetRandomWallLedgeTile(string tileName, Vector2Int position, Color? color = null)
        {
            SetRandomTile(wallTilemap, ledgeTiles, tileName, position, color);
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

        private void SetRandomTile(Tilemap tilemap, List<NamedTileBase> tileList, string tileListName, Vector2Int position, Color? color = null)
        {
            var collection = tileList.FirstOrDefault(tileList => tileList.Name == tileListName);
            if (collection == null)
                Debug.Log($"Tile with name '{tileListName}' not found in {nameof(tileList)} List");

            var random = Random.Range(0, collection.Tiles.Count);
            var tileBase = collection.Tiles[random];

            SetSingleTile(tilemap, tileBase, position, color);
        }

        public void Clear()
        {
            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();
        }
    }
}