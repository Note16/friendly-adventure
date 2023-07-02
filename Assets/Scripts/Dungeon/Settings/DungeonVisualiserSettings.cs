using Assets.Scripts.Dungeon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DungeonVisualiserSettings", menuName = "ScriptableObjects/DungeonVisualiserSettings", order = 1)]
public class DungeonVisualiserSettings : ScriptableObject
{
    [SerializeField]
    public TileBase darkTile;

    [SerializeField]
    public TileBase floorTile;

    [SerializeField]
    public List<NamedTileBase> wallTiles;

    [SerializeField]
    public List<NamedTileBase> ledgeTiles;
    public List<NamedTileBase> ledgeTiles2;
}