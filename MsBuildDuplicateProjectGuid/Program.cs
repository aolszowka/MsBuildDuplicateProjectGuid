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
    using System.Text;

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
            StringBuilder message = new StringBuilder();
            message.AppendLine("Scans given directory for MsBuild Projects, looking for duplicate ProjectGuids.");
            message.AppendLine("Invalid Command/Arguments. Valid commands are:");
            message.AppendLine();
            message.AppendLine("[directory] - [READS] Spins through the specified directory\n" +
                               "              and all subdirectories for Project files prints\n" +
                               "              projects which have duplicated ProjectGuid.\n" +
                               "              Returns the number of duplicated Guids.");
            Console.WriteLine(message);
            return 21;
        }

        private static int FindDuplicateGuids(string targetDirectory)
        {
            KeyValuePair<string, ConcurrentBag<string>>[] results = DuplicateProjectGuid.Find(targetDirectory).ToArray();

            foreach (KeyValuePair<string, ConcurrentBag<string>> kvp in results)
            {
                Console.WriteLine($"Key `{kvp.Key}` duplicated in projects:");
                foreach (string duplicateProject in kvp.Value)
                {
                    Console.WriteLine(duplicateProject);
                }
                Console.WriteLine();
            }

            return results.Length;
        }
    }
}
