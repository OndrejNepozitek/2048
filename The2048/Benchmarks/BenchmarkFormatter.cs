namespace The2048.Benchmarks
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class BenchmarkFormatter
	{
		private int columnWidth;
		private readonly List<Column> columns = new List<Column>();

		public void SetColumnWidth(int width)
		{
			columnWidth = width;
		}

		public BenchmarkFormatter AddColumn(string name, int width = -1)
		{
			columns.Add(new Column(name, width <= 0 ? columnWidth : width));

			return this;
		}

		public void PrintHeader(TextWriter writer)
		{
			var totalWidth = columns.Sum(x => x.Width) + 1;

			writer.WriteLine(new string('-', totalWidth));

			var first = true;
			foreach (var column in columns)
			{
				if (first)
				{
					first = false;

					writer.Write($" {column.Name.PadRight(column.Width - 1, ' ')}");
				}
				else
				{
					writer.Write($"| {column.Name.PadRight(column.Width - 2, ' ')}");
				}
			}

			writer.WriteLine("|");
			writer.WriteLine(new string('-', totalWidth));
		}

		public void PrintRow(TextWriter writer, params string[] values)
		{
			if (values.Length > columns.Count)
				throw new InvalidOperationException("The number of values must not be greater that the number of columns.");

			for (var i = 0; i < values.Length; i++)
			{
				var column = columns[i];
				var value = values[i];

				writer.Write("  ");
				writer.Write(value.PadRight(column.Width - 2, ' '));
			}
			
			writer.WriteLine();
		}

		public void PreviewRow(params string[] values)
		{
			var writer = Console.Out;

			if (values.Length > columns.Count)
				throw new InvalidOperationException("The number of values must not be greater that the number of columns.");

			for (var i = 0; i < values.Length; i++)
			{
				var column = columns[i];
				var value = values[i];

				writer.Write("  ");
				writer.Write(value.PadRight(column.Width - 2, ' '));
			}

			Console.SetCursorPosition(0, Console.CursorTop);
		}

		private class Column
		{
			public string Name { get; }

			public int Width { get; }

			public Column(string name, int width)
			{
				Name = name;
				Width = width;
			}
		}
	}
}