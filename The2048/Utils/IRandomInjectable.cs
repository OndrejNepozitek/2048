namespace The2048.Utils
{
	using System;

	/// <summary>
	/// Provides a method to inject an instance of a random numbers generator.
	/// </summary>
	public interface IRandomInjectable
	{
		/// <summary>
		/// Injects an instance of a random numbers generator.
		/// </summary>
		/// <param name="random"></param>
		void InjectRandomGenerator(Random random);
	}
}