namespace The2048.AI.MonteCarlo.Modes
{
	public class TimeMode : IMode
	{
		public int TimePerMove { get; }

		public TimeMode(int timePerMove)
		{
			TimePerMove = timePerMove;
		}
	}
}