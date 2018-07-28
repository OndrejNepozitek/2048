namespace The2048.Benchmarks
{
	using BenchmarkUtils.Attributes;
	using BenchmarkUtils.Enums;

	public class BenchmarkResult
	{
		[Order(1)]
		[Length(25)]
		[Name("Name")]
		public string Name { get; set; }

		[Order(2)]
		[Length(13)]
		[Name("Score max")]
		public uint ScoreMax { get; set; }

		[Order(3)]
		[Length(13)]
		[Name("Score mean")]
		public uint ScoreMean { get; set; }

		[Order(4)]
		[Length(13)]
		[Name("Moves / s")]
		[ValueFormat("{0:F}")]
		public double MovesPerSecond { get; set; }

		[Order(5)]
		[Length(8)]
		[Name("32768")]
		[ValueFormat("{0:0}%")]
		public double Tile32768 => TilePercentages[15];

		[Order(6)]
		[Length(8)]
		[Name("16384")]
		[ValueFormat("{0:0}%")]
		public double Tile16384 => TilePercentages[14];

		[Order(7)]
		[Length(7)]
		[Name("8192")]
		[ValueFormat("{0:0}%")]
		public double Tile8192 => TilePercentages[13];

		[Order(8)]
		[Length(7)]
		[Name("4096")]
		[ValueFormat("{0:0}%")]
		public double Tile4096 => TilePercentages[12];

		[Order(9)]
		[Length(7)]
		[Name("2048")]
		[ValueFormat("{0:0}%")]
		public double Tile2048 => TilePercentages[11];

		[Order(10)]
		[Length(7)]
		[Name("1024")]
		[ValueFormat("{0:0}%")]
		public double Tile1024 => TilePercentages[10];

		[Order(11)]
		[Length(6)]
		[Name("512")]
		[ValueFormat("{0:0}%")]
		public double Tile512 => TilePercentages[9];

		[Show(ShowIn.None)]
		public double[] TilePercentages { get; set; }
	}
}