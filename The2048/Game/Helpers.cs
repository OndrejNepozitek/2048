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

		/// <summary>
		/// Efficient transposition using bitwise operations.
		/// </summary>
		/// <param name="state">Current state</param>
		/// <returns>Transposed state</returns>
		public static ulong Transpose(ulong state)
		{
			var a1 = state & 0xF0F00F0FF0F00F0FL;
			var a2 = state & 0x0000F0F00000F0F0L;
			var a3 = state & 0x0F0F00000F0F0000L;
			var a = a1 | (a2 << 12) | (a3 >> 12);
			var b1 = a & 0xFF00FF0000FF00FFL;
			var b2 = a & 0x00FF00FF00000000L;
			var b3 = a & 0x00000000FF00FF00L;

			return b1 | (b2 >> 24) | (b3 << 24);
		}
	}
}