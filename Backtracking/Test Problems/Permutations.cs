using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;

using System.Threading.Tasks;

using System.Linq;

namespace SharpAlgorithms
{
	public class Permutations
	{
		public Permutations ()
		{
		}

		private const int Size = 10;

		public void Test ()
		{
			var configurator = new BacktrackingConfigurator<int, BitArray> (Size, 
																			PartialCheck, 
																			TotalCheck,
																			GeneratePartiallyBacktrack,
																		    CreateArgument,
																			ResetArgument/*,
																			new [] { 3, 1, 2, 4 },
																			new [] { 4, 1, 2, 3 }*/);
			var backtracking = new Backtracking<int, BitArray> (configurator);
			int total = 0;

			foreach (var solution in backtracking)
			{
				++total;
			}
			Console.WriteLine ($"Serial Total = {total}");

			total = 0;

			var sums = new int[Size];

			Parallel.For (1, Size + 1, (int i) => {
			//for (int i = 1; i < Size + 1; i++) {
				var init = new int[Size];
				var last = new int[Size];
				init [0] = last [0] = i;

				int v = 1, u = Size;

				for (int p = 1; p < Size; ++p) {
					if (v == i)
						++v;
					init [p] = v++;

					if (u == i)
						--u;
					last [p] = u--;
				}
				var configurator2 = new BacktrackingConfigurator<int, BitArray> (Size, 
															                     PartialCheck, 
															                     TotalCheck,
															                     GeneratePartiallyBacktrack,
															                     CreateArgument,
															                     ResetArgument,
															                     init,
															                     last);
				var backtracking2 = new Backtracking<int, BitArray> (configurator2);
				int tot = 0;

				foreach (var solution in backtracking2)
				{
					++tot;
				}
				sums [i - 1] = tot;
			//}
			});

			Array.ForEach (sums, (o) => total += o);
			Console.WriteLine ($"Parallel Total = {total}");
			/*var e1 = backtracking.GetEnumerator ();

			while (e1.MoveNext ())
			{
				foreach (var part in e1.Current)
				{
					Console.Write ($"{part}");	
				}
				Console.WriteLine ();
				var e2 = backtracking.GetEnumerator ();

				while (e2.MoveNext ())
				{
					foreach (var par in e2.Current)
					{
						Console.Write ($"{par}");	
					}
					Console.Write (" ");
				}
				Console.WriteLine ();
				Console.WriteLine ();
			}*/

			/*foreach (var solution in backtracking)
			{
				foreach (var part in solution)
				{
					Console.Write ($"{part}");	
				}
				Console.WriteLine ();
				foreach (var sol in backtracking)
				{
					foreach (var part2 in sol)
					{
						Console.Write ($"{part2}");	
					}
					Console.Write (" ");
				}
				Console.WriteLine ();
				Console.WriteLine ();
			}*/
		}

		public static bool PartialCheck (IReadOnlyList<int> partialSolution, int position, BitArray argument)
		{
			var bits = argument;

			if (bits == null)
				return false;
			
			var size = partialSolution.Count;
			var value = partialSolution [position];

			if (value < 1 || value > size || bits [value - 1])
				return false;

			bits [value - 1] = true;

			return true;
		}

		public static bool TotalCheck (IReadOnlyList<int> solution, BitArray argument)
		{
			var bits = argument;

			if (bits == null)
				return false;

			var size = solution.Count;

			for (int i = 0; i < size; ++i)
			{
				if (!bits [i])
					return false;
			}
			int sum = 0;

			foreach (var item in solution) {
				sum += item;
			}
			return sum == ((size * (size + 1)) >> 1);
		}

		public static IEnumerator<int> GeneratePositionally (int position, int size, BitArray argument)
		{
			return Enumerable.Range (1, size).GetEnumerator ();
		}

		public static IEnumerator<int> GeneratePartiallyBacktrack (IReadOnlyList<int> partialSolution, int position, BitArray argument)
		{
			var bits = argument;

			if (bits == null)
				return Enumerable.Range (1, 0).GetEnumerator ();

			var size = partialSolution.Count;

			for (int i = 0; i < size; ++i)
			{
				if (!bits [i])
					return Enumerable.Range (i + 1, size - i).GetEnumerator ();
			}
			return Enumerable.Range (1, 0).GetEnumerator ();
		}

		public static BitArray CreateArgument (int size)
		{
			return new BitArray (size);
		}

		public static void ResetArgument (int value, int position, int size, BitArray argument)
		{
			var bits = argument;

			if (bits == null)
				return ;

			if (value < 1 || value > size)
				return ;

			bits [value - 1] = false;
		}
	}
}
