// <copyright file="LineProbeResolver.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Datadog.Trace.Debugger.Configurations;
using Datadog.Trace.Debugger.Configurations.Models;
using Datadog.Trace.Debugger.Models;
using Datadog.Trace.ExtensionMethods;
using Datadog.Trace.Logging;
using Datadog.Trace.Pdb;
using Datadog.Trace.Vendors.dnlib.DotNet.Pdb.Symbols;

namespace Datadog.Trace.Debugger
{
    internal class LineProbeResolver : ILineProbeResolver
    {
        private static readonly IDatadogLogger Log = DatadogLogging.GetLoggerFor<LineProbeResolver>();

        private readonly object _locker;
        private readonly Dictionary<Assembly, Trie> _loadedAssemblies;

        private LineProbeResolver()
        {
            _locker = new object();
            _loadedAssemblies = new Dictionary<Assembly, Trie>();
        }

        public static LineProbeResolver Create()
        {
            return new LineProbeResolver();
        }

        private static IList<SymbolDocument> GetDocumentsFromPDB(Assembly loadedAssembly)
        {
            try
            {
                if (loadedAssembly.IsDynamic ||
                    loadedAssembly.ManifestModule.IsResource() ||
                    string.IsNullOrWhiteSpace(loadedAssembly.Location) ||
                    IsThirdPartyCode(loadedAssembly))
                {
                    return null;
                }

                using var reader = DatadogPdbReader.CreatePdbReader(loadedAssembly);
                if (reader != null)
                {
                    return reader.GetDocuments();
                }
            }
            catch (Exception e)
            {
                Log.Warning(e, $"Failed to retrieve documents from PDB for {loadedAssembly.Location}");
            }

            return null;
        }

        private static bool IsThirdPartyCode(Assembly loadedAssembly)
        {
            // This implementation is just a stub - we will need to replace it
            // with a proper implementation in the future.
            string[] thirdPartyStartsWith = { "Microsoft", "System" };

            var assemblyName = loadedAssembly.GetName().Name;
            return thirdPartyStartsWith.Any(t => assemblyName.StartsWith(t));
        }

        private static string GetReversePath(string documentFullPath)
        {
            var partsReverse = documentFullPath.Split(' ', Path.PathSeparator).Reverse();
            return string.Join(Path.PathSeparator.ToString(), partsReverse);
        }

        private Trie GetSourceFilePathForAssembly(Assembly loadedAssembly)
        {
            if (_loadedAssemblies.TryGetValue(loadedAssembly, out var trie))
            {
                return trie;
            }

            return _loadedAssemblies[loadedAssembly] = CreateTrieForSourceFilePaths(loadedAssembly);
        }

        private Trie CreateTrieForSourceFilePaths(Assembly loadedAssembly)
        {
            var documents = GetDocumentsFromPDB(loadedAssembly);
            if (documents == null)
            {
                return null; // No PDB available or unsupported assembly
            }

            var trie = new Trie();
            foreach (var symbolDocument in documents)
            {
                trie.Insert(GetReversePath(symbolDocument.URL));
            }

            return trie;
        }

        private Assembly FindAssemblyContainingFile(string sourceFileFullPath)
        {
            lock (_locker)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var trie = GetSourceFilePathForAssembly(assembly);

                    if (trie != null && trie.ContainsPrefix(GetReversePath(sourceFileFullPath)))
                    {
                        return assembly;
                    }
                }
            }

            return null;
        }

        public LineProbeResolveResult TryResolveLineProbe(ProbeDefinition probe, out BoundLineProbeLocation location)
        {
            location = null;
            var assembly = FindAssemblyContainingFile(probe.Where.SourceFile);
            if (assembly == null)
            {
                var message = $"Could not find a source file location for probe {probe.Id}.";
                Log.Information(message);
                return new LineProbeResolveResult(LiveProbeResolveStatus.Unbound, message);
            }

            using var pdbReader = DatadogPdbReader.CreatePdbReader(assembly);
            if (pdbReader == null)
            {
                var message = $"Failed to read from PDB for probe ID {probe.Id}";
                Log.Information(message);

                return new LineProbeResolveResult(LiveProbeResolveStatus.Error, message);
            }

            if (probe.Where.Lines?.Length != 1 || !int.TryParse(probe.Where.Lines[0], out var lineNum))
            {
                var message = $"Failed to parse line number for Line Probe {probe.Id}. " +
                              $"The Lines collection contains {PrintContents(probe.Where.Lines)}.";
                Log.Error(message);

                return new LineProbeResolveResult(LiveProbeResolveStatus.Error, message);
            }

            var method = pdbReader.GetContainingMethodAndOffset(probe.Where.SourceFile, lineNum, column: 0, out var bytecodeOffset);
            location = new BoundLineProbeLocation(probe, assembly.ManifestModule.ModuleVersionId, method.Token, bytecodeOffset, lineNum);
            return new LineProbeResolveResult(LiveProbeResolveStatus.Bound);

            string PrintContents<T>(T[] array)
            {
                const string separator = ", ";
                return array == null ? "null" : $"[{string.Join(separator, array)}]";
            }
        }

        public void OnDomainUnloaded()
        {
            lock (_locker)
            {
                foreach (var unloadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    _loadedAssemblies.Remove(unloadedAssembly);
                }
            }
        }
    }
}
