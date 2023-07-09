using UnityEngine;

namespace Assets.Scripts.Characters.Player.Attacks
{
    public class ProjectileSpell : SpellBase
    {
        private float hitRange = 1f;

        protected override void Awake()
        {
            base.Awake();

            playerStats = FindObjectOfType<PlayerStats>();
            Destroy(gameObject, 1f);
        }

        private void FixedUpdate()
        {
            Move(playerStats.GetSpriteCenter());

            if (HitEnemy(hitRange))
                Destroy(gameObject);
        }

        private void Update()
        {
            transform.rotation = Quaternion.FromToRotation(transform.position, playerStats.transform.position - transform.position - new Vector3(0, -1, 0));
        }
    }
}