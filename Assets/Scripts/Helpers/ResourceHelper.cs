using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class ResourceHelper
    {
        public static GameObject CombatText => (GameObject)Resources.Load("CombatText/TextPopup", typeof(GameObject));
    }
}