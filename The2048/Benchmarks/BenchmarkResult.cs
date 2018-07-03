namespace The2048.Benchmarks
{
	public class BenchmarkResult
	{
		public string Name { get; set; }

		public double MovesPerSecond { get; set; }

		public double[] TilePercentages { get; set; }
	}
}