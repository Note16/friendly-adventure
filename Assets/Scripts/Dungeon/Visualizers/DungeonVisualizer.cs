using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Dungeon.Visualizers
{
    [Serializable]
    public class NamedTileBase
    {
        public string Name;
        public TileBase TileBase;
    }

    public class DungeonVisualizer : TilemapVisualizer
    {
        [SerializeField]
        private TileBase floorTile;

        [SerializeField]
        private List<NamedTileBase> wallTiles;

        [SerializeField]
        private List<NamedTileBase> ledgeTiles;

        public void SetFloorTiles(IEnumerable<Vector2Int> floor, Color? color)
        {
            SetFloorTiles(floorTile, floor, color);
        }

        public void ClearWallTiles(IEnumerable<Vector2Int> wallPositions)
        {
            foreach (var position in wallPositions)
            {
                ClearWallTile(position);
            }
        }

        public void SetRoomNorthWall(RectInt wallRect)
        {
            var wallVectors = wallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(tile => new
                {
                    Position = GetTopWallTilePosition(wallRect, tile),
                    Tile = tile
                })
                .GroupBy(topWall => topWall.Tile)
                .Select((group, i) =>
                {
                    return new
                    {
                        Type = (i % 4 == 0) ? "Pillar" : "Wall",
                        TopWall = group
                    };
                });

            // Set wall tiles
            foreach (var group in groupedWalls)
            {
                foreach (var wall in group.TopWall)
                {
                    if (group.Type == "Pillar")
                    {
                        if (wall.Position == "Top")
                            SetWallTile(wallTiles.First(wallTile => wallTile.Name == "1_wall_north_pillar_top").TileBase, wall.Tile);

                        if (wall.Position == "Middle")
                            SetWallTile(wallTiles.First(wallTile => wallTile.Name == "1_wall_north_pillar_middle").TileBase, wall.Tile);

                        if (wall.Position == "Bottom")
                            SetWallTile(ledgeTiles.First(wallTile => wallTile.Name == "1_ledge_north_middle").TileBase, wall.Tile);
                    }
                    else
                    {
                        if (wall.Position == "Top")
                            SetWallTile(wallTiles.First(wallTile => wallTile.Name == "1_wall_north_top").TileBase, wall.Tile);

                        if (wall.Position == "Middle")
                            SetWallTile(wallTiles.First(wallTile => wallTile.Name == "1_wall_north_middle").TileBase, wall.Tile);

                        if (wall.Position == "Bottom")
                            SetWallTile(ledgeTiles.First(wallTile => wallTile.Name == "1_ledge_north_middle").TileBase, wall.Tile);
                    }
                }
            }
        }

        public void SetRoomSouthWall(RectInt wallRect)
        {
            foreach (var tile in wallRect.allPositionsWithin)
            {
                SetWallTile(wallTiles.First(wallTile => wallTile.Name == "1_wall_south_middle").TileBase, tile);
            }
        }

        public void SetRoomWestWall(RectInt leftWallRect, RectInt topWallRect, RectInt bottomWallRect)
        {
            var wallVectors = leftWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(tile => new
                {
                    Position = GetSideWallTilePosition("Left", leftWallRect, topWallRect, bottomWallRect, tile),
                    Tile = tile
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                if (wall.Position == "Top")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_west_top").TileBase, wall.Tile);
                if (wall.Position == "Middle")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_west_middle").TileBase, wall.Tile);
                if (wall.Position == "Bottom")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_west_bottom").TileBase, wall.Tile);
                if (wall.Position == "Ledge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_west_middle").TileBase, wall.Tile);
                if (wall.Position == "CornerLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_north_left").TileBase, wall.Tile);
            }
        }

        public void SetCorridorWestWall(RectInt leftWallRect, RectInt overlapRoomSouth)
        {
            var wallVectors = leftWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(tile => new
                {
                    Position = GetCorridorSideWallTilePosition("Left", leftWallRect, overlapRoomSouth, tile),
                    Tile = tile
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                if (wall.Position == "Top")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_south_left_corner").TileBase, wall.Tile);
                if (wall.Position == "Middle")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_west_middle").TileBase, wall.Tile);
                if (wall.Position == "TopCornerBottom")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_north_left_top_corner").TileBase, wall.Tile);
                if (wall.Position == "MiddleCornerBottom")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_north_left_middle_corner").TileBase, wall.Tile);
                if (wall.Position == "BottomLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_north_middle").TileBase, wall.Tile);
                if (wall.Position == "TopCornerLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_south_left_corner").TileBase, wall.Tile);
                if (wall.Position == "MiddleLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_west_middle").TileBase, wall.Tile);
                if (wall.Position == "BottomCornerLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_north_left_corner").TileBase, wall.Tile);
            }
        }

        public void SetCorridorEastWall(RectInt eastWallRect, RectInt overlapRoomSouth)
        {
            var wallVectors = eastWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(tile => new
                {
                    Position = GetCorridorSideWallTilePosition("Right", eastWallRect, overlapRoomSouth, tile),
                    Tile = tile
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                if (wall.Position == "Top")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_south_right_corner").TileBase, wall.Tile);
                if (wall.Position == "Middle")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_east_middle").TileBase, wall.Tile);
                if (wall.Position == "TopCornerBottom")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_north_right_top_corner").TileBase, wall.Tile);
                if (wall.Position == "MiddleCornerBottom")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_north_right_middle_corner").TileBase, wall.Tile);
                if (wall.Position == "BottomLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_north_middle").TileBase, wall.Tile);
                if (wall.Position == "TopCornerLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_south_right_corner").TileBase, wall.Tile);
                if (wall.Position == "MiddleLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_east_middle").TileBase, wall.Tile);
                if (wall.Position == "BottomCornerLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_north_right_corner").TileBase, wall.Tile);
            }
        }

        public void SetRoomEastWall(RectInt rightWallRect, RectInt topWallRect, RectInt bottomWallRect)
        {
            var wallVectors = rightWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(tile => new
                {
                    Position = GetSideWallTilePosition("Right", rightWallRect, topWallRect, bottomWallRect, tile),
                    Tile = tile
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                if (wall.Position == "Top")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_east_top").TileBase, wall.Tile);
                if (wall.Position == "Middle")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_east_middle").TileBase, wall.Tile);
                if (wall.Position == "Bottom")
                    SetWallTile(wallTiles.First(lw => lw.Name == "1_wall_east_bottom").TileBase, wall.Tile);
                if (wall.Position == "Ledge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_east_middle").TileBase, wall.Tile);
                if (wall.Position == "CornerLedge")
                    SetWallTile(ledgeTiles.First(lw => lw.Name == "1_ledge_north_right").TileBase, wall.Tile);
            }
        }

        private string GetTopWallTilePosition(RectInt rectInt, Vector2Int vector2Int)
        {
            if (rectInt.y == vector2Int.y)
                return "Bottom";

            if (rectInt.y + rectInt.height - 1 == vector2Int.y)
                return "Top";

            return "Middle";
        }

        private string GetSideWallTilePosition(string side, RectInt targetWallRect, RectInt topWallRect, RectInt bottomWallRect, Vector2Int vector2Int)
        {
            if ((targetWallRect.x < vector2Int.x && side == "Left") || (targetWallRect.x == vector2Int.x && side == "Right"))
            {
                if (topWallRect.Contains(vector2Int))
                {
                    if (topWallRect.yMin == vector2Int.y)
                    {
                        return "CornerLedge";
                    }
                }
                else if (!bottomWallRect.Contains(vector2Int))
                {
                    return "Ledge";
                }

                return "";
            }

            if (targetWallRect.y == vector2Int.y)
                return "Bottom";

            if (targetWallRect.y + targetWallRect.height - 1 == vector2Int.y)
                return "Top";

            return "Middle";
        }

        private string GetCorridorSideWallTilePosition(string side, RectInt targetWallRect, RectInt overlapRoomSouth, Vector2Int vector2Int)
        {
            if ((targetWallRect.x < vector2Int.x && side == "Left") || (targetWallRect.x == vector2Int.x && side == "Right"))
            {
                if (targetWallRect.yMax - 1 == vector2Int.y)
                    return "TopCornerLedge";

                if (targetWallRect.yMin == vector2Int.y)
                    return "BottomCornerLedge";

                return "MiddleLedge";
            }

            if (overlapRoomSouth.Contains(vector2Int))
            {
                if (overlapRoomSouth.yMax - 1 == vector2Int.y)
                    return "TopCornerBottom";

                if (overlapRoomSouth.yMax - 1 == vector2Int.y)
                    return "TopCornerBottom";

                if (targetWallRect.y == vector2Int.y)
                    return "BottomLedge";

                return "MiddleCornerBottom";
            }

            if (targetWallRect.y + targetWallRect.height - 1 == vector2Int.y)
                return "Top";

            return "Middle";
        }
    }
}