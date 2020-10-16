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
    using System.Xml.Linq;

    using MsBuildDuplicateProjectGuid.Properties;

    using NDesk.Options;

    class Program
    {
        static void Main(string[] args)
        {
            string targetDirectory = string.Empty;
            bool showHelp = false;
            bool xmlOutput = false;

            OptionSet p = new OptionSet()
            {
                { "<>", Strings.TargetDirectoryArgument, v => targetDirectory = v },
                { "xml", Strings.XmlOutputFlag, v => xmlOutput = v != null },
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
                    KeyValuePair<string, ConcurrentBag<string>>[] results =
                        DuplicateProjectGuid
                            .Find(targetDirectory)
                            .ToArray();

                    if (xmlOutput)
                    {
                        PrintXmlToConsole(results);
                    }
                    else
                    {
                        PrintToConsole(results);
                    }

                    Environment.ExitCode = results.Length;
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

        static void PrintXmlToConsole(IEnumerable<KeyValuePair<string, ConcurrentBag<string>>> duplicateGuids)
        {
            XDocument outputDocument = new XDocument(new XDeclaration("1.0", null, null));
            outputDocument.Add(new XElement(Strings.ProgramName));

            foreach (KeyValuePair<string, ConcurrentBag<string>> duplicateGuid in duplicateGuids)
            {
                XElement projectElement = new XElement("Duplicate", new XAttribute("GUID", duplicateGuid.Key));
                foreach (string duplicatedProject in duplicateGuid.Value)
                {
                    XElement duplicatedProjectElement =
                        new XElement("Project", new XAttribute("Path", duplicatedProject));

                    projectElement.Add(duplicatedProjectElement);
                }
                outputDocument.Root.Add(projectElement);
            }

            Console.WriteLine(outputDocument.ToString());
        }

        static void PrintToConsole(IEnumerable<KeyValuePair<string, ConcurrentBag<string>>> duplicateGuids)
        {
            foreach (KeyValuePair<string, ConcurrentBag<string>> kvp in duplicateGuids)
            {
                Console.WriteLine($"Key `{kvp.Key}` duplicated in projects:");
                foreach (string duplicateProject in kvp.Value)
                {
                    Console.WriteLine(duplicateProject);
                }
                Console.WriteLine();
            }
        }
    }
}
