﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.Helpers
{
    public static class RandomHelper
    {
        /// <summary>
        /// Get a Random value from given <c>List&lt;T&gt;</c>
        /// </summary>
        public static T GetRandom<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            var index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        /// <summary>
        /// Get a random value from given Enum
        /// </summary>
        public static T GetRandom<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
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
    }
}