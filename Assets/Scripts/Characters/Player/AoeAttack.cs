using Assets.Scripts.Characters.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    public class AoeAttack : MonoBehaviour
    {
        [SerializeField]
        public int damage = 4;

        void Awake()
        {
            var rb = GetComponent<Rigidbody2D>();
            var hits = new List<RaycastHit2D>();
            rb.Cast(Vector2.zero, hits);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.Damage(damage);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}