using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private int healthPoints = 10;

        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Damage(int damage)
        {
            healthPoints -= damage;

            if (healthPoints <= 0)
                animator.Play("Death");
            else
                animator.Play("TakeHit");
        }

    }
}