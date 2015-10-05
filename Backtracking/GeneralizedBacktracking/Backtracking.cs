
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

//#define TYPESAFE_BACKTRACKING

using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpAlgorithms.GeneralizedBacktracking
{
	public class Backtracking<T, SearchSpaceType, ArgType> : IBacktracking<T, SearchSpaceType, ArgType>
	{
		public Backtracking (IBacktrackingConfigurator<T, SearchSpaceType, ArgType> configurator)
		{
			if (configurator == null)
				throw new Exception ("No configurator provided.");

			Configurator = configurator;
		}

		#region IEnumerable implementation

		public IEnumerator<IEnumerable<T>> GetEnumerator ()
		{
			var searchSpace = Configurator.SearchSpace;
			var argument = Configurator.CreateArgument?.Invoke (searchSpace);

			List<T> solution = null;
			List<IEnumerator<T>> enumerators = null;
			InitializeState (out solution, out enumerators, argument);

			#if TYPESAFE_BACKTRACKING
			#else
			IReadOnlyList<T> shadowSolution = solution;
			#endif

			while (solution.Count > 0)
			{
				#if TYPESAFE_BACKTRACKING
				IReadOnlyList<T> shadowSolution = new List<T> (solution);
				#endif

				bool foundValidSolution = false;

				if (Configurator.TotalChecker?.Invoke (shadowSolution, searchSpace, argument) ?? false)
				{
					yield return shadowSolution;

					if (Configurator.LastState != null && ReachedLastState (solution))
					{
						foreach (var enumerator in enumerators)
						{
							enumerator?.Dispose ();
						}
						yield break ;
					}
					foundValidSolution = true;
				}

				if (foundValidSolution && solution.Count > 1)
				{
					int currentPosition = solution.Count - 1;
					solution.RemoveAt (currentPosition);
				}
				else
				{
					int currentPosition = enumerators.Count - 1;

					if (enumerators.Count > solution.Count)
					{
						Configurator.ResetArgument?.Invoke (enumerators [currentPosition].Current, searchSpace, argument);
					}
					else
					{
						enumerators.Add (GenerateNew (solution, searchSpace, argument));
						currentPosition = enumerators.Count - 1;
					}
					bool advance = false;

					while (!advance && enumerators [currentPosition].MoveNext ())
					{
						var item = enumerators [currentPosition].Current;

						if (solution.Count > currentPosition)
						{
							solution [currentPosition] = item;
						}
						else
						{
							solution.Add (item);
						}

						#if TYPESAFE_BACKTRACKING
						shadowSolution = new List<T> (solution);
						#endif

						if (Configurator.PartialChecker?.Invoke (shadowSolution, searchSpace, argument) ?? false)
						{
							advance = true;
						}
					}

					if (!advance)
					{
						if (enumerators.Count == solution.Count)
						{
							solution.RemoveAt (currentPosition);
						}
						enumerators [currentPosition].Dispose ();
						enumerators.RemoveAt (currentPosition);
						solution.RemoveAt (currentPosition - 1);
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

		public IBacktrackingConfigurator<T, SearchSpaceType, ArgType> Configurator
		{
			get;
			private set;
		}

		#endregion
	
		protected virtual IEnumerator<T> GenerateNew (IReadOnlyList<T> solution, SearchSpaceType searchSpace, ArgType argument)
		{
			if (Configurator.Generator != null)
			{
				#if TYPESAFE_BACKTRACKING
				IReadOnlyList<T> shadowSolution = new List<T> (solution);
				#else
				var shadowSolution = solution;
				#endif

				return Configurator.Generator (shadowSolution, searchSpace, argument);
			}

			return null;
		}

		protected virtual void InitializeState (out List<T> solution, out List<IEnumerator<T>> enumerators, ArgType argument)
		{
			solution = new List<T> ();
			enumerators = new List<IEnumerator<T>> ();

			#if TYPESAFE_BACKTRACKING
			#else
			IReadOnlyList<T> shadowSolution = solution;
			#endif

			if (Configurator.InitialState == null || Configurator.InitialState.Count == 0)
			{
				var enumerator = GenerateNew (solution, Configurator.SearchSpace, argument);
				enumerators.Add (enumerator);

				bool advance = false;

				while (!advance && enumerator.MoveNext ())
				{
					if (solution.Count == 0)
					{
						solution.Add (enumerator.Current);
					}
					else
					{
						solution [0] = enumerator.Current;
					}

					#if TYPESAFE_BACKTRACKING
					IReadOnlyList<T> shadowSolution = new List<T> (solution);
					#endif

					if (Configurator.PartialChecker?.Invoke (shadowSolution, Configurator.SearchSpace, argument) ?? false)
					{
						advance = true;
					}
				}

				if (!advance)
				{
					solution.Clear ();
				}
				return;
			}
			int i = 0;

			foreach (var item in Configurator.InitialState)
			{
				var enumerator = GenerateNew (solution, Configurator.SearchSpace, argument);
				enumerators.Add (enumerator);
				solution.Add (item);
				bool advance = false;

				while (!advance && enumerator.MoveNext ())
				{
					if (enumerator.Current.Equals (item))
					{
						#if TYPESAFE_BACKTRACKING
						IReadOnlyList<T> shadowSolution = new List<T> (solution);
						#endif

						if (Configurator.PartialChecker?.Invoke (shadowSolution, Configurator.SearchSpace, argument) ?? false)
						{
							advance = true;
						}
					}
				}

				if (!advance)
				{
					enumerators.RemoveAt (i);
					solution.RemoveAt (i);
					break;
				}
				++i;
			}
		}

		protected virtual bool ReachedLastState (IList<T> solution)
		{
			if (solution.Count != Configurator.LastState.Count)
				return false;
			
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
