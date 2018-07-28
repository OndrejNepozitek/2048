namespace The2048.Benchmarks
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using AI;
	using BenchmarkUtils;
	using Game;

	public class BenchmarkJob : IPreviewableBenchmarkJob<BenchmarkResult>
	{
		public event Action<BenchmarkResult> OnPreview;
		public event Action<ulong, Move> OnMoveGenerated;
		public event Action<string> OnSetupStarted;

		private readonly string name;
		private readonly ISolver solver;
		private readonly int count;

		public BenchmarkJob(string name, ISolver solver, int count)
		{
			this.name = name;
			this.solver = solver;
			this.count = count;
		}

		public BenchmarkResult Execute()
		{
			var tilesFrequencies = new int[16];
			var tilesPercentages = new double[16];
			var scores = new List<uint>();
			var board = new Board();
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

				for (var j = (int)maxTile; j >= 0; j--)
				{
					tilesFrequencies[j]++;
				}

				for (var j = 0; j < 16; j++)
				{
					tilesPercentages[j] = tilesFrequencies[j] / (double)(i + 1);
				}

				scores.Add(board.GetScore(state));

				var result = new BenchmarkResult()
				{
					Name = $"Run {i + 2}/{count}",
					ScoreMax = scores.Max(),
					ScoreMean = (uint)scores.GetMedian(),
					MovesPerSecond = (movesCount / (double)timer.ElapsedMilliseconds) * 1000,
					TilePercentages = tilesPercentages.Select(x => x * 100).ToArray(),
				};

				OnPreview?.Invoke(result);
			}

			return new BenchmarkResult()
			{
				Name = name,
				ScoreMax = scores.Max(),
				ScoreMean = (uint)scores.GetMedian(),
				MovesPerSecond = (movesCount / (double)timer.ElapsedMilliseconds) * 1000,
				TilePercentages = tilesPercentages.Select(x => x * 100).ToArray(),
			};
		}
	}
}