namespace The2048.AI.Heuristics
{
	using System;
	using Game;

	public abstract class AbstractHeuristics : IHeuristics
	{
		private double[] heuristicsTable;

		protected void SetHeuristicsTable(double[] heuristicsTable)
		{
			this.heuristicsTable = heuristicsTable;
		}

		protected double[] GenerateHeuristicsTable(Func<byte[], double> rowEvaluation)
		{
			var heuristicTable = new double[65536];

			for (var i = 0; i < 65535; i++)
			{
				var row = new byte[4];

				for (byte k = 0; k < 4; k++)
				{
					row[k] = (byte)((i >> k * 4) & 15);
				}

				heuristicTable[i] = rowEvaluation(row);
			}

			return heuristicTable;
		}

		public double EvaluateState(ulong state)
		{
			double score = 0;

			score += heuristicsTable[(state & 0xFFFF)]
			         + heuristicsTable[(state >> 16) & 0xFFFF]
			         + heuristicsTable[(state >> 32) & 0xFFFF]
			         + heuristicsTable[(state >> 48) & 0xFFFF];
			state = Helpers.Transpose(state);
			score += heuristicsTable[(state & 0xFFFF)]
			         + heuristicsTable[(state >> 16) & 0xFFFF]
			         + heuristicsTable[(state >> 32) & 0xFFFF]
			         + heuristicsTable[(state >> 48) & 0xFFFF];

			return score;
		}
	}
}