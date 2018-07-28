namespace The2048.Benchmarks
{
	using System;
	using BenchmarkUtils;
	using Game;

	public class Benchmark : Benchmark<BenchmarkJob, BenchmarkResult>
	{
		public event Action<ulong, Move> OnMoveGenerated;
		public event Action<string> OnSetupStarted;

		protected override void Run(BenchmarkJob job)
		{
			void MoveGeneratedFunc(ulong state, Move move) => OnMoveGenerated?.Invoke(state, move);
			void SetupStartedFunc(string name) => OnSetupStarted?.Invoke(name);

			job.OnMoveGenerated += MoveGeneratedFunc;
			job.OnSetupStarted += SetupStartedFunc;

			base.Run(job);

			job.OnMoveGenerated -= MoveGeneratedFunc;
			job.OnSetupStarted -= SetupStartedFunc;
		}
	}
}