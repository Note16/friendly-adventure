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

        public void SetFloor(IEnumerable<Vector2Int> tiles, Color? color)
        {
            dungeonVisualizer.SetFloorTiles(tiles, color);
        }

        public void SetRoomNorthWall(RectInt wallRect)
        {
            var wallVectors = wallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetTopWallTileLocation(wallRect, position),
                    Position = position
                })
                .GroupBy(topWall => topWall.Position)
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
                        if (wall.Location == "Top")
                            dungeonVisualizer.SetWallTile("1_wall_north_pillar_top", wall.Position);

                        if (wall.Location == "Middle")
                            dungeonVisualizer.SetWallTile("1_wall_north_pillar_middle", wall.Position);

                        if (wall.Location == "Bottom")
                            dungeonVisualizer.SetWallLedgeTile("1_ledge_north_middle", wall.Position);
                    }
                    else
                    {
                        if (wall.Location == "Top")
                            dungeonVisualizer.SetWallTile("1_wall_north_top", wall.Position);

                        if (wall.Location == "Middle")
                            dungeonVisualizer.SetWallTile("1_wall_north_middle", wall.Position);

                        if (wall.Location == "Bottom")
                            dungeonVisualizer.SetWallLedgeTile("1_ledge_north_middle", wall.Position);
                    }
                }
            }
        }

        public void SetRoomSouthWall(RectInt wallRect)
        {
            foreach (var tile in wallRect.allPositionsWithin)
            {
                dungeonVisualizer.SetWallTile("1_wall_south_middle", tile);
            }
        }

        public void SetRoomWestWall(RectInt leftWallRect, RectInt topWallRect, RectInt bottomWallRect)
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
                    dungeonVisualizer.SetWallTile("1_wall_west_top", wall.Position);
                if (wall.Location == "Middle")
                    dungeonVisualizer.SetWallTile("1_wall_west_middle", wall.Position);
                if (wall.Location == "Bottom")
                    dungeonVisualizer.SetWallTile("1_wall_west_bottom", wall.Position);
                if (wall.Location == "Ledge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_west_middle", wall.Position);
                if (wall.Location == "CornerLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_north_left", wall.Position);
            }
        }

        public void SetRoomEastWall(RectInt rightWallRect, RectInt topWallRect, RectInt bottomWallRect)
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
                    dungeonVisualizer.SetWallTile("1_wall_east_top", wall.Position);
                if (wall.Location == "Middle")
                    dungeonVisualizer.SetWallTile("1_wall_east_middle", wall.Position);
                if (wall.Location == "Bottom")
                    dungeonVisualizer.SetWallTile("1_wall_east_bottom", wall.Position);
                if (wall.Location == "Ledge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_east_middle", wall.Position);
                if (wall.Location == "CornerLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_north_right", wall.Position);
            }
        }

        private string GetTopWallTileLocation(RectInt rectInt, Vector2Int vector2Int)
        {
            if (rectInt.y == vector2Int.y)
                return "Bottom";

            if (rectInt.y + rectInt.height - 1 == vector2Int.y)
                return "Top";

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