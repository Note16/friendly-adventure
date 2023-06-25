using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthGlobe : MonoBehaviour
    {
        private Image image;

        void Start()
        {
            image = GetComponent<Image>();
        }

        public void UpdateHealthGlobe(float fillAmount)
        {
            image.fillAmount = fillAmount;
        }
    }
}