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
            for (int i = transform.childCount; i > 0; --i)
            {
                if (Application.IsPlaying(transform.GetChild(0).gameObject))
                    Object.Destroy(transform.GetChild(0).gameObject);
                else
                    Object.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}