using Assets.Scripts.Characters.Enemies;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private void FixedUpdate()
        {
            AggroMobs();
        }

        public void AggroMobs()
        {
            var aggroDistance = 10f;

            var enemyObjects = FindObjectsOfType<BaseEnemy>();
            foreach (var enemy in enemyObjects)
            {
                // Find enemies within 10f radius
                if (Vector3.Distance(enemy.transform.position, transform.position) < aggroDistance)
                {
                    enemy.Move(transform.position);
                }
                else
                {
                    enemy.StopMovement();
                }
            }
        }
    }
}