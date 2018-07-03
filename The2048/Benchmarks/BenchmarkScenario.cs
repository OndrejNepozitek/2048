namespace The2048.Benchmarks
{
	using System;
	using System.Collections.Generic;
	using AI;

	public class BenchmarkScenario
	{
		private List<Tuple<string, ISolver>> setups = new List<Tuple<string, ISolver>>();

		public void AddSetup(string name, ISolver solver)
		{
			setups.Add(Tuple.Create(name, solver));
		}

		public List<Tuple<string, ISolver>> GetSetups()
		{
			return setups;
		}
	}
}