using UnityEngine;

namespace Assets.Scripts.Characters.Player.Attacks
{
    public class ProjectileSpell : BaseSpell
    {
        private float hitRange = 1f;

        protected override void Awake()
        {
            base.Awake();

            playerController = FindObjectOfType<PlayerController>();
            Destroy(gameObject, 1f);
        }

        private void FixedUpdate()
        {
            Move(playerController.GetSpriteCenter());

            if (HitEnemy(hitRange))
                Destroy(gameObject);
        }

        private void Update()
        {
            transform.rotation = Quaternion.FromToRotation(transform.position, playerController.transform.position - transform.position - new Vector3(0, -1, 0));
        }
    }
}