namespace The2048.Game
{
	using System.Collections.Generic;

	public static class Helpers
	{
		public static byte GetMaxTile(ulong state)
		{
			byte max = 0;

			foreach (var tile in GetTiles(state))
			{
				if (tile > max)
				{
					max = tile;
				}
			}

			return max;
		}

		public static IEnumerable<byte> GetTiles(ulong state)
		{
			for (var i = 0; i < 16; i++)
			{
				yield return (byte)(state & 0xF);
				state >>= 4;
			}
		}
	}
}