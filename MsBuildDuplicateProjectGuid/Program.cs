// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildDuplicateProjectGuid
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            int errorCode = 0;

            if (args.Any())
            {
                string command = args.First().ToLowerInvariant();

                if (command.Equals("-?") || command.Equals("/?") || command.Equals("-help") || command.Equals("/help"))
                {
                    errorCode = ShowUsage();
                }
                else
                {
                    if (Directory.Exists(command))
                    {
                        string targetPath = command;
                        errorCode = FindDuplicateGuids(targetPath);
                    }
                    else
                    {
                        string error = string.Format("The specified path `{0}` is not valid.", command);
                        Console.WriteLine(error);
                        errorCode = 1;
                    }
                }
            }
            else
            {
                // This was a bad command
                errorCode = ShowUsage();
            }

            Environment.Exit(errorCode);
        }

        private static int ShowUsage()
        {
            throw new NotImplementedException();
        }

        private static int FindDuplicateGuids(string targetDirectory)
        {
            KeyValuePair<string, ConcurrentBag<string>>[] results = DuplicateProjectGuid.Find(targetDirectory).ToArray();

            foreach (KeyValuePair<string, ConcurrentBag<string>> kvp in results)
            {
                string duplicatedProjects = string.Join(",", kvp.Value);
                Console.WriteLine($"Key `{kvp.Key}` duplicated in projects: {duplicatedProjects}");
            }

            return results.Length;
        }
    }
}
