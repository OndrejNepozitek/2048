namespace The2048.AI.Heuristics
{
	using System;
	using System.Linq;

	/// <summary>
	/// Collection of useful heuristics.
	/// </summary>
	public static class HeuristicsHelpers
	{
		public static double LineMonotony(byte[] row, double power)
		{
			double monotonicityLeft = 0;
			double monotonicityRight = 0;

			for (var i = 1; i < 4; ++i)
			{
				if (row[i - 1] > row[i])
				{
					monotonicityLeft += Math.Pow(row[i - 1], power) - Math.Pow(row[i], power);
				}
				else
				{
					monotonicityRight += Math.Pow(row[i], power) - Math.Pow(row[i - 1], power);
				}
			}

			return Math.Min(monotonicityLeft, monotonicityRight);
		}

		public static int EmptyTiles(byte[] row)
		{
			return row.Sum(x => x == 0 ? 1 : 0);
		}

		public static double MaxAlongSidesWeighted(byte[] row, double power)
		{
			double score = 0;

			if (row[0] != 0
			    && row[0] >= row[1]
			    && row[0] >= row[2]
			    && row[0] >= row[3])
			{
				score += Math.Pow(row[0], power);
			}

			if (row[3] != 0
			    && row[3] >= row[1]
			    && row[3] >= row[2]
			    && row[3] >= row[0])
			{
				score += Math.Pow(row[3], power);
			}

			return score;
		}

		public static int MaxAlongSides(byte[] row)
		{
			var score = 0;

			if (row[0] != 0
			    && row[0] >= row[1]
			    && row[0] >= row[2]
			    && row[0] >= row[3])
			{
				score++;
			}

			if (row[3] != 0
			    && row[3] >= row[1]
			    && row[3] >= row[2]
			    && row[3] >= row[0])
			{
				score++;
			}

			return score;
		}

		public static double AdjacentTilesScore(byte[] row, double power)
		{
			double score = 0;

			for (var i = 0; i < 3; i++)
			{
				if (row[i] != 0 && row[i] != row[i + 1])
				{
					score += Math.Pow(Math.Abs(row[i + 1] - row[i]), power);
				}
			}

			return score;
		}

		public static int AdjacentTiles(byte[] row)
		{
			var score = 0;

			for (var i = 0; i < 3; i++)
			{
				if (row[i] != 0 && row[i] == row[i + 1])
				{
					score++;
				}
			}

			return score;
		}
	}
}