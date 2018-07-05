namespace The2048.AI.MonteCarlo.Modes
{
	/// <summary>
	/// Specified number of random walks is divided among possible moves.
	/// </summary>
	public class FixedCountMode : IMode
	{
		public int WalksPerMove { get; }

		public FixedCountMode(int walksPerMove)
		{
			WalksPerMove = walksPerMove;
		}
	}
}