using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    using Random = UnityEngine.Random;

    public static class Utilities
    {
        public static float Round(float value, float digits)
        {
            float mult = Mathf.Pow(10.0f, digits);
            return Mathf.Round(value * mult) / mult;
        }
        
        public static float BearingFromCoordinates(float lat1, float long1, float lat2, float long2)
        {
            lat1 *= Mathf.Deg2Rad;
            lat2 *= Mathf.Deg2Rad;
            long1 *= Mathf.Deg2Rad;
            long2 *= Mathf.Deg2Rad;

            float dLon = long2 - long1;
            float y = Mathf.Sin(dLon) * Mathf.Cos(lat2);
            float x = Mathf.Cos(lat1) * Mathf.Sin(lat2) - Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(dLon);
            float brng = Mathf.Atan2(y, x);

            brng = Mathf.Rad2Deg * brng;
            brng = (brng + 360) % 360;

            // brng = 360 - brng;
            brng = (brng + 180) % 360;
            return brng;
        }

        public static float BearingFromCoordinatesFor2DCompass(float lat1, float long1, float lat2, float long2)
        {
            lat1 *= Mathf.Deg2Rad;
            lat2 *= Mathf.Deg2Rad;
            long1 *= Mathf.Deg2Rad;
            long2 *= Mathf.Deg2Rad;

            float dLon = long2 - long1;
            float y = Mathf.Sin(dLon) * Mathf.Cos(lat2);
            float x = Mathf.Cos(lat1) * Mathf.Sin(lat2) - Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(dLon);
            float brng = Mathf.Atan2(y, x);

            brng = Mathf.Rad2Deg * brng;
            brng = (brng + 360) % 360;
            brng = 360 - brng;
            return brng;
        }

        public static float CalculatePercentage(float current, float max)
        {
            if (current > max)
            {
                current = max;
            }

            return current / max * 100;
        }

        public static bool CompareMultipleStrings(string data, StringComparison compareType, params string[] compareValues)
        {
            foreach (string s in compareValues)
            {
                if (data.Equals(s, compareType))
                {
                    return true;
                }
            }

            return false;
        }

        public static string DisplayTime(float theTime)
        {
            if (theTime <= 0)
            {
                theTime = 0;
            }

            int minutes = (int)Mathf.Floor(theTime / 60f);
            int seconds = (int)theTime - minutes * 60;
            string minutesString = minutes < 10 ? string.Format("0{0}", minutes) : minutes.ToString();
            string secondsString = seconds < 10 ? string.Format("0{0}", seconds) : seconds.ToString();

            return string.Format("{0}:{1}", minutesString, secondsString);
        }

        public static float DistanceBetweenCoordinates(float lat1, float lon1, float lat2, float lon2)
        {
            float R = 6371000; // metres
            float omega1 = lat1 / 180 * Mathf.PI;
            float omega2 = lat2 / 180 * Mathf.PI;
            float variacionomega1 = (lat2 - lat1) / 180 * Mathf.PI;
            float variacionomega2 = (lon2 - lon1) / 180 * Mathf.PI;
            float a = Mathf.Sin(variacionomega1 / 2) * Mathf.Sin(variacionomega1 / 2) + Mathf.Cos(omega1) * Mathf.Cos(omega2) * Mathf.Sin(variacionomega2 / 2) * Mathf.Sin(variacionomega2 / 2);
            float c = 2 * Mathf.Asin(Mathf.Sqrt(a));
            float d = R * c;
            return d;
        }

        public static float GetHigherVal(float value1, float value2)
        {
            return Mathf.Max(value1, value2);
        }

        public static int GetHighestVal(int value1, int value2, int value3)
        {
            return Math.Max(value1, Math.Max(value2, value3));
        }

        public static float GetLowerVal(float value1, float value2)
        {
            return Mathf.Min(value1, value2);
        }

        public static int GetLowestVal(int value1, int value2, int value3)
        {
            return Math.Min(value1, Math.Min(value2, value3));
        }

        public static float GetPitch(this Vector3 v)
        {
            float len = Mathf.Sqrt(v.x * v.x + v.z * v.z); // Length on xz plane.
            return -Mathf.Atan2(v.y, len);
        }

        public static float GetYaw(this Vector3 v)
        {
            return Mathf.Atan2(v.x, v.z);
        }

        public static Vector3 RandomCircle(Vector3 theCenter, float theRadius)
        {
            float anAngle = Random.value * 360;
            Vector3 aPosOnCircle = new Vector3(theCenter.x + theRadius * Mathf.Sin(anAngle * Mathf.Deg2Rad), theCenter.y + theRadius * Mathf.Cos(anAngle * Mathf.Deg2Rad), theCenter.z);
            return aPosOnCircle;
        }

        public static Vector3 PosOnCircle(Vector3 theCenter, float theRadius, float anAngle)
        {
            Vector3 aPosOnCircle = new Vector3(theCenter.x + theRadius * Mathf.Sin(anAngle * Mathf.Deg2Rad), theCenter.y + theRadius * Mathf.Cos(anAngle * Mathf.Deg2Rad), theCenter.z);
            return aPosOnCircle;
        }

        public static float RandomFloat(float a, float b)
        {
            float i = Random.Range(a, b);
            return i;
        }

        public static int RandomInt(int a, int b)
        {
            int i = Random.Range(a, b);
            return i;
        }

        public static float Remap(float input, float inputMin, float inputMax, float min, float max)
        {
            return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static void RemoveNulls<T>(this IList<T> collection)
            where T : class
        {
            for (var i = collection.Count - 1; i >= 0; i--)
            {
                if (collection[i] == null)
                {
                    collection.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Adds a dictionary to another dictionary, replacing values with the same keys.
        /// </summary>
        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // replace existing key with new value
                    source[item.Key] = item.Value;
                }
            }
        }

        public static void RotateX(this Vector3 v, float angle)
        {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            float ty = v.y;
            float tz = v.z;
            v.y = cos * ty - sin * tz;
            v.z = cos * tz + sin * ty;
        }

        public static void RotateY(this Vector3 v, float angle)
        {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            float tx = v.x;
            float tz = v.z;
            v.x = cos * tx + sin * tz;
            v.z = cos * tz - sin * tx;
        }

        public static void RotateZ(this Vector3 v, float angle)
        {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            float tx = v.x;
            float ty = v.y;
            v.x = cos * tx - sin * ty;
            v.y = cos * ty + sin * tx;
        }

        public static Color SetAlpha(this Color theColor, float theAlpha)
        {
            theColor.a = theAlpha;
            return theColor;
        }
    }

    public static class IListExtensions
    {
        /// <summary>
        /// Returns true if this list is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IList<T> items)
        {
            return items == null || !items.Any();
        }

        /// <summary>
        /// Returns true if this list is NOT null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool NotNullOrEmpty<T>(this IList<T> items)
        {
            return items != null && items.Any();
        }

#if !(UNITY_5_6 && UNITY_WINRT)

        /// <summary>
        /// Shuffle the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do
                {
                    provider.GetBytes(box);
                }
                while (!(box[0] < n * (byte.MaxValue / n)));

                int k = box[0] % n;
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
#endif
    }
}