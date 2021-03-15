using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.Utils
{
	public static class ListCleaner
	{
		public delegate bool Filter<T>(T obj);
 
		/// <summary>
		/// Removes elements from a list using a delegate filter. Return "false" to keep an element, and return "true" to remove it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="filter"></param>
		public static void CleanList<T>(List<T> list, Filter<T> filter)
        {
			List<T> toRemove = new List<T>();
			foreach (T obj in list)
            {
				if (filter(obj))
                {
					toRemove.Add(obj);
                }
            }
			foreach (T obj in toRemove)
            {
				list.Remove(obj);
            }
        }
	}
}