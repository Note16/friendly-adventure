using Assets.Scripts.Items;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        public GameObject UiScore;
        private PlayerStats playerStats;

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
        }

        private void FixedUpdate()
        {
            AggroItems();
        }

        public void AggroItems()
        {
            var consumeDistance = 0.5f;
            var aggroDistance = 5f;


            var gemObjects = FindObjectsOfType<Gem>();
            foreach (var gem in gemObjects)
            {
                var playerPosition = transform.position + new Vector3(0, 1f);
                // Find items within 5f radius
                if (Vector3.Distance(gem.transform.position, playerPosition) < consumeDistance)
                {
                    UiScore.GetComponent<Score>().UpdateScore(gem.PickUp());
                }
                else if (Vector3.Distance(gem.transform.position, playerPosition) < aggroDistance)
                {
                    gem.Move(playerPosition);
                }
            }

            var potionObjects = FindObjectsOfType<HealthPotion>();
            foreach (var potion in potionObjects)
            {
                var playerPosition = transform.position + new Vector3(0, 1f);
                // Find items within 5f radius
                if (Vector3.Distance(potion.transform.position, playerPosition) < consumeDistance)
                {
                    var heal = potion.Consume();
                    CombatTextPopup.Heal(transform, 1.4f, heal);
                    playerStats.Heal(heal);

                }
                else if (Vector3.Distance(potion.transform.position, playerPosition) < aggroDistance)
                {
                    potion.Move(playerPosition);
                }
            }
        }
    }
}