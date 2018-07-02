namespace The2048
{
	using System;
	using System.Collections.Generic;

	public static class Extensions
	{
		public static T GetRandom<T>(this IList<T> collection, Random random)
		{
			return collection[random.Next(collection.Count)];
		}
	}
}