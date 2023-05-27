using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class GameObjectHelper
    {
        public static void Destroy<T>(IEnumerable<T> objects) where T : Object
        {
            if (objects == null)
                return;

            foreach (var gameObject in objects)
            {
                if (gameObject == null)
                    continue;

                if (Application.IsPlaying(gameObject))
                    Object.Destroy(gameObject);
                else
                    Object.DestroyImmediate(gameObject);
            }
        }

        public static void DestroyChildren(Transform transform)
        {
            if (Application.IsPlaying(transform.gameObject))
            {
                foreach (Transform child in transform)
                {
                    Object.Destroy(child.gameObject);
                }
            }
            else
            {
                for (int i = transform.childCount; i > 0; --i)
                {
                    Object.DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }
        }
    }
}