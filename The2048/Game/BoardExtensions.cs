namespace The2048.Game
{
	using System.Collections.Generic;

	public static class BoardExtensions
	{
		public static ulong PlayAndGenerate(this IBoard board, ulong state, Move move)
		{
			state = board.PlayMove(state, move);
			state = board.GenerateRandomTile(state);

			return state;
		}
	}
}