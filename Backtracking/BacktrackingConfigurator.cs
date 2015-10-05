
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

using System;
using System.Collections.Generic;

namespace SharpAlgorithms
{
	public class BacktrackingConfigurator<T, ArgType> : IBacktrackingConfigurator<T, ArgType>
	{
		public BacktrackingConfigurator (int size,
										 Func<IReadOnlyList<T>, int, ArgType, bool> partialChecker,
										 Func<IReadOnlyList<T>, ArgType, bool> totalChecker, 
										 Func<int, int, ArgType, IEnumerator<T>> positionalGenerator,
										 Func<int, ArgType> createArgument = null,
										 Action<T, int, int, ArgType> resetArgument = null,
										 IReadOnlyList<T> initialState = null,
										 IReadOnlyList<T> lastState = null
										)
		{
			Size = size;
			PartialChecker = partialChecker;
			TotalChecker = totalChecker;
			PositionalGenerator = positionalGenerator;
			CreateArgument = createArgument;
			ResetArgument = resetArgument;
			InitialState = initialState;
			LastState = lastState;
		}

		public BacktrackingConfigurator (int size,
										 Func<IReadOnlyList<T>, int, ArgType, bool> partialChecker,
										 Func<IReadOnlyList<T>, ArgType, bool> totalChecker, 
										 Func<IReadOnlyList<T>, int, ArgType, IEnumerator<T>> partialBacktrackGenerator,
										 Func<int, ArgType> createArgument = null,
										 Action<T, int, int, ArgType> resetArgument = null,
										 IReadOnlyList<T> initialState = null,
										 IReadOnlyList<T> lastState = null
										)
		{
			Size = size;
			PartialChecker = partialChecker;
			TotalChecker = totalChecker;
			PartialBacktrackGenerator = partialBacktrackGenerator;
			CreateArgument = createArgument;
			ResetArgument = resetArgument;
			InitialState  = initialState;
			LastState = lastState;
		}

		#region IBacktracking implementation

		public int Size
		{
			get;
			private set;
		}

		public Func<IReadOnlyList<T>, int, ArgType, bool> PartialChecker
		{
			get;
			private set;
		}

		public Func<IReadOnlyList<T>, ArgType, bool> TotalChecker
		{
			get;
			private set;
		}

		public Func<int, int, ArgType, IEnumerator<T>> PositionalGenerator
		{
			get;
			private set;
		}

		public Func<IReadOnlyList<T>, int, ArgType, IEnumerator<T>> PartialBacktrackGenerator
		{
			get;
			private set;
		}

		public Func<int, ArgType> CreateArgument
		{
			get;
			private set;
		}

		public Action<T, int, int, ArgType> ResetArgument
		{
			get;
			private set;
		}

		public IReadOnlyList<T> InitialState
		{
			get;
			private set;
		}

		public IReadOnlyList<T> LastState
		{
			get;
			private set;
		}

		#endregion
	}
}
