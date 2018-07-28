# 2048
Optimized board operations, AI and GUI for the game 2048. Initially implemented as a school project.

## Features
- 3 implemented AIs - ExpectiMax, MonteCarlo Pure Search and RandomAI
- Fast board operations using 64-bit representation of the board and bitwise operations
- Simple GUI to setup the AIs and see how they play
- .NET Standard 2.0 dll
- Simple benchmarking infrastructure

## How to setup
- run the GUI with **GUI.exe** (.NET Framework 4.6.1) **or**
- include the **The2048.dll** (.NET Standard 2.0 dll)

## How to use
The `IBoard` interface and its `Board` implementation provide everything to create your own AI.

```C#
/// <summary>
/// Represents a board.
/// </summary>
public interface IBoard
{
  /// <summary>
  /// Applies a given move to a given state.
  /// </summary>
  /// <param name="state"></param>
  /// <param name="move">Move to be played.</param>
  /// <returns>State after a given move is played.</returns>
  ulong PlayMove(ulong state, Move move);

  /// <summary>
  /// Gets all possible moves for a given state.
  /// </summary>
  /// <param name="state"></param>
  /// <returns></returns>
  List<Move> GetPossibleMoves(ulong state);

  /// <summary>
  /// Generates a random tile.
  /// </summary>
  /// <param name="state"></param>
  /// <returns>State after a random empty tile is filled either with tile 2 or 4.</returns>
  ulong GenerateRandomTile(ulong state);

  /// <summary>
  /// Checks if a given state is terminal.
  /// </summary>
  /// <param name="state"></param>
  /// <returns></returns>
  bool IsTerminal(ulong state);

  /// <summary>
  /// Gets the score of a given state.
  /// </summary>
  /// <param name="state"></param>
  /// <returns></returns>
  uint GetScore(ulong state);
}
```

## GUI screenshot
![alt text](http://github.ondra.nepozitek.cz/2048/image.png)
