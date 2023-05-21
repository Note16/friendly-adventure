using Assets.Scripts.Characters.Shared;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private int healthPoints = 10;

        Animator animator;
        Movement movement;

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
            movement = new Movement(GetComponent<Rigidbody2D>());
        }

        public void Damage(int damage)
        {
            healthPoints -= damage;

            if (healthPoints <= 0)
                animator.Play("Death");
            else
                animator.Play("TakeHit");
        }

        public void Move(Vector3 targetPosition)
        {
            movement.Move((targetPosition - transform.position));
        }
    }
}