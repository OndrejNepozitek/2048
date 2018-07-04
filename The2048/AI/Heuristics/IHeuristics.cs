namespace The2048.AI.Heuristics
{
	public interface IHeuristics
	{
		double EvaluateState(ulong state);
	}
}