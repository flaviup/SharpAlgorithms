
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

using System;
using System.Collections.Generic;

namespace SharpAlgorithms
{
	public interface IBacktrackingConfigurator<T, ArgType>
	{
		int Size { get; }
		Func<IReadOnlyList<T>, int, ArgType, bool> PartialChecker { get; }
		Func<IReadOnlyList<T>, ArgType, bool> TotalChecker { get; }
		Func<int, int, ArgType, IEnumerator<T>> PositionalGenerator { get; }
		Func<IReadOnlyList<T>, int, ArgType, IEnumerator<T>> PartialBacktrackGenerator { get; }
		Func<int, ArgType> CreateArgument { get; }
		Action<T, int, int, ArgType> ResetArgument { get; }
		IReadOnlyList<T> InitialState { get; }
		IReadOnlyList<T> LastState { get; }
	}
}
