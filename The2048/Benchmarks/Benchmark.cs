namespace The2048.Benchmarks
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using AI;
	using Game;

	public class Benchmark
	{
		private readonly BenchmarkFormatter benchmarkFormatter;
		private readonly IBoard board = new Board();

		public event Action<string> OnSetupStarted;
		public event Action OnRunStarted;
		public event Action<ulong, Move> OnMoveGenerated;
		public event Action OnRunEnded;
		public event Action OnSetupEnded;

		public Benchmark()
		{
			benchmarkFormatter = new BenchmarkFormatter();
			benchmarkFormatter.SetColumnWidth(15);
			benchmarkFormatter
				.AddColumn("Name", 25)
				.AddColumn("Score")
				.AddColumn("Moves / s");

			for (var i = 16 - 1; i >= 9; i--)
			{
				var text = ((int) Math.Pow(2, i)).ToString();
				benchmarkFormatter.AddColumn(text, text.Length + 3);
			}
		}

		public void Execute(BenchmarkScenario scenario, int count)
		{
			benchmarkFormatter.PrintHeader(Console.Out);

			foreach (var setup in scenario.GetSetups())
			{
				var result = Execute(setup.Item1, setup.Item2, count);
				var values = ResultToText(result);

				benchmarkFormatter.PrintRow(Console.Out, values);
			}
		}

		private string[] ResultToText(BenchmarkResult result)
		{
			var values = new List<string>();

			values.Add(result.Name);
			values.Add("");
			values.Add($"{result.MovesPerSecond:F}");

			for (var i = 16 - 1; i >= 9; i--)
			{
				values.Add($"{(int)Math.Floor(result.TilePercentages[i] * 100)}%");
			}

			return values.ToArray();
		}

		public BenchmarkResult Execute(string name, ISolver solver, int count)
		{
			var tilesFrequencies = new int[16];
			var tilesPercentages = new double[16];
			OnSetupStarted?.Invoke(name);

			var timer = new Stopwatch();
			timer.Start();
			var movesCount = 0L;

			for (var i = 0; i < count; i++)
			{
				var state = board.GetInitialState();

				while (!board.IsTerminal(state))
				{
					var move = solver.GetNextMove(state);
					movesCount++;
					state = board.PlayAndGenerate(state, move);

					OnMoveGenerated?.Invoke(state, move);
				}

				var maxTile = BoardStateHelpers.GetMaxTile(state);

				for (var j = (int) maxTile; j >= 0; j--)
				{
					tilesFrequencies[j]++;
				}

				for (var j = 0; j < 16; j++)
				{
					tilesPercentages[j] = tilesFrequencies[j] / (double) (i+1);
				}

				var result = new BenchmarkResult()
				{
					Name = $"Run {i+2}/{count}",
					MovesPerSecond = (movesCount / (double) timer.ElapsedMilliseconds) * 1000,
					TilePercentages = tilesPercentages,
				};

				benchmarkFormatter.PreviewRow(ResultToText(result));
			}

			return new BenchmarkResult()
			{
				Name = name,
				MovesPerSecond = (movesCount / (double)timer.ElapsedMilliseconds) * 1000,
				TilePercentages = tilesPercentages,
			};
		}
	}
}