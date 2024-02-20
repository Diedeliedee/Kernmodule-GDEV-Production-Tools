using System.Collections.Generic;
using UnityEngine;

namespace Joeri.Tools.Utilities
{
    /// <summary>
    /// Static class that holds some simple, but common calculation functions.
    /// </summary>
    public static class Util
    {
        /// <returnsThe 'current' integer, with 1 added to it. It loops back to zero once it reached it's max.></returns>
        public static int ScrollOne(int current, int max)
        {
            current++;
            if (current > max) current = 0;
            return current;
        }

        /// <returns>The 'current' integer, with 'amount' added to it, but loops around with 'max' as it's max value.</returns>
        public static int Scroll(int current, int amount, int max)
        {
            current += amount;
            current %= max;
            return current;
        }

        /// <returns>The passed in 'color', but with the passed in 'opacity'.</returns>
        public static Color SetOpacity(Color color, float opacity)
        {
            return new Color(color.r, color.g, color.b, opacity);
        }

        /// <returns>The passed in 'current' float, but swapped around according to the 'max' value.</returns>
        public static float Reverse(float current, float max)
        {
            current *= -1;
            current /= max;
            current += 1;
            current *= max;
            return current;
        }

        /// <returns>The passed in 0-1 integer, but swapped in value.</returns>
        public static float OneMinus(float current)
        {
            return current * -1 + 1;
        }

        /// <returns>Whether a list of colliders containts the desired component.</returns>
        public static bool Contains<T>(out T[] containingComponents, params Collider[] colliders)
        {
            var componentList = new List<T>();

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out T component)) componentList.Add(component);
            }
            containingComponents = componentList.ToArray();
            return containingComponents.Length > 0;
        }

        /// <returns>True if the passed in array is null, or has nothing in it.</returns>
        public static bool IsUnusableArray<T>(T[] array)
        {
            if (array == null || array.Length == 0) return true;
            return false;
        }

        /// <returns>True if the passed in list is null, or has nothing in it.</returns>
        public static bool IsUnusableList<T>(List<T> list)
        {
            if (list == null || list.Count == 0) return true;
            return false;
        }

        /// <returns>The result of a random number lower than, or equal to the given probability value.</returns>
        public static bool RandomChance(float probability)
        {
            probability = Mathf.Clamp01(probability);
            return (Random.Range(0f, 1f) <= probability);
        }

        /// <returns>A random point in a 2D circle.</returns>
        public static Vector2 RandomCirclePoint()
        {
            var r = Mathf.Sqrt(Random.Range(0f, 1f));
            var t = Random.Range(0f, 1f) * 2 * Mathf.PI;

            return new Vector2(r * Mathf.Cos(t), r * Mathf.Sin(t));
        }

        /// <returns>A random point in a 2D circle without using square root.</returns>
        public static Vector2 RandomCirclePoint(int accuracy)
        {
            var offset = Vector2.zero;

            for (int i = 0; i < accuracy; i++)
            {
                offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                if (offset.sqrMagnitude > 1f) continue;
                break;
            }
            return offset;
        }

        /// <returns>A "random" point in a 3D sphere, defined by the radius.</returns>
        public static Vector3 RandomSpherePoint(float radius = 1f)
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * radius;
        }
    }
}

