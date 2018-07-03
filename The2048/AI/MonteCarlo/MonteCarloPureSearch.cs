namespace The2048.AI.MonteCarlo
{
	using System;
	using System.Diagnostics;
	using Game;
	using The2048.AI.MonteCarlo.Modes;

	public class MonteCarloPureSearch : ISolver
	{
		private readonly IBoard board = new Board();
		private readonly int randomWalksCount;
		private readonly Random random = new Random(0);
		private readonly IMode mode;

		public MonteCarloPureSearch(IMode mode)
		{
			this.mode = mode;
		}

		public Move GetBestMove(ulong state)
		{
			var bestMove = Move.Undefined;
			var bestScore = long.MinValue;

			if (board.IsTerminal(state))
			{
				throw new InvalidOperationException();
			}

			var possibleMoves = board.GetPossibleMoves(state);

			if (possibleMoves.Count == 0)
				throw new InvalidOperationException(); // TODO: remove

			if (possibleMoves.Count == 1)
				return possibleMoves[0];

			var stopCondition = GetStopCondition(possibleMoves.Count);

			foreach (var move in possibleMoves)
			{
				var score = EvaluateMove(state, move, stopCondition);

				if (score > bestScore)
				{
					bestMove = move;
					bestScore = score;
				}
			}

			if (bestMove == Move.Undefined)
			{
				throw new InvalidOperationException();
			}

			return bestMove;
		}

		private Func<int, Stopwatch, bool> GetStopCondition(int possibleMovesCount)
		{
			switch (mode)
			{
				case FixedCountMode fcm:
					return (count, _) => count > fcm.WalksPerMove / (float) possibleMovesCount;

				case TimeMode tm:
					return (_, timer) => timer.ElapsedMilliseconds > tm.TimePerMove / (float) possibleMovesCount;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private long EvaluateMove(ulong state, Move move, Func<int, Stopwatch, bool> stopCondition)
		{
			long scoreSum = 0; // Init simulation
			var stateAfterMove = board.PlayMove(state, move);
			var randomWalksCount = 0;
			var timer = new Stopwatch();
			timer.Start();

			while (!stopCondition(randomWalksCount, timer))
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
				randomWalksCount++;
			}

			return scoreSum;
		}
	}
}