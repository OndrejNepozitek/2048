namespace The2048.Game
{
	public static class BoardExtensions
	{
		public static ulong PlayAndGenerate(this IBoard board, ulong state, Move move)
		{
			state = board.PlayMove(state, move);
			state = board.GenerateRandomTile(state);

			return state;
		}

		public static ulong GetInitialState(this IBoard board)
		{
			return board.GenerateRandomTile(0);
		}
	}
}