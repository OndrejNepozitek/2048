namespace The2048.Game
{
	using System.Collections.Generic;

	/// <summary>
	/// Represents a board.
	/// </summary>
	public interface IBoard
	{
		/// <summary>
		/// Applies a given move to a given state.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="move">Move to be played.</param>
		/// <returns>State after a given move is played.</returns>
		ulong PlayMove(ulong state, Move move);

		/// <summary>
		/// Gets all possible moves for a given state.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		List<Move> GetPossibleMoves(ulong state);

		/// <summary>
		/// Generates a random tile.
		/// </summary>
		/// <param name="state"></param>
		/// <returns>State after a random empty tile is filled either with tile 2 or 4.</returns>
		ulong GenerateRandomTile(ulong state);

		/// <summary>
		/// Checks if a given state is terminal.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		bool IsTerminal(ulong state);

		/// <summary>
		/// Gets the score of a given state.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		uint GetScore(ulong state);
	}
}