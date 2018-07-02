namespace The2048.Game
{
	using System;
	using System.Collections.Generic;

	public class Board : IBoard
	{
		private ushort[] movesTableRight;
		private ushort[] movesTableLeft;
		private uint[] scoresTable;
		private Random random = new Random();

		private readonly Move[] possibleMoves =
		{
			Move.Up, Move.Down, Move.Left, Move.Right, Move.Undefined
		};

		public Board()
		{
			GenerateTables();
		}

		private void GenerateTables()
		{
			movesTableRight = new ushort[65536];
			movesTableLeft = new ushort[65536];
			scoresTable = new uint[65536];

			// Generate score and move tables
			for (var i = 0; i <= 65535; i++)
			{
				var pos = new byte[4];
				var score = 0;

				for (byte k = 0; k < 4; k++)
				{
					pos[k] = (byte)((i >> k * 4) & 15);

					if (pos[k] >= 2)
					{
						score += ((pos[k] - 1) * (1 << pos[k]));
					}
				}

				pos = ShiftRight(pos);
				pos = MatchRight(pos);
				pos = ShiftRight(pos);

				movesTableRight[i] = (ushort) (pos[0] + (pos[1] << 4) + (pos[2] << 8) + (pos[3] << 12));
				scoresTable[i] = (uint) score;
			}

			// Generate move table
			for (var i = 0; i <= 65535; i++)
			{
				var pos = new byte[4];

				for (byte k = 0; k < 4; k++)
				{
					pos[3 - k] = (byte)((i >> k * 4) & 15);
				}

				pos = ShiftRight(pos);
				pos = MatchRight(pos);
				pos = ShiftRight(pos);

				movesTableLeft[i] = (ushort) (pos[3] + (pos[2] << 4) + (pos[1] << 8) + (pos[0] << 12));
			}
		}

		/// <summary>
		/// Efficient transposition using bitwise operations.
		/// </summary>
		/// <param name="state">Current state</param>
		/// <returns>Transposed state</returns>
		private ulong Transpose(ulong state)
		{
			var a1 = state & 0xF0F00F0FF0F00F0FL;
			var a2 = state & 0x0000F0F00000F0F0L;
			var a3 = state & 0x0F0F00000F0F0000L;
			var a = a1 | (a2 << 12) | (a3 >> 12);
			var b1 = a & 0xFF00FF0000FF00FFL;
			var b2 = a & 0x00FF00FF00000000L;
			var b3 = a & 0x00000000FF00FF00L;

			return b1 | (b2 >> 24) | (b3 << 24);
		}

		/// <summary>
		/// Shifts all tiles in the row to the right side.
		/// </summary>
		/// <param name="row">Row consisting of 4 tiles</param>
		/// <returns>Shifted row</returns>
		private static byte[] ShiftRight(byte[] row)
		{
			for (var i = 0; i < 4; i++)
			{
				for (var j = 0; j < 3; j++)
				{
					if (row[j + 1] == 0)
					{
						row[j + 1] = row[j];
						row[j] = 0;
					}
				}
			}

			return row;
		}

		/// <summary>
		/// If there are two non-zero tiles with the same value next to each other, we merge them into one tile.
		/// </summary>
		/// <param name="row">Row consisting of 4 tiles</param>
		/// <returns>Row after merging adjacent tiles</returns>
		private static byte[] MatchRight(byte[] row)
		{
			// This process must be done from right to left to ensure proper merging.
			for (var i = 2; i > -1; i--)
			{
				if (row[i] == row[i + 1] && row[i] != 0)
				{
					row[i + 1] = (byte)(row[i + 1] + 1);
					row[i] = 0;
				}
			}

			return row;
		}

		public ulong PlayMove(ulong state, Move move)
		{
			var movesTable = movesTableRight;

			if (move == Move.Left || move == Move.Up)
			{
				movesTable = movesTableLeft;
			}

			if (move == Move.Up || move == Move.Down)
			{
				state = Transpose(state);
			}

			state = movesTable[(int)(state & 0xFFFF)]
			        | ((ulong)movesTable[(int)((state >> 16) & 0xFFFF)] << 16)
			        | ((ulong)movesTable[(int)((state >> 32) & 0xFFFF)] << 32)
			        | ((ulong)movesTable[(int)((state >> 48) & 0xFFFF)] << 48);

			if (move == Move.Up || move == Move.Down)
			{
				state = Transpose(state);
			}

			return state;
		}

		public List<Move> GetPossibleMoves(ulong state)
		{
			var moves = new List<Move>();

			foreach (var m in possibleMoves)
			{

				if (m == Move.Undefined)
				{
					continue;
				}

				if (state != PlayMove(state, m))
				{
					moves.Add(m);
				}
			}

			return moves;
		}

		public ulong GenerateRandomTile(ulong state)
		{
			// Count empty tiles
			byte empty = 0;
			for (int i = 0; i < 16; i++)
			{
				if (((state >> (i * 4)) & 0xF) == 0)
				{
					empty++;
				}
			}

			// There are no empty tiles
			if (empty == 0)
			{
				return state;
			}

			// Get a number from 0 to 100
			int field = random.Next(empty);
			int numberChance = random.Next(100);

			// Generate a 4 tile if numberChance was over 90
			int number = numberChance >= 90 ? 4 : 2;

			// Create the tile
			int c = -1;
			for (int i = 0; i < 16; i++)
			{
				if (((state >> (i * 4)) & 0xF) == 0)
				{
					c++;
					if (c == field)
					{
						if (number == 2)
						{
							state = state + ((ulong)0x1 << (i * 4));
						}
						else
						{
							state = state + ((ulong)0x2 << (i * 4));
						}

						break;
					}
				}
			}

			return state;
		}

		public bool IsTerminal(ulong state)
		{
			foreach (var m in possibleMoves)
			{

				if (m == Move.Undefined)
				{
					continue;
				}

				if (state != PlayMove(state, m))
				{
					return false;
				}
			}

			return true;
		}

		public uint GetScore(ulong state)
		{
			return scoresTable[(state & 0xFFFF)]
			       + scoresTable[(state >> 16) & 0xFFFF]
			       + scoresTable[(state >> 32) & 0xFFFF]
			       + scoresTable[(state >> 48) & 0xFFFF];
		}
	}
}