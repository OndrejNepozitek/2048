namespace The2048.Game
{
	using System.Collections.Generic;

	public interface IBoard
	{
		ulong PlayMove(ulong state, Move move);

		List<Move> GetPossibleMoves(ulong state);

		ulong GenerateRandomTile(ulong state);

		bool IsTerminal(ulong state);

		uint GetScore(ulong state);
	}
}