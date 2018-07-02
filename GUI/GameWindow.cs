namespace GUI
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Globalization;
	using System.Windows.Forms;
	using The2048.Game;

	public partial class GameWindow : Form
	{
		private Label[] labels = new Label[16];
		private readonly bool manual;

		private readonly IBoard board = new Board();
		private ulong state = 0;
		private Dictionary<Keys, Move> keyToMoveMapping;

		// Tile colors
		private readonly string[] tileBackColors =
		{
			"cdc1b4", "eee4da", "ede0c8", "f2b179",
			"f59563", "f67c5f", "f65e3b", "edcf72",
			"edcc61", "edc850", "edc53f", "edc22e",
			"3c3a32", "3c3a32", "3c3a32", "3c3a32",
		};


		public GameWindow(bool manual = true)
		{
			this.manual = manual;
			InitializeComponent();
			CreateTiles();

			keyToMoveMapping = new Dictionary<Keys, Move>
			{
				{Keys.Up, The2048.Game.Move.Up},
				{Keys.Down, The2048.Game.Move.Down},
				{Keys.Right, The2048.Game.Move.Right},
				{Keys.Left, The2048.Game.Move.Left}
			};

			if (manual)
			{
				InitManualPlay();
			}
		}

		private void InitManualPlay()
		{
			state = board.GenerateRandomTile(0);
			DrawState(state);
		}

		private void CreateTiles()
		{
			var totalWidth = boardContainer.Width;
			var totalHeight = boardContainer.Height;
			const int margin = 10;
			var tileWidth = (totalWidth - 5 * margin) / 4;
			var tileHeight = (totalHeight - 5 * margin) / 4;

			for (var i = 0; i < 16; i++)
			{
				var l = new Label();

				var top = i / 4;
				var left = i % 4;

				l.Location = new Point(
					left * (tileWidth + margin) + margin,
					top * (tileHeight + margin) + margin);
				l.Text = "";
				l.Size = new Size(tileWidth, tileHeight);
				l.TextAlign = ContentAlignment.MiddleCenter;
				l.BorderStyle = BorderStyle.None;
				l.BackColor = ColorTranslator.FromHtml("#cdc1b4");
				l.ForeColor = ColorTranslator.FromHtml("#776e65");
				l.Font = new Font("Arial", 28, FontStyle.Bold);
				labels[i] = l;
				boardContainer.Controls.Add(l);
			}

			boardContainer.BackColor = ColorTranslator.FromHtml("#bbada0");
		}

		public void DrawState(ulong state)
		{
			var counter = 0;

			foreach (var tile in GetTiles(state))
			{
				// Update text
				labels[counter].Text = tile != 0 ? Math.Pow(2, tile).ToString(CultureInfo.InvariantCulture) : string.Empty;

				// Handle background colors
				labels[counter].BackColor = ColorTranslator.FromHtml("#" + tileBackColors[tile]);

				// Handle font colors
				labels[counter].ForeColor = ColorTranslator.FromHtml(tile >= 3 ? "#f9f6f2" : "#776e65");

				// Handle font sizes
				labels[counter].Font = new Font(labels[counter].Font.FontFamily, GetFontSize(tile), labels[counter].Font.Style);

				counter++;
			}
		}

		private int GetFontSize(byte tile)
		{
			if (tile >= 14)
			{
				return 18;
			}

			if (tile >= 10)
			{
				return 21;
			}

			if (tile >= 7)
			{
				return 24;
			}

			return 28;
		}

		private IEnumerable<byte> GetTiles(ulong state)
		{
			for (var i = 0; i < 16; i++)
			{
				yield return (byte) (state & 0xF);
				state >>= 4;
			}
		}

		private void GameWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if (!keyToMoveMapping.TryGetValue(e.KeyCode, out var move))
			{
				return;
			}

			state = board.PlayAndGenerate(state, move);
			DrawState(state);
			UpdateScore();
		}

		private void UpdateScore()
		{
			var score = board.GetScore(state);
			scoreLabel.Text = $"Score: {score}";
			scoreLabel.Refresh();
		}
	}
}
