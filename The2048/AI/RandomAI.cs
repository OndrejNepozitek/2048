namespace The2048.AI
{
	using System;
	using Game;
	using Utils;

	/// <summary>
	/// Always picks a random move.
	/// </summary>
	public class RandomAI : ISolver, IRandomInjectable
	{
		private readonly IBoard board = new Board();
		private Random random = new Random();

		/// <inheritdoc />
		public Move GetNextMove(ulong state)
		{
			return board.GetPossibleMoves(state).GetRandom(random);
		}

		/// <inheritdoc />
		public void InjectRandomGenerator(Random random)
		{
			this.random = random;
		}
	}
}