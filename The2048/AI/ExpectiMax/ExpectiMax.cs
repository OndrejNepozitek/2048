namespace The2048.AI.ExpectiMax
{
	using System;
	using System.Collections.Generic;
	using AI.Heuristics;
	using Game;

	/// <summary>
	/// ExpectiMax strategy.
	/// https://en.wikipedia.org/wiki/Expectiminimax_tree
	/// </summary>
	public class ExpectiMax : ISolver
	{
		private readonly int maxDepth = 8;
		private Dictionary<ulong, double> previouslySeenStates;
		private readonly IBoard board = new Board();
		private readonly IHeuristics heuristics = new Heuristics();

		private readonly Move[] possibleMoves =
		{
			Move.Up, Move.Down, Move.Left, Move.Right
		};

		/// <inheritdoc />
		public Move GetNextMove(ulong state)
	    {
			previouslySeenStates = new Dictionary<ulong, double>();

		    var bestMove = Move.Undefined;
		    var bestScore = double.MinValue;

		    foreach (var move in possibleMoves)
		    {
			    var nextState = board.PlayMove(state, move);

				if (nextState == state)
					continue;

			    var score = EvaluateNode(nextState, 1, 1d);

			    if (score > bestScore)
			    {
				    bestScore = score;
				    bestMove = move;
			    }
		    }

			if (bestMove == Move.Undefined)
				throw new InvalidOperationException();

		    return bestMove;
	    }

		/// <summary>
		/// Evaluates a given node.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="depth"></param>
		/// <param name="probability"></param>
		/// <returns></returns>
	    private double EvaluateNode(ulong state, int depth, double probability)
	    {
			if (depth > maxDepth)
		    {
			    return EvaluateState(state);
		    }

		    if (board.IsTerminal(state))
		    {
			    return EvaluateState(state);
		    }

			if (probability < 0.005)
			{
				return EvaluateState(state);
			}

			if (previouslySeenStates.TryGetValue(state, out var previouslySeenStateScore))
		    {
			    return previouslySeenStateScore;
		    }

		    var score = depth % 2 == 0 ?
			    EvaluateMoveNode(state, depth, probability) : 
			    EvaluateRandomNode(state, depth, probability);

			previouslySeenStates.Add(state, score);

		    return score;
	    }

		/// <summary>
		/// Returns the average score of all possible nodes created by filling a random empty tile.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="depth"></param>
		/// <param name="probability"></param>
		/// <returns></returns>
	    private double EvaluateRandomNode(ulong state, int depth, double probability)
	    {
		    var score = 0d;
		    var emptyTiles = 0;
		    var stateTemp = state;

		    for (var i = 0; i < 16; i++)
		    {
			    if ((stateTemp & 0xF) == 0)
			    {
				    emptyTiles++;

				    score += EvaluateNode(state | ((ulong) 0x1 << (i * 4)), depth + 1, probability * 0.9d) * 0.9d;
				    score += EvaluateNode(state | ((ulong) 0x2 << (i * 4)), depth + 1, probability * 0.1d) * 0.1d; 
			    }

			    stateTemp >>= 4;
			}

			if (emptyTiles == 0)
				throw new InvalidOperationException();

		    var avgScore = score / emptyTiles;

		    return avgScore;
	    }

		/// <summary>
		/// Returns the score of the best move available from a given state.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="depth"></param>
		/// <param name="probability"></param>
		/// <returns></returns>
	    private double EvaluateMoveNode(ulong state, int depth, double probability)
	    {
		    var bestScore = double.MinValue;

		    foreach (var move in possibleMoves)
		    {
			    var nextState = board.PlayMove(state, move);

				// Skip invalid moves
				if (nextState == state)
					continue;

			    var score = EvaluateNode(nextState, depth + 1, probability);

			    if (score > bestScore)
			    {
				    bestScore = score;
			    }
		    }

		    return bestScore;
	    }

		/// <summary>
		/// Evaluates a given state.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
	    private double EvaluateState(ulong state)
	    {
		    return board.IsTerminal(state) ? 0 : heuristics.EvaluateState(state);
	    }
    }
}
