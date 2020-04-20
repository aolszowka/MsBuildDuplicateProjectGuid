// -----------------------------------------------------------------------
// <copyright file="DuplicateProjectGuid.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildDuplicateProjectGuid
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class DuplicateProjectGuid
    {
        internal static ConcurrentDictionary<string, ConcurrentBag<string>> Find(string targetDirectory)
        {
            IEnumerable<string> projectFiles = GetProjectsInDirectory(targetDirectory);

            ConcurrentDictionary<string, string> distinctProjectGuids = new ConcurrentDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            ConcurrentDictionary<string, ConcurrentBag<string>> duplicatedGuids = new ConcurrentDictionary<string, ConcurrentBag<string>>(StringComparer.InvariantCultureIgnoreCase);

            Parallel.ForEach(projectFiles, projectFile =>
            {
                string projectGuid = MSBuildUtilities.GetMSBuildProjectGuid(projectFile);

                if (!distinctProjectGuids.TryAdd(projectGuid, projectFile))
                {
                    // The Key was duplicated

                    // First try to create a new bag (assume its the first duplication we have)
                    ConcurrentBag<string> duplicatedProjects = new ConcurrentBag<string>(new string[] { distinctProjectGuids[projectGuid] });

                    if (!duplicatedGuids.TryAdd(projectGuid, duplicatedProjects))
                    {
                        duplicatedProjects = duplicatedGuids[projectGuid];
                    }

                    duplicatedProjects.Add(projectFile);
                }
            }
            );

            return duplicatedGuids;
        }

        /// <summary>
        /// Gets all Project Files that are understood by this
        /// tool from the given directory and all subdirectories.
        /// </summary>
        /// <param name="targetDirectory">The directory to scan for projects.</param>
        /// <returns>All projects that this tool supports.</returns>
        internal static IEnumerable<string> GetProjectsInDirectory(string targetDirectory)
        {
            HashSet<string> supportedFileExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                ".csproj",
                ".fsproj",
                ".sqlproj",
                ".synproj",
                ".vbproj",
            };

            return
                Directory
                .EnumerateFiles(targetDirectory, "*proj", SearchOption.AllDirectories)
                .Where(currentFile => supportedFileExtensions.Contains(Path.GetExtension(currentFile)));
        }
    }
}
