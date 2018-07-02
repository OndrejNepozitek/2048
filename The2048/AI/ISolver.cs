namespace The2048.AI
{
	using Game;

	public interface ISolver
	{
		Move GetBestMove(ulong state);
	}
}