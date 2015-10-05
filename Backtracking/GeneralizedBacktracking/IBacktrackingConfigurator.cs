
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

using System;
using System.Collections.Generic;

namespace SharpAlgorithms.GeneralizedBacktracking
{
	public interface IBacktrackingConfigurator<T, SearchSpaceType, ArgType>
	{
		SearchSpaceType SearchSpace { get; }
		Func<IReadOnlyList<T>, SearchSpaceType, ArgType, bool> PartialChecker { get; }
		Func<IReadOnlyList<T>, SearchSpaceType, ArgType, bool> TotalChecker { get; }
		Func<IReadOnlyList<T>, SearchSpaceType, ArgType, IEnumerator<T>> Generator { get; }
		Func<SearchSpaceType, ArgType> CreateArgument { get; }
		Action<T, SearchSpaceType, ArgType> ResetArgument { get; }
		IReadOnlyList<T> InitialState { get; }
		IReadOnlyList<T> LastState { get; }
	}
}
