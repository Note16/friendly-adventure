using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Score : MonoBehaviour
    {
        private TextMeshProUGUI textMesh;
        private int currentScore = 0;

        void Start()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        public void UpdateScore(int score)
        {
            currentScore += score;

            textMesh.SetText(currentScore.ToString());
        }
    }
}