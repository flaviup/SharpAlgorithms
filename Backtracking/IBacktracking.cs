
// Flaviu Pasca
// flaviup @ gmail.com
// (C) 2015

using System;
using System.Collections.Generic;

namespace SharpAlgorithms
{
	public interface IBacktracking<T, ArgType> : IEnumerable<IEnumerable<T>>
	{
		IBacktrackingConfigurator<T, ArgType> Configurator { get; }
	}
}
