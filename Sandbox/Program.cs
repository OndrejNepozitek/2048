namespace Sandbox
{
	using System;
	using System.Windows.Forms;
	using GUI;
	using The2048.AI;
	using The2048.Benchmarks;

	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new SandboxForm());

			//var benchmark = new Benchmark();
			//var scenario = new BenchmarkScenario();

			//for (var i = 1; i < 11; i++)
			//{
			//	scenario.AddSetup($"MonteCarlo {i * 100} iters", new MonteCarloPureSearch(i * 100));
			//}

			//benchmark.Execute(scenario, 10);
		}
	}
}
