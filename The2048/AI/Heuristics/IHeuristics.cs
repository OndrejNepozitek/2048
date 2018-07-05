namespace The2048.AI.Heuristics
{
	/// <summary>
	/// Provides a method to heuristically evaluate a state.
	/// </summary>
	public interface IHeuristics
	{
		/// <summary>
		/// Evaluates a given state.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		double EvaluateState(ulong state);
	}
}