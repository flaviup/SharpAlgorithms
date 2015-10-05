
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

using System;
using System.Collections.Generic;
using System.Drawing;

namespace SharpAlgorithms.GeneralizedBacktracking
{
	public class Backtracking2D
	{
		public Backtracking2D ()
		{
		}

		private static char[,] SearchSpace = new char[5, 7] {
			{'*', '*', '*', '*', '*', '*', '*'},
			{'*', 'S', ' ', '*', '*', '*', '*'},
			{'*', '*', ' ', ' ', '*', '*', '*'},
			{'*', '*', ' ', ' ', ' ', ' ', '*'},
			{'*', '*', '*', '*', ' ', 'E', '*'}
		};

		public void Test ()
		{
			var configurator = new BacktrackingConfigurator<Point, char[,], bool[,]> (SearchSpace, 
																                      PartialCheck, 
																                      TotalCheck,
																                      Generate,
																                      CreateArgument,
																                      ResetArgument, 
																					  new Point[] { new Point (1, 1), new Point (1, 2)/*, new Point (2, 2), new Point (2, 3) */},
																					  new Point[] { new Point (1, 1), new Point (1, 2), new Point (2, 2), new Point (2, 3), new Point (3, 3), new Point (3, 4), new Point (4, 4), new Point (4, 5) });
			var backtracking = new Backtracking<Point, char[,], bool[,]> (configurator);
			int total = 0;

			foreach (var solution in backtracking)
			{
				++total;
				foreach (var part in solution)
				{
					Console.Write ($"({part.X}, {part.Y}) ");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ($"Serial total: {total}");
		}

		public static bool PartialCheck (IReadOnlyList<Point> partialSolution, char[,] searchSpace, bool[,] argument)
		{
			var bits = argument;

			if (bits == null)
				return false;

			if (partialSolution.Count < 1)
				return false;
			
			var position = partialSolution.Count;
			var value = partialSolution [position - 1];

			if (value.X < 0 || value.X >= bits.GetLength (0))
				return false;

			if (value.Y < 0 || value.Y >= bits.GetLength (1))
				return false;

			if (bits [value.X, value.Y])
				return false;
			
			bits [value.X, value.Y] = true;

			return true;
		}

		public static bool TotalCheck (IReadOnlyList<Point> solution, char[,] searchSpace, bool[,] argument)
		{
			var bits = argument;

			if (bits == null)
				return false;

			if (solution.Count < 1)
				return false;
			
			var position = solution.Count;
			var value = solution [position - 1];

			if (value.X < 0 || value.X >= bits.GetLength (0))
				return false;

			if (value.Y < 0 || value.Y >= bits.GetLength (1))
				return false;

			return (searchSpace [value.X, value.Y] == 'E');
		}

		public static IEnumerator<Point> Generate (IReadOnlyList<Point> partialSolution, char[,] searchSpace, bool[,] argument)
		{
			var bits = argument;

			if (bits == null)
				return new List<Point> ().GetEnumerator ();

			if (partialSolution.Count < 1)
			{
				return new List<Point> () { new Point (1, 1) }.GetEnumerator ();
			}
			var position = partialSolution.Count - 1;
			var v = partialSolution[position];

			return new List<Point> () { new Point (v.X + 1, v.Y),
								  		new Point (v.X - 1, v.Y),
								  		new Point (v.X, v.Y + 1),
								  		new Point (v.X, v.Y - 1)
									  }.GetEnumerator ();
		}

		public static bool[,] CreateArgument (char[,] searchSpace)
		{
			var bits = new bool[searchSpace.GetLength (0), searchSpace.GetLength (1)];

			for (int i = 0; i < bits.GetLength (0); ++i)
			{
				for (int j = 0; j < bits.GetLength (1); ++j)
				{
					bits [i, j] = (searchSpace[i, j] == '*');
				}
			}
			return bits;
		}

		public static void ResetArgument (Point value, char[,] searchSpace, bool[,] argument)
		{
			var bits = argument;

			if (bits == null)
				return ;

			if (value.X < 0 || value.X >= bits.GetLength (0))
				return ;

			if (value.Y < 0 || value.Y >= bits.GetLength (1))
				return ;
			
			bits [value.X, value.Y] = false;
		}
	}
}
