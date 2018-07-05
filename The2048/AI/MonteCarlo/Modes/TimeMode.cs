namespace The2048.AI.MonteCarlo.Modes
{
	/// <summary>
	/// Specified time is divided among possible moves. 
	/// </summary>
	public class TimeMode : IMode
	{
		public int TimePerMove { get; }

		public TimeMode(int timePerMove)
		{
			TimePerMove = timePerMove;
		}
	}
}