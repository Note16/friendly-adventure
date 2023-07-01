using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Rooms
{
    public class RoomVisualizer
    {
        private readonly DungeonVisualizer dungeonVisualizer;

        public RoomVisualizer(DungeonVisualizer dungeonVisualizer)
        {
            this.dungeonVisualizer = dungeonVisualizer;
        }

        public void SetFloor(IEnumerable<Vector2Int> tiles)
        {
            dungeonVisualizer.SetFloorTiles(tiles);
        }

        public void ClearWallCollider(IEnumerable<Vector2Int> tiles)
        {
            dungeonVisualizer.ClearWallCollider(tiles);
        }

        public void SetNorthWall(RectInt wallRect, int pillarDistance)
        {
            var wallVectors = wallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetNorthWallTileLocation(wallRect, position),
                    Position = position
                })
                .GroupBy(topWall => topWall.Position.x)
                .Select((group, i) =>
                {
                    return new
                    {
                        Type = (i % pillarDistance == 0) ? "Pillar" : "Wall",
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
                        if (wall.Location == "Top")
                            dungeonVisualizer.SetRandomWallTile("1_wall_north_pillar_top", wall.Position);

                        if (wall.Location == "MiddleShade")
                            dungeonVisualizer.SetRandomWallTile("1_wall_north_pillar_middle", wall.Position);

                        if (wall.Location == "Middle")
                            dungeonVisualizer.SetRandomWallTile("1_wall_north_pillar_middle", wall.Position);

                        if (wall.Location == "Bottom")
                            dungeonVisualizer.SetRandomWallLedgeTile("1_ledge_north_middle", wall.Position);
                    }
                    else
                    {
                        if (wall.Location == "Top")
                            dungeonVisualizer.SetRandomWallTile("1_wall_north_top", wall.Position);

                        if (wall.Location == "MiddleShade")
                            dungeonVisualizer.SetRandomWallTile("1_wall_north_middle_shade", wall.Position);

                        if (wall.Location == "Middle")
                            dungeonVisualizer.SetRandomWallTile("1_wall_north_middle", wall.Position);

                        if (wall.Location == "Bottom")
                            dungeonVisualizer.SetRandomWallLedgeTile("1_ledge_north_middle", wall.Position);
                    }
                }
            }
        }

        public void SetSouthWall(RectInt wallRect)
        {
            foreach (var tile in wallRect.allPositionsWithin)
            {
                dungeonVisualizer.SetRandomWallTile("1_wall_south_middle", tile);
            }
        }

        public void SetWestWall(RectInt leftWallRect, RectInt topWallRect, RectInt bottomWallRect)
        {
            var wallVectors = leftWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetSideWallTileLocation("Left", leftWallRect, topWallRect, bottomWallRect, position),
                    Position = position
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                if (wall.Location == "Top")
                    dungeonVisualizer.SetRandomWallTile("1_wall_west_top", wall.Position);
                if (wall.Location == "Middle")
                    dungeonVisualizer.SetRandomWallTile("1_wall_west_middle", wall.Position);
                if (wall.Location == "Bottom")
                    dungeonVisualizer.SetRandomWallTile("1_wall_west_bottom", wall.Position);
                if (wall.Location == "Ledge")
                    dungeonVisualizer.SetRandomWallLedgeTile("1_ledge_west_middle", wall.Position);
                if (wall.Location == "CornerLedge")
                    dungeonVisualizer.SetRandomWallLedgeTile("1_ledge_north_left", wall.Position);
            }
        }

        public void SetEastWall(RectInt rightWallRect, RectInt topWallRect, RectInt bottomWallRect)
        {
            var wallVectors = rightWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetSideWallTileLocation("Right", rightWallRect, topWallRect, bottomWallRect, position),
                    Position = position
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                if (wall.Location == "Top")
                    dungeonVisualizer.SetRandomWallTile("1_wall_east_top", wall.Position);
                if (wall.Location == "Middle")
                    dungeonVisualizer.SetRandomWallTile("1_wall_east_middle", wall.Position);
                if (wall.Location == "Bottom")
                    dungeonVisualizer.SetRandomWallTile("1_wall_east_bottom", wall.Position);
                if (wall.Location == "Ledge")
                    dungeonVisualizer.SetRandomWallLedgeTile("1_ledge_east_middle", wall.Position);
                if (wall.Location == "CornerLedge")
                    dungeonVisualizer.SetRandomWallLedgeTile("1_ledge_north_right", wall.Position);
            }
        }

        private string GetNorthWallTileLocation(RectInt rectInt, Vector2Int vector2Int)
        {
            if (rectInt.y == vector2Int.y)
                return "Bottom";

            if (rectInt.y + rectInt.height - 1 == vector2Int.y)
                return "Top";

            if (rectInt.y + rectInt.height - 2 == vector2Int.y)
                return "MiddleShade";

            return "Middle";
        }

        private string GetSideWallTileLocation(string side, RectInt targetWallRect, RectInt topWallRect, RectInt bottomWallRect, Vector2Int vector2Int)
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
    }
}