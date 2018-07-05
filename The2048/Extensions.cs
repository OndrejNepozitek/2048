namespace The2048
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Extension methods.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Gets a random element from a given collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="random"></param>
		/// <returns></returns>
		public static T GetRandom<T>(this IList<T> collection, Random random)
		{
			return collection[random.Next(collection.Count)];
		}
	}
}