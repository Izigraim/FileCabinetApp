using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Print command.
    /// </summary>
    public class PrintHelpCommandHandler : CommandHandlerBase
    {
        private static string[] commands = new string[] { "help", "exit", "stat", "create", "list", "find", "export", "import", "purge", "insert", "delete", "update" };
        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the count of records", "The 'stat' command prints the count of records." },
            new string[] { "create", "create a record", "The 'create' command create a record." },
            new string[] { "list", "returns list of records", "The 'list' command returns list of records." },
            new string[] { "find", "find a record or records by property", "The 'find' command find a record or records by property." },
            new string[] { "export", "export data to file", "The 'export' command export a records to file" },
            new string[] { "import", "import data from file", "The 'import' command import records from file" },
            new string[] { "purge", "purge a file with records", "The 'purge' command remove records marked as deleted from file." },
            new string[] { "insert", "create or edit a record", "The 'insert' command create or edit a record." },
            new string[] { "delete", "delete record(s) using the specified criteria", "The 'delete' command delete record(s) using the specified criteria." },
            new string[] { "update", "update a record(s) that matches the search criteria with the specified fields", "The 'update' command update a record(s) that matches the search criteria with the specified fields." },
        };

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!commands.Contains(request.Command.ToLower(new CultureInfo("en-US"))))
            {
                List<string> similarCommands = new List<string>();
                Console.WriteLine($"There is no '{request.Command}' command.");

                foreach (string command in commands)
                {
                    if (FindDifference(command, request.Command) < 1)
                    {
                        similarCommands.Add(command);
                    }
                }

                if (similarCommands.Count == 0)
                {
                    foreach (string command in commands)
                    {
                        if (FindDifference(command, request.Command) < 2)
                        {
                            similarCommands.Add(command);
                        }
                    }
                }

                if (similarCommands.Count == 0)
                {
                    foreach (string command in commands)
                    {
                        if (FindDifference(command, request.Command) < 3)
                        {
                            similarCommands.Add(command);
                        }
                    }
                }

                if (similarCommands.Count == 0)
                {
                    foreach (string command in commands)
                    {
                        if (command[0] == request.Command[0])
                        {
                            similarCommands.Add(command);
                        }
                    }
                }

                if (similarCommands.Count != 0)
                {
                    Console.WriteLine($"The most similar commands are");
                }
                else
                {
                    Console.WriteLine("Try using the 'help' command to find the necessary command.");
                }

                foreach (string command in similarCommands)
                {
                    Console.WriteLine("\t\t" + command);
                }

                Console.WriteLine();
            }

            if (request.Command.ToLower(new CultureInfo("en-US")) == "help")
            {
                PrintHelp(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static int FindDifference(string first, string second)
        {
            int n = first.Length;
            int m = second.Length;
            int[,] array = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; array[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; array[0, j] = j++)
            {
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (second[j - 1] == first[i - 1]) ? 0 : 1;

                    array[i, j] = Math.Min(Math.Min(array[i - 1, j] + 1, array[i, j - 1] + 1), array[i - 1, j - 1] + cost);
                }
            }

            return array[n, m];
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
