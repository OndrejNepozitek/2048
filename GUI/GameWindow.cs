namespace GUI
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Globalization;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using The2048.AI;
	using The2048.AI.ExpectiMax;
	using The2048.AI.MonteCarlo;
	using The2048.AI.MonteCarlo.Modes;
	using The2048.Game;

	public partial class GameWindow : Form
	{
		protected Label[] labels = new Label[16];

		protected readonly IBoard board = new Board();
		protected ulong state = 0;
		protected Dictionary<Keys, Move> keyToMoveMapping;
		protected Mode mode;
		protected State currentState = State.Stopped;
		protected ISolver currentSolver;

		protected bool limitSimulationSpeed = false;
		protected int limitSimulationSpeedValue = 1;

		// Tile colors
		private readonly string[] tileBackColors =
		{
			"cdc1b4", "eee4da", "ede0c8", "f2b179",
			"f59563", "f67c5f", "f65e3b", "edcf72",
			"edcc61", "edc850", "edc53f", "edc22e",
			"3c3a32", "3c3a32", "3c3a32", "3c3a32",
		};

		public GameWindow(Mode mode)
		{
			this.mode = mode;
			InitializeComponent();
			CreateTiles();

			keyToMoveMapping = new Dictionary<Keys, Move>
			{
				{Keys.Up, The2048.Game.Move.Up},
				{Keys.Down, The2048.Game.Move.Down},
				{Keys.Right, The2048.Game.Move.Right},
				{Keys.Left, The2048.Game.Move.Left}
			};
		}

		private void InitManualPlay()
		{
			state = board.GenerateRandomTile(0);
			DrawState(state);
		}

		private void InitSimulation(ISolver solver = null)
		{
			if (solver == null)
			{
				solver = currentSolver;
			}

			Task.Run(() =>
			{
				var timer = new Stopwatch();

				while (!board.IsTerminal(state))
				{
					var move = solver.GetNextMove(state);
					state = board.PlayAndGenerate(state, move);

					Invoke((Action)(() =>
					{
						if (currentState != State.Running)
						{
							return;
						}

						DrawState(state);
						UpdateScore(state);
					}));

					if (currentState != State.Running)
					{
						return;
					}

					if (limitSimulationSpeed)
					{
						if (timer.ElapsedMilliseconds < limitSimulationSpeedValue)
						{
							Thread.Sleep((int)(limitSimulationSpeedValue - timer.ElapsedMilliseconds));
						}
						else
						{
							timer.Restart();
						}
					}
				}

				Invoke((Action)(() =>
				{
					scoreLabel.Text += " - Game Over";
					buttonStart.Show();
					buttonStop.Hide();
					buttonPause.Hide();
					currentState = State.Stopped;
				}));
			});
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
			if (currentState != State.Manual)
			{
				return;
			}

			if (!keyToMoveMapping.TryGetValue(e.KeyCode, out var move))
			{
				return;
			}

			state = board.PlayAndGenerate(state, move);
			DrawState(state);
			UpdateScore(state);
		}

		protected void UpdateScore(ulong state)
		{
			var score = board.GetScore(state);
			scoreLabel.Text = $"Score: {score}";
			scoreLabel.Refresh();
		}

		protected void UpdateName(string name)
		{
			nameLabel.Text = name;
		}

		public enum Mode
		{
			Manual, Simulation, Readonly
		}

		public enum State
		{
			Running, Paused, Stopped, Manual
		}

		private void buttonStart_Click(object sender, EventArgs e)
		{
			buttonStart.Hide();
			buttonStop.Show();
			buttonPause.Show();

			switch (currentState)
			{
				case State.Paused:

					currentState = State.Running;
					InitSimulation(currentSolver);
					break;

				case State.Stopped:

					currentState = State.Running;
					state = board.GetInitialState();

					if (radioExpectimax.Checked)
					{
						currentSolver = new ExpectiMax((int) numericExpectimaxDepth.Value);
					} else if (radioMonteCarlo.Checked)
					{
						IMode mode = null;

						if (radioMonteCarloCount.Checked)
						{
							mode = new FixedCountMode((int) numericMonteCarloCount.Value);
						} else if (radioMonteCarloTime.Checked)
						{
							mode = new TimeMode((int) numericMonteCarloTime.Value);
						}

						currentSolver = new MonteCarloPureSearch(mode);
					} else if (radioRandom.Checked)
					{
						currentSolver = new RandomAI();
					}

					InitSimulation();

					break;
			}
		}

		private void HideSettings()
		{
			groupMonteCarlo.Hide();
			groupExpectimax.Hide();
		}

		private void buttonPause_Click(object sender, EventArgs e)
		{
			currentState = State.Paused;
			buttonPause.Hide();
			buttonStart.Show();
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			currentState = State.Stopped;

			buttonStart.Show();
			buttonStop.Hide();
			buttonPause.Hide();

			state = 0;
			DrawState(0);
			UpdateScore(0);
		}

		private void radioExpectimax_CheckedChanged(object sender, EventArgs e)
		{
			if (radioExpectimax.Checked)
			{
				HideSettings();
				groupExpectimax.Show();
			}
		}

		private void radioMonteCarlo_CheckedChanged(object sender, EventArgs e)
		{
			if (radioMonteCarlo.Checked)
			{
				HideSettings();
				groupMonteCarlo.Show();
			}
		}

		private void radioRandom_CheckedChanged(object sender, EventArgs e)
		{
			HideSettings();
		}

		private void checkBoxLimitSimulationSpeed_CheckedChanged(object sender, EventArgs e)
		{
			limitSimulationSpeed = checkBoxLimitSimulationSpeed.Checked;
			limitSimulationSpeedValue = (int) numericLimitSimulationSpeed.Value;
		}

		private void numericLimitSimulationSpeed_ValueChanged(object sender, EventArgs e)
		{
			limitSimulationSpeedValue = (int)numericLimitSimulationSpeed.Value;
		}
	}
}
