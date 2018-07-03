namespace The2048.AI.MonteCarlo.Modes
{
	public class FixedCountMode : IMode
	{
		public int WalksPerMove { get; }

		public FixedCountMode(int walksPerMove)
		{
			WalksPerMove = walksPerMove;
		}
	}
}