namespace The2048.AI
{
	using Game;

	/// <summary>
	/// Represents an AI for the game
	/// </summary>
	public interface ISolver
	{
		/// <summary>
		/// Suggests a next move for a given state.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		Move GetNextMove(ulong state);
	}
}