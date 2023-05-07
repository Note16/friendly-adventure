using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Corridors
{
    public class CorridorVisualizer
    {
        private readonly DungeonVisualizer dungeonVisualizer;

        public CorridorVisualizer(DungeonVisualizer dungeonVisualizer)
        {
            this.dungeonVisualizer = dungeonVisualizer;
        }

        public void SetFloor(IEnumerable<Vector2Int> tiles, Color? color)
        {
            dungeonVisualizer.SetFloorTiles(tiles, color);
        }

        public void ClearWalls(IEnumerable<Vector2Int> tiles)
        {
            dungeonVisualizer.ClearWallTiles(tiles);
        }

        public void SetCorridorWestWall(RectInt leftWallRect, RectInt overlapRoomSouth)
        {
            var wallVectors = leftWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetCorridorSideWallTileLocation("Left", leftWallRect, overlapRoomSouth, position),
                    Position = position
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                if (wall.Location == "Top")
                    dungeonVisualizer.SetWallTile("1_wall_south_left_corner", wall.Position);
                if (wall.Location == "Middle")
                    dungeonVisualizer.SetWallTile("1_wall_west_middle", wall.Position);
                if (wall.Location == "TopCornerBottom")
                    dungeonVisualizer.SetWallTile("1_wall_north_left_top_corner", wall.Position);
                if (wall.Location == "MiddleCornerBottom")
                    dungeonVisualizer.SetWallTile("1_wall_north_left_middle_corner", wall.Position);
                if (wall.Location == "BottomLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_north_middle", wall.Position);
                if (wall.Location == "TopCornerLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_south_left_corner", wall.Position);
                if (wall.Location == "MiddleLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_west_middle", wall.Position);
                if (wall.Location == "BottomCornerLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_north_left_corner", wall.Position);
            }
        }

        public void SetCorridorEastWall(RectInt eastWallRect, RectInt overlapRoomSouth)
        {
            var wallVectors = eastWallRect.allPositionsWithin.ToVector2Int();

            // Group tiles by xAxis and assign every 4th item as Pillar type
            var groupedWalls = wallVectors
                .Select(position => new
                {
                    Location = GetCorridorSideWallTileLocation("Right", eastWallRect, overlapRoomSouth, position),
                    Position = position
                });

            // Set wall tiles
            foreach (var wall in groupedWalls)
            {
                if (wall.Location == "Top")
                    dungeonVisualizer.SetWallTile("1_wall_south_right_corner", wall.Position);
                if (wall.Location == "Middle")
                    dungeonVisualizer.SetWallTile("1_wall_east_middle", wall.Position);
                if (wall.Location == "TopCornerBottom")
                    dungeonVisualizer.SetWallTile("1_wall_north_right_top_corner", wall.Position);
                if (wall.Location == "MiddleCornerBottom")
                    dungeonVisualizer.SetWallTile("1_wall_north_right_middle_corner", wall.Position);
                if (wall.Location == "BottomLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_north_middle", wall.Position);
                if (wall.Location == "TopCornerLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_south_right_corner", wall.Position);
                if (wall.Location == "MiddleLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_east_middle", wall.Position);
                if (wall.Location == "BottomCornerLedge")
                    dungeonVisualizer.SetWallLedgeTile("1_ledge_north_right_corner", wall.Position);
            }
        }

        private string GetCorridorSideWallTileLocation(string side, RectInt targetWallRect, RectInt overlapRoomSouth, Vector2Int vector2Int)
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