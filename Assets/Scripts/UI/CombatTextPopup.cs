using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CombatTextPopup : MonoBehaviour
    {
        private static int sortingOrder;
        private const float disappearTimerMax = 0.6f;

        private TextMeshPro textMesh;
        private float disappearTimer;
        private Color textColor;
        private Vector3 moveVector;

        public static void Damage(Transform parent, float yAxis, int damage, bool isCritical)
        {
            var textPopUpTransform = Instantiate(ResourceHelper.CombatText, parent.position + new Vector3(0, 1.7f), Quaternion.identity, parent);
            var textPopup = textPopUpTransform.GetComponent<CombatTextPopup>();
            if (isCritical)
            {
                textPopup.Setup(yAxis, damage.ToString(), 8, Color.yellow);
            }
            else
            {
                textPopup.Setup(yAxis, damage.ToString(), 6, Color.white);
            }
        }

        public static void Heal(Transform parent, float yAxis, int damage)
        {
            var textPopUpTransform = Instantiate(ResourceHelper.CombatText, parent.position + new Vector3(0, 1.7f), Quaternion.identity, parent);
            var textPopup = textPopUpTransform.GetComponent<CombatTextPopup>();
            textPopup.Setup(yAxis, damage.ToString(), 8, Color.green);
        }


        void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        public void Setup(float yAxis, string text, int fontSize, Color color)
        {
            textMesh.SetText(text);
            textMesh.fontSize = fontSize;
            textMesh.color = color;
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