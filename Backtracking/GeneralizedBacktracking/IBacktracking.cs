
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

using System;
using System.Collections.Generic;

namespace SharpAlgorithms.GeneralizedBacktracking
{
	public interface IBacktracking<T, SearchSpaceType, ArgType> : IEnumerable<IEnumerable<T>>
	{
		IBacktrackingConfigurator<T, SearchSpaceType, ArgType> Configurator { get; }
	}
}
