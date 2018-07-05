namespace The2048.AI.ExpectiMax
{
	using AI.Heuristics;

	/// <summary>
	/// Heuristic functions for ExpectiMax.
	/// </summary>
	public class Heuristics : AbstractHeuristics
	{
		public Heuristics()
		{
			var table = GenerateHeuristicsTable(EvaluateRow);
			SetHeuristicsTable(table);
		}

		private double EvaluateRow(byte[] row)
		{
			var score = 0d;

			score += HeuristicsHelpers.EmptyTiles(row) * 300;
			score += HeuristicsHelpers.LineMonotony(row, 4) * -4;
			score += HeuristicsHelpers.MaxAlongSidesWeighted(row, 2) * 2;
			score += HeuristicsHelpers.MaxAlongSides(row) * 100;
			score += HeuristicsHelpers.AdjacentTilesScore(row, 1) * -0.5;
			score += HeuristicsHelpers.AdjacentTiles(row) * 600;

			score += 100000;

			return score;
		}
	}
}