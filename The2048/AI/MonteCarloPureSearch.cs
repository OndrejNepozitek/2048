namespace The2048.AI
{
	using System;
	using Game;

	public class MonteCarloPureSearch : ISolver
	{
		private readonly IBoard board = new Board();
		private readonly int randomWalksCount;
		private readonly Random random = new Random();

		public MonteCarloPureSearch(int randomWalksCount)
		{
			this.randomWalksCount = randomWalksCount;
		}

		public Move GetBestMove(ulong state)
		{
			var bestMove = Move.Undefined;
			ulong bestScore = 0;

			if (board.IsTerminal(state))
			{
				throw new InvalidOperationException();
			}

			foreach(var move in board.GetPossibleMoves(state))
			{
				var score = EvaluateMove(state, move);

				if (score > bestScore)
				{
					bestMove = move;
					bestScore = score;
				}
			}

			return bestMove;
		}

		private ulong EvaluateMove(ulong state, Move move)
		{
			ulong scoreSum = 0; // Init simulation
			var stateAfterMove = board.PlayMove(state, move);

			for (var i = 0; i < randomWalksCount; i++)
			{
				var stateBackup = board.GenerateRandomTile(stateAfterMove);

				while (!board.IsTerminal(stateBackup))
				{
					var possibleMoves = board.GetPossibleMoves(stateBackup);
					var randomMove = possibleMoves.GetRandom(random);

					stateBackup = board.PlayAndGenerate(stateBackup, randomMove);
					scoreSum++;
				}

				// scoreSum += board.GetScore(stateBackup);
			}

			return scoreSum;
		}
	}
}