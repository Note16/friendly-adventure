using UnityEngine;

namespace Assets.Scripts.Dungeon.Objects
{
    public class Exit : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                GetComponentInParent<DungeonGenerator>().GenerateDungeon();
            }
        }
    }
}