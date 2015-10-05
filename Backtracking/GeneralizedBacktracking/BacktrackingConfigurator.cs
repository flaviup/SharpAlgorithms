
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

using System;
using System.Collections.Generic;

namespace SharpAlgorithms.GeneralizedBacktracking
{
	public class BacktrackingConfigurator<T, SearchSpaceType, ArgType> : IBacktrackingConfigurator<T, SearchSpaceType, ArgType>
	{
		public BacktrackingConfigurator (SearchSpaceType searchSpace,
										 Func<IReadOnlyList<T>, SearchSpaceType, ArgType, bool> partialChecker, 
										 Func<IReadOnlyList<T>, SearchSpaceType, ArgType, bool> totalChecker, 
										 Func<IReadOnlyList<T>, SearchSpaceType, ArgType, IEnumerator<T>> generator,
										 Func<SearchSpaceType, ArgType> createArgument = null,
										 Action<T, SearchSpaceType, ArgType> resetArgument = null,
										 IReadOnlyList<T> initialState = null,
										 IReadOnlyList<T> lastState = null
										)
		{
			SearchSpace = searchSpace;
			PartialChecker = partialChecker;
			TotalChecker = totalChecker;
			Generator = generator;
			CreateArgument = createArgument;
			ResetArgument = resetArgument;
			InitialState  = initialState;
			LastState = lastState;
		}

		#region IBacktracking implementation

		public SearchSpaceType SearchSpace
		{
			get;
			private set;
		}

		public Func<IReadOnlyList<T>, SearchSpaceType, ArgType, bool> PartialChecker
		{
			get;
			private set;
		}

		public Func<IReadOnlyList<T>, SearchSpaceType, ArgType, bool> TotalChecker
		{
			get;
			private set;
		}

		public Func<IReadOnlyList<T>, SearchSpaceType, ArgType, IEnumerator<T>> Generator
		{
			get;
			private set;
		}

		public Func<SearchSpaceType, ArgType> CreateArgument
		{
			get;
			private set;
		}

		public Action<T, SearchSpaceType, ArgType> ResetArgument
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
