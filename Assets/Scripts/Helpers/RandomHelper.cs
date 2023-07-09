using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class RandomHelper
    {
        public static T GetRandom<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            var index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        public static T GetRandomEnum<T>(ICollection<T> exlusions) where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>();
            var possibleEnums = values.Where(i => !exlusions.Contains(i));
            return GetRandom(possibleEnums.ToList());
        }

        public static T GetRandomEnum<T>() where T : Enum
        {
            return GetRandomEnum(new List<T>());
        }

        /// <summary>
        /// Get a random bool by given percentage. Where 100% is always true and 0% is always false.
        /// </summary>
        public static bool GetRandom(int percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage));

            return UnityEngine.Random.value <= (float)percentage / 100;
        }

        /// <summary>
        /// Get a random position within bounds.
        /// </summary>
        public static Vector3 GetRandom(Bounds bounds)
        {
            return new Vector3(
                UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
                UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}