using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class DamagePopup : MonoBehaviour
    {
        private const float disappearTimerMax = 0.6f;

        private TextMeshPro textMesh;
        private float disappearTimer;
        private Color textColor;
        private Vector3 moveVector;

        public static void Create(Transform parent, float yAxis, int damage, bool isCritical)
        {
            var damagePopUpTransform = Instantiate(ResourceHelper.CombatText, parent.position + new Vector3(0, 1.7f), Quaternion.identity, parent);
            var damagePopup = damagePopUpTransform.GetComponent<DamagePopup>();
            damagePopup.Setup(yAxis, damage, isCritical);
        }

        private static int sortingOrder;

        void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        public void Setup(float yAxis, int damageAmount, bool isCritical)
        {
            textMesh.SetText(damageAmount.ToString());

            if (isCritical)
            {
                textMesh.fontSize = 8;
                textColor = Color.yellow;
            }
            else
            {
                textMesh.fontSize = 6;
                textColor = Color.white;
            }
            textMesh.color = textColor;
            disappearTimer = disappearTimerMax;

            sortingOrder++;
            textMesh.sortingOrder = sortingOrder;

            moveVector = new Vector3(Random.value, yAxis) * 10f;
        }

        private void Update()
        {
            transform.position += moveVector * Time.deltaTime * 1.2f;
            moveVector -= moveVector * 10 * Time.deltaTime * 1.2f;

            if (disappearTimer > disappearTimerMax * .3f)
            {
                // First half of the popup lifetime
                float increaseScaleAmount = 0.7f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                float decreaseScaleAmount = 0.7f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }

            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0)
            {
                float disappearSpeed = 2f;
                textColor.a -= disappearSpeed * Time.deltaTime;
                textMesh.color = textColor;
                if (textColor.a <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}