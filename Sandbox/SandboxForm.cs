namespace Sandbox
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using GUI;
	using The2048.AI;
	using The2048.AI.ExpectiMax;
	using The2048.AI.MonteCarlo;
	using The2048.AI.MonteCarlo.Modes;
	using The2048.Benchmarks;

	public partial class SandboxForm : GameWindow
	{
		public SandboxForm() : base(Mode.Readonly)
		{
			InitializeComponent();
			HideControlsForBenchmark();
			RunBenchmark();
		}

		private void RunBenchmark()
		{
			var timer = new Stopwatch();
			timer.Start();

			Task.Run(() =>
			{
				var jobs = new List<BenchmarkJob>();

				for (var i = 1; i < 10; i++)
				{
					jobs.Add(new BenchmarkJob($"MonteCarlo {i * 400} iters", new MonteCarloPureSearch(new FixedCountMode(i * 400)), 3));
					// scenario.AddSetup($"MonteCarlo {i * 400} iters", new MonteCarloPureSearch(new TimeMode(50)));
				}

				for (var i = 2; i < 4; i++)
				{
					jobs.Add(new BenchmarkJob($"ExpectiMax d{2 * i}, nprune", new ExpectiMax(2 * i, false, false), 5));
				}

				for (var i = 2; i < 4; i++)
				{
					jobs.Add(new BenchmarkJob($"ExpectiMax d{2 * i}, prune", new ExpectiMax(2 * i, true, false),5));
				}

				var benchmark = new Benchmark();
				benchmark.AddFileOutput();

				benchmark.OnSetupStarted += (name) =>
				{
					BeginInvoke((Action)(() =>
					{
						UpdateName(name);
					}));
				};

				benchmark.OnMoveGenerated += (newState, move) =>
				{
					if (timer.ElapsedMilliseconds >= 20)
					{
						BeginInvoke((Action)(() =>
						{

							DrawState(newState);
							UpdateScore(newState);
						}));

						timer.Restart();
					}
				};

				//var job = new BenchmarkJob($"ExpectiMax d{2 * 3}, prune", new ExpectiMax(2 * 3, true, false), 5);

				benchmark.Run(jobs.ToArray(), "2048 benchmark");
			});
		}
	}
}
