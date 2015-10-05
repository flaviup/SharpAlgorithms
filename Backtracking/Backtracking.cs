
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

//#define TYPESAFE_BACKTRACKING

using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpAlgorithms
{
	public class Backtracking<T, ArgType> : IBacktracking<T, ArgType>
	{
		public Backtracking (IBacktrackingConfigurator<T, ArgType> configurator)
		{
			if (configurator == null)
				throw new Exception ("No configurator provided.");

			Configurator = configurator;
		}

		#region IEnumerable implementation

		public IEnumerator<IEnumerable<T>> GetEnumerator ()
		{
			var size = Configurator.Size;
			var argument = Configurator.CreateArgument?.Invoke (size);

			int currentPosition = 0;
			T[] solution = null;
			IEnumerator<T>[] enumerators = null;
			InitializeState (ref currentPosition, out solution, out enumerators, argument);

			#if TYPESAFE_BACKTRACKING
			var shadowSolution = new T [size];
			#else
			var shadowSolution = solution;
			#endif

			while (currentPosition > -1)
			{
				if (currentPosition == size)
				{
					#if TYPESAFE_BACKTRACKING
					solution.CopyTo (shadowSolution, 0);
					#endif

					if (Configurator.TotalChecker?.Invoke (shadowSolution, argument) ?? false)
					{
						yield return shadowSolution;
					}

					if (Configurator.LastState != null && ReachedLastState (solution))
					{
						foreach (var enumerator in enumerators)
						{
							enumerator?.Dispose ();
						}
						yield break ;
					}
					--currentPosition;
				}
				else
				{

					if (enumerators [currentPosition] != null)
					{
						Configurator.ResetArgument?.Invoke (solution [currentPosition], currentPosition, size, argument);
					}
					enumerators [currentPosition] = enumerators [currentPosition] ?? GenerateNewOnPosition (currentPosition, solution, argument);
					bool advance = false;

					while (!advance && enumerators [currentPosition].MoveNext ())
					{
						solution [currentPosition] = enumerators [currentPosition].Current;

						#if TYPESAFE_BACKTRACKING
						solution.CopyTo (shadowSolution, 0);
						#endif

						if (Configurator.PartialChecker?.Invoke (shadowSolution, currentPosition, argument) ?? false)
						{
							++currentPosition;
							advance = true;
						}
					}

					if (!advance)
					{
						enumerators [currentPosition].Dispose ();
						enumerators [currentPosition] = null;
						solution [currentPosition] = default (T);
						--currentPosition;
					}
				}
			}
			yield break ;
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}

		#endregion

		#region IBacktracking implementation

		public IBacktrackingConfigurator<T, ArgType> Configurator
		{
			get;
			private set;
		}

		#endregion

		protected virtual IEnumerator<T> GenerateNewOnPosition (int currentPosition, T[] solution, ArgType argument)
		{
			if (Configurator.PositionalGenerator != null)
				return Configurator.PositionalGenerator (currentPosition, Configurator.Size, argument);

			if (Configurator.PartialBacktrackGenerator != null)
			{
				#if TYPESAFE_BACKTRACKING
				var partialSolution = new T [currentPosition];
				Array.Copy (solution, partialSolution, currentPosition);
				#else
				var partialSolution = solution;
				#endif

				return Configurator.PartialBacktrackGenerator (partialSolution, currentPosition, argument);
			}

			return null;
		}

		protected virtual void InitializeState (ref int currentPosition, out T[] solution, out IEnumerator<T>[] enumerators, ArgType argument)
		{
			solution = new T [Configurator.Size];
			enumerators = new IEnumerator<T> [Configurator.Size];

			if (Configurator.InitialState == null || Configurator.InitialState.Count == 0)
			{
				currentPosition = 0;
				return;
			}
			#if TYPESAFE_BACKTRACKING
			var shadowSolution = new T[Configurator.Size];
			#else
			var shadowSolution = solution;
			#endif
			int i = 0;

			foreach (var item in Configurator.InitialState)
			{
				solution [i] = item;
				enumerators [i] = GenerateNewOnPosition (i, solution, argument);
				bool advance = false;

				while (!advance && enumerators [i].MoveNext ())
				{
					if (enumerators [i].Current.Equals (item))
					{
						#if TYPESAFE_BACKTRACKING
						solution.CopyTo (shadowSolution, 0);
						#endif

						if (Configurator.PartialChecker?.Invoke (shadowSolution, i, argument) ?? false)
						{
							advance = true;
						}
					}
				}

				if (!advance)
					break;
				++i;
			}
			currentPosition = i;
		}

		protected virtual bool ReachedLastState (T[] solution)
		{
			int i = 0;

			foreach (var item in solution)
			{
				if (!item.Equals (Configurator.LastState [i++]))
					return false;
			}
			return true;
		}
	}
}
