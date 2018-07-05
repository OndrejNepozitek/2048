namespace The2048.Game
{
	/// <summary>
	/// Board extension methods.
	/// </summary>
	public static class BoardExtensions
	{
		/// <summary>
		/// Plays a given move and generates a random tile.
		/// </summary>
		/// <param name="board"></param>
		/// <param name="state"></param>
		/// <param name="move"></param>
		/// <returns></returns>
		public static ulong PlayAndGenerate(this IBoard board, ulong state, Move move)
		{
			state = board.PlayMove(state, move);
			state = board.GenerateRandomTile(state);

			return state;
		}

		/// <summary>
		/// Gets an initial board state.
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public static ulong GetInitialState(this IBoard board)
		{
			return board.GenerateRandomTile(0);
		}
	}
}