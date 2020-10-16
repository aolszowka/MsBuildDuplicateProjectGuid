// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildDuplicateProjectGuid
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;

    using MsBuildDuplicateProjectGuid.Properties;

    using NDesk.Options;

    class Program
    {
        static void Main(string[] args)
        {
            string targetDirectory = string.Empty;
            bool showHelp = false;

            OptionSet p = new OptionSet()
            {
                { "<>", Strings.TargetDirectoryArgument, v => targetDirectory = v },
                { "?|h|help", Strings.HelpDescription, v => showHelp = v != null },
            };

            try
            {
                p.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine(Strings.ShortUsageMessage);
                Console.WriteLine($"Try `{Strings.ProgramName} --help` for more information.");
                Environment.Exit(21);
            }

            if (showHelp || string.IsNullOrEmpty(targetDirectory))
            {
                int exitCode = ShowUsage(p);
                Environment.Exit(exitCode);
            }
            else
            {
                if (Directory.Exists(targetDirectory))
                {
                    Environment.ExitCode = PrintToConsole(targetDirectory);
                }
                else
                {
                    string error = string.Format(Strings.InvalidDirectoryArgument, targetDirectory);
                    Console.WriteLine(error);
                    Environment.ExitCode = 9009;
                }
            }
        }

        private static int ShowUsage(OptionSet p)
        {
            Console.WriteLine(Strings.ShortUsageMessage);
            Console.WriteLine();
            Console.WriteLine(Strings.LongDescription);
            Console.WriteLine();
            Console.WriteLine($"              <>            {Strings.TargetDirectoryArgument}");
            p.WriteOptionDescriptions(Console.Out);
            return 21;
        }

        private static int PrintToConsole(string targetDirectory)
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
