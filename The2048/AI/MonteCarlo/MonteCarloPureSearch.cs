namespace The2048.AI.MonteCarlo
{
	using System;
	using System.Diagnostics;
	using Game;
	using Modes;

	/// <inheritdoc />
	/// <summary>
	/// Monte Carlo Pure Search strategy.
	/// </summary>
	/// <remarks>
	/// Specified number of random walks is made for every possible move.
	/// Average score is computed for every such move and the best move is then taken.
	/// </remarks>
	public class MonteCarloPureSearch : ISolver
	{
		private readonly IBoard board = new Board();
		private readonly Random random = new Random(0);
		private readonly IMode mode;

		public MonteCarloPureSearch(IMode mode)
		{
			this.mode = mode;
		}

		/// <inheritdoc />
		public Move GetNextMove(ulong state)
		{
			if (board.IsTerminal(state))
			{
				throw new ArgumentException("A given state must not be terminal.", nameof(state));
			}

			var bestMove = Move.Undefined;
			var bestScore = double.MinValue;
			var possibleMoves = board.GetPossibleMoves(state);

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

			return bestMove;
		}

		/// <summary>
		/// Gets the stop condition for the simulation.
		/// </summary>
		/// <param name="possibleMovesCount"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Evaluates a given move.
		/// </summary>
		/// <remarks>
		/// Random walks are played until a given stop condition holds.
		/// </remarks>
		/// <param name="initialState">Initial board state.</param>
		/// <param name="move">Move to be evaluated.</param>
		/// <param name="stopCondition"></param>
		/// <returns>The average score of random walks.</returns>
		private double EvaluateMove(ulong initialState, Move move, Func<int, Stopwatch, bool> stopCondition)
		{
			long scoreSum = 0;
			var stateAfterMove = board.PlayMove(initialState, move);
			var randomWalksCount = 0;
			var timer = new Stopwatch();
			timer.Start();

			// Play random walks until the stop condition holds
			while (!stopCondition(randomWalksCount, timer))
			{
				var state = board.GenerateRandomTile(stateAfterMove);

				// Play random moves until the state is terminal
				while (!board.IsTerminal(state))
				{
					var possibleMoves = board.GetPossibleMoves(state);
					var randomMove = possibleMoves.GetRandom(random);

					state = board.PlayAndGenerate(state, randomMove);
					scoreSum++;
				}

				randomWalksCount++;
			}

			return scoreSum / (double) randomWalksCount;
		}
	}
}