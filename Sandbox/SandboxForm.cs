namespace Sandbox
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using GUI;
	using The2048.AI;
	using The2048.AI.MonteCarlo;
	using The2048.AI.MonteCarlo.Modes;
	using The2048.Benchmarks;

	public partial class SandboxForm : GameWindow
	{
		public SandboxForm() : base(Mode.Readonly)
		{
			InitializeComponent();
			RunBenchmark();
		}

		private void RunBenchmark()
		{
			var timer = new Stopwatch();
			timer.Start();

			Task.Run(() =>
			{
				var benchmark = new Benchmark();
				var scenario = new BenchmarkScenario();

				for (var i = 1; i < 10; i++)
				{
					scenario.AddSetup($"MonteCarlo {i * 400} iters", new MonteCarloPureSearch(new FixedCountMode(i * 400)));
					// scenario.AddSetup($"MonteCarlo {i * 400} iters", new MonteCarloPureSearch(new TimeMode(50)));
				}

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

				benchmark.Execute(scenario, 30);
			});
		}
	}
}
