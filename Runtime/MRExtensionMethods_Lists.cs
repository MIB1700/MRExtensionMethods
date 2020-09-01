using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// https://docs.microsoft.com/en-us/dotnet/csharp/codedoc
// https://www.youtube.com/watch?v=mr5xkf6zSzk

//TODO:
//  organize code
//  document
//  remove useless stuff
//  figure out consistency

/// <summary>
/// The main MRExtensionsMethods class
/// Uses the MR.CustomExtensions namespace
///
/// Don't forget to use:
///     using MR.CustomExtensions
/// in your code!
/// </summary>
namespace MR.CustomExtensions
{
    public static class MRExtensionMethods_Lists
    {
/*******************************************************************/
//
//                      List Functions
//
/*******************************************************************/
#region
        public static void ClearAndDestroyAllGos(this List<GameObject> gos)
        {
            foreach (GameObject go in gos)
            {
                if (Application.isEditor)
                {
                    Object.DestroyImmediate(go);
                }
                else
                {
                    Object.Destroy(go, 0f);
                }
            }
            gos.Clear();
        }
/*******************************************************************/
        /// <summary>
        /// Initialize a List<T> with a default value
        /// </summary>
        /// <param name="lst">List to fill.</param>
        /// <param name="val">Value that will be added to list.</param>
        /// <example>
        /// <code>
        /// List<int> x  = new List<int>(10); x.Fill();
        /// List<int> x  = new List<int>(10); x.Fill(44);
        /// List<float> x  = new List<float>(10); x.Fill(74.47);
        /// List<GameObject> x  = new List<GameObject>(10); x.Fill(BulletPrefab);
        /// </code>
        /// </example>
        public static void Fill<T>(this List<T> lst, T val)
        {
            // lst = Enumerable.Repeat(val, lst.Capacity).ToList();
            for (int i = 0; i < lst.Capacity; i++)
            {
                lst.Add(val);
            }
        }
/*******************************************************************/
        /// <remarks>
        /// default(T) will return 0 for numeric types and null for reference types!
        /// </remarks>
        public static void Fill<T>(this List<T> lst)
        {
            Fill(lst, default(T));
        }
/*******************************************************************/
    /// <summary>
    /// Return a random item from the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("empty list");

        return list[UnityEngine.Random.Range(0, list.Count)];
    }
/*******************************************************************/
    /// <summary>
    /// Return a random item from the list and remove it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomItemRemove<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("empty list");

        int rand = UnityEngine.Random.Range(0, list.Count);
        var item = list[rand];
        list.RemoveAt(rand);

        return item;
    }
/*******************************************************************/
#endregion
    }
}