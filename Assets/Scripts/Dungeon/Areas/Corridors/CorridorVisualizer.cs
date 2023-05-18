using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Corridors
{
    public class CorridorVisualizer
    {
        private enum Location
        {
            Top,
            Middle,
            MiddleShade,
            RightCornerTop,
            RightCorner,
            LeftCornerTop,
            LeftCorner,
            RightLedge,
            RightCornerLedge,
            LeftLedge,
            LeftCornerLedge,
            BottomLedge,
            CornerLedgeTop,
            CornerLedge,
            CornerLedgeBottom,
            CornerTop,
            Corner
        }

        private readonly DungeonVisualizer dungeonVisualizer;

        public CorridorVisualizer(DungeonVisualizer dungeonVisualizer)
        {
            this.dungeonVisualizer = dungeonVisualizer;
        }

        public void SetFloor(RectInt rect, Color? color)
        {
            var area = rect.allPositionsWithin.ToVector2Int();
            foreach (var item in area)
            {
                if (item.x > rect.x && item.y > rect.y && item.x < rect.xMax - 1 && item.y < rect.yMax)
                    dungeonVisualizer.SetFloorTile(item, color);

            }
        }

        public void ClearWalls(IEnumerable<Vector2Int> tiles)
        {
            dungeonVisualizer.ClearWallTiles(tiles);
        }

        public void SetNorthWall(RectInt wallRect)
        {
            var wallVectors = wallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetNorthWallTileLocation(wallRect, position),
                    Position = position
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                var tileWallName = wall.Location switch
                {
                    Location.Top => "1_wall_north_top",
                    Location.Middle => "1_wall_north_middle",
                    Location.MiddleShade => "1_wall_north_middle_shade",
                    Location.RightCornerTop => "1_wall_north_right_top_corner",
                    Location.RightCorner => "1_wall_north_right_middle_corner",
                    Location.LeftCornerTop => "1_wall_north_left_top_corner",
                    Location.LeftCorner => "1_wall_north_left_middle_corner",
                    _ => null
                };
                if (tileWallName != null)
                {
                    dungeonVisualizer.SetRandomWallTile(tileWallName, wall.Position);
                    continue;
                }

                var tileLedgeName = wall.Location switch
                {
                    Location.RightLedge => "1_ledge_east_middle",
                    Location.RightCornerLedge => "1_ledge_north_right_corner",
                    Location.LeftLedge => "1_ledge_west_middle",
                    Location.LeftCornerLedge => "1_ledge_north_left_corner",
                    Location.BottomLedge => "1_ledge_north_middle",
                    _ => null
                };
                if (tileLedgeName != null)
                {
                    dungeonVisualizer.SetRandomWallLedgeTile(tileLedgeName, wall.Position);
                }
            }
        }

        public void SetSouthWall(RectInt wallRect)
        {
            var wallVectors = wallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetSouthWallTileLocation(wallRect, position),
                    Position = position
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                var tileWallName = wall.Location switch
                {
                    Location.LeftCorner => "1_wall_south_left_corner",
                    Location.Middle => "1_wall_south_middle",
                    Location.RightCorner => "1_wall_south_right_corner",
                    _ => null
                };
                if (tileWallName != null)
                {
                    dungeonVisualizer.SetRandomWallTile(tileWallName, wall.Position);
                    continue;
                }

                var tileLedgeName = wall.Location switch
                {
                    Location.RightCornerLedge => "1_ledge_south_right_corner",
                    Location.LeftCornerLedge => "1_ledge_south_left_corner",
                    _ => null
                };
                if (tileLedgeName != null)
                {
                    dungeonVisualizer.SetRandomWallLedgeTile(tileLedgeName, wall.Position);
                }
            }
        }

        public void SetWestWall(RectInt leftWallRect, RectInt overlapRoomSouth)
        {
            var wallVectors = leftWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetSideWallTileLocation("Left", leftWallRect, overlapRoomSouth, position),
                    Position = position
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                var tileWallName = wall.Location switch
                {
                    Location.Top => "1_wall_south_left_corner",
                    Location.Middle => "1_wall_west_middle",
                    Location.CornerTop => "1_wall_north_left_top_corner",
                    Location.Corner => "1_wall_north_left_middle_corner",
                    _ => null
                };
                if (tileWallName != null)
                {
                    dungeonVisualizer.SetRandomWallTile(tileWallName, wall.Position);
                    continue;
                }

                var tileLedgeName = wall.Location switch
                {
                    Location.BottomLedge => "1_ledge_north_middle",
                    Location.CornerLedgeTop => "1_ledge_south_left_corner",
                    Location.CornerLedge => "1_ledge_west_middle",
                    Location.CornerLedgeBottom => "1_ledge_north_left_corner",
                    _ => null
                };
                if (tileLedgeName != null)
                {
                    dungeonVisualizer.SetRandomWallLedgeTile(tileLedgeName, wall.Position);
                }
            }
        }

        public void SetEastWall(RectInt eastWallRect, RectInt overlapRoomSouth)
        {
            var wallVectors = eastWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetSideWallTileLocation("Right", eastWallRect, overlapRoomSouth, position),
                    Position = position
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                var tileWallName = wall.Location switch
                {
                    Location.Top => "1_wall_south_right_corner",
                    Location.Middle => "1_wall_east_middle",
                    Location.CornerTop => "1_wall_north_right_top_corner",
                    Location.Corner => "1_wall_north_right_middle_corner",
                    _ => null
                };
                if (tileWallName != null)
                {
                    dungeonVisualizer.SetRandomWallTile(tileWallName, wall.Position);
                    continue;
                }

                var tileLedgeName = wall.Location switch
                {
                    Location.BottomLedge => "1_ledge_north_middle",
                    Location.CornerLedgeTop => "1_ledge_south_right_corner",
                    Location.CornerLedge => "1_ledge_east_middle",
                    Location.CornerLedgeBottom => "1_ledge_north_right_corner",
                    _ => null
                };
                if (tileLedgeName != null)
                {
                    dungeonVisualizer.SetRandomWallLedgeTile(tileLedgeName, wall.Position);
                }
            }
        }

        private Location? GetNorthWallTileLocation(RectInt rectInt, Vector2Int vector2Int)
        {
            if (rectInt.y < vector2Int.y)
            {
                if (rectInt.x == vector2Int.x || rectInt.xMax - 1 == vector2Int.x)
                    return null;
            }

            if (rectInt.x == vector2Int.x)
                return Location.RightCornerLedge;

            if (rectInt.xMax - 1 == vector2Int.x)
                return Location.LeftCornerLedge;

            if (rectInt.y == vector2Int.y)
                return Location.BottomLedge;

            if (rectInt.x + 1 == vector2Int.x)
            {
                if (rectInt.yMax - 1 == vector2Int.y)
                    return Location.RightCornerTop;
                else
                    return Location.RightCorner;
            }

            if (rectInt.xMax - 2 == vector2Int.x)
            {
                if (rectInt.yMax - 1 == vector2Int.y)
                    return Location.LeftCornerTop;
                else
                    return Location.LeftCorner;
            }

            if (rectInt.y + rectInt.height - 1 == vector2Int.y)
                return Location.Top;

            if (rectInt.y + rectInt.height - 2 == vector2Int.y)
                return Location.MiddleShade;

            return Location.Middle;
        }

        private Location GetSouthWallTileLocation(RectInt rectInt, Vector2Int vector2Int)
        {
            if (rectInt.x == vector2Int.x)
                return Location.RightCornerLedge;

            if (rectInt.x + 1 == vector2Int.x)
                return Location.RightCorner;

            if (rectInt.xMax - 1 == vector2Int.x)
                return Location.LeftCornerLedge;

            if (rectInt.xMax - 2 == vector2Int.x)
                return Location.LeftCorner;

            return Location.Middle;
        }

        private Location GetSideWallTileLocation(string side, RectInt targetWallRect, RectInt overlapRoomSouth, Vector2Int vector2Int)
        {
            if ((targetWallRect.x < vector2Int.x && side == "Left") || (targetWallRect.x == vector2Int.x && side == "Right"))
            {
                if (targetWallRect.yMax - 1 == vector2Int.y)
                    return Location.CornerLedgeTop;

                if (targetWallRect.yMin == vector2Int.y)
                    return Location.CornerLedgeBottom;

                return Location.CornerLedge;
            }

            if (overlapRoomSouth.Contains(vector2Int))
            {
                if (overlapRoomSouth.yMax - 1 == vector2Int.y)
                    return Location.CornerTop;

                if (targetWallRect.y == vector2Int.y)
                    return Location.BottomLedge;

                return Location.Corner;
            }

            if (targetWallRect.y + targetWallRect.height - 1 == vector2Int.y)
                return Location.Top;

            return Location.Middle;
        }
    }
}