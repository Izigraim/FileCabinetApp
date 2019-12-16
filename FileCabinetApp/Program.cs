using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    /// Class with main method.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Ilya Vrublevsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static IRecordValidator validator;
        private static IFIleCabinetService fileCabinetService;

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the count of records", "The 'stat' command prints the count of records." },
            new string[] { "create", "create a record", "The 'create' command create a record." },
            new string[] { "list", "returns list of records", "The 'list' command returns list of records." },
            new string[] { "edit", "edit a record", "The 'edit' command edit a record" },
            new string[] { "find", "find a record or records by property", "The 'find' command find a record or records by property." },
            new string[] { "export", "export data to file", "The 'export' command export a records to file" },
        };

        /// <summary>
        /// Gets or sets type of validation.
        /// </summary>
        /// <value>
        /// Type of validation.
        /// </value>
        public static string ValidationType { get; set; } = "default";

        public static string StorageType { get; set; } = "file";

        /// <summary>
        /// Start of execution.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            string[] parametersOfCommandLineArray;
            if (args != null && args.Length != 0)
            {
                if (args.Length > 1 && args[0].Trim(' ') == "-v")
                {
                    ValidationType = args[1].ToLower(new CultureInfo("en-US")).Trim(' ');
                }
                else if (args.Length == 1)
                {
                    parametersOfCommandLineArray = args[0].Split('=');

                    if (parametersOfCommandLineArray[0].ToLower(new CultureInfo("en-US")).Trim(' ') == "--validation-rules")
                    {
                        ValidationType = parametersOfCommandLineArray[1].ToLower(new CultureInfo("en-US")).Trim(' ');
                    }
                }
                else
                {
                    parametersOfCommandLineArray = args;
                }

                if (args.Length == 2 && args[0].Trim(' ') == "-s")
                {
                    if (args[1].ToLower(new CultureInfo("en-US")).Trim(' ') == "memory" || args[1].ToLower(new CultureInfo("en-US")).Trim(' ') == "file")
                    {
                        StorageType = args[1].ToLower(new CultureInfo("en-US")).Trim(' ');
                    }
                }
                else if (args.Length == 1)
                {
                    parametersOfCommandLineArray = args[0].Split('=');

                    if (parametersOfCommandLineArray[0].Trim(' ') == "--storage" && parametersOfCommandLineArray.Length > 1)
                    {
                        if (parametersOfCommandLineArray[1].ToLower(new CultureInfo("en-US")).Trim(' ') == "memory" || parametersOfCommandLineArray[1].ToLower(new CultureInfo("en-US")).Trim(' ') == "file")
                        {
                            StorageType = parametersOfCommandLineArray[1].ToLower(new CultureInfo("en-US")).Trim(' ');
                        }
                    }
                }
            }

            if (ValidationType != "default" && ValidationType != "custom")
            {
                ValidationType = "default";
            }

            if (ValidationType == "default")
            {
                validator = new DefaultValidation();
            }
            else
            {
                validator = new CustomValidation();
            }

            if (StorageType == "memory")
            {
                fileCabinetService = new FileCabinetMemoryService(validator);
            }
            else
            {
                using FileStream fileStream = File.Open("cabinet-records.db", FileMode.OpenOrCreate);
                fileStream.Close();
                fileCabinetService = new FileCabinetFilesystemService(fileStream, validator);

            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine($"Using '{ValidationType}' validation rules");
            Console.WriteLine($"Using '{StorageType}' storage type");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            FileCabinetRecord record = Program.validator.ValidateParametersProgram();

            if (Program.fileCabinetService.CreateRecord(record) == -1)
            {
                Console.WriteLine("An error occured creating the record.");
            }
            else
            {
                var recordsCount = Program.fileCabinetService.GetStat();
                Console.WriteLine($"Record #{recordsCount} created.");
            }
        }

        private static void Edit(string parameters)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            int id;
            try
            {
                id = Convert.ToInt32(parameters, culture);
                if (id > Program.fileCabinetService.GetStat() || id <= 0)
                {
                    Console.WriteLine($"#{id} record is not found.");
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Incorrext format of ID");
                return;
            }

            FileCabinetRecord record = record = Program.validator.ValidateParametersProgram();

            record.Id = id - 1;

            Program.fileCabinetService.EditRecord(record);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = Program.fileCabinetService.GetRecords();
            for (int i = 0; i < fileCabinetRecords.Count; i++)
            {
                Console.WriteLine($"#{fileCabinetRecords[i].Id + 1}, {fileCabinetRecords[i].Sex}, {fileCabinetRecords[i].FirstName}, {fileCabinetRecords[i].LastName}, {fileCabinetRecords[i].Age}, {fileCabinetRecords[i].Salary}, {fileCabinetRecords[i].DateOfBirth:yyyy-MMM-dd}");
            }
        }

        private static void Find(string parameters)
        {
            try
            {
                if (parameters.Where(c => c == ' ').Count() > 1 || !parameters.Where(c => c == ' ').Any() || parameters[0] == ' ')
                {
                    throw new ArgumentException("Incorrect command format.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            string[] findParameters = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ReadOnlyCollection<FileCabinetRecord> findedRecords = null;

            switch (findParameters[0].ToLower(CultureInfo.CreateSpecificCulture("en-US")))
            {
                case "firstname":
                    findedRecords = fileCabinetService.FindByFirstName(findParameters[1].Trim('"'));
                    break;
                case "lastname":
                    findedRecords = fileCabinetService.FindByLastName(findParameters[1].Trim('"'));
                    break;
                case "dateofbirth":
                    findedRecords = fileCabinetService.FindByDateOfBirth(findParameters[1].Trim('"'));
                    break;
            }

            if (findedRecords == null)
            {
                return;
            }

            foreach (FileCabinetRecord record in findedRecords)
            {
                Console.WriteLine($"#{record.Id + 1}, {record.Sex}, {record.FirstName}, {record.LastName}, {record.Age}, {record.Salary}, {record.DateOfBirth:yyyy-MMM-dd}");
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Export(string parameters)
        {
            try
            {
                if (string.IsNullOrEmpty(parameters.Trim()))
                {
                    throw new ArgumentException("Incorrect parameters.");
                }

                if (parameters.Trim().Where(x => x == ' ').Count() != 1)
                {
                    throw new ArgumentException("Incorrect parameters.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            string[] parametersArray = parameters.Trim().Split(' ');

            var fileCabinetServiceShapshot = fileCabinetService.MakeSnapshot();

            switch (parametersArray[0].ToLower(new CultureInfo("en-US")))
            {
                case "csv":
                    {
                        string path = null;

                        try
                        {
                            path = parametersArray[1];

                            if (!path.Contains(".csv", StringComparison.Ordinal))
                            {
                                throw new ArgumentException("Incorrect file name.");
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            return;
                        }

                        try
                        {
                            string answer = string.Empty;
                            if (!File.Exists(path))
                            {
                                using (FileStream stream = new FileStream(path, FileMode.Create))
                                {
                                    fileCabinetServiceShapshot.SaveToCsv(stream);
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    Console.Write($"File is exist - rewrite {path}? [Y/n] ");
                                    answer = Console.ReadLine();
                                    if (answer.ToLower(new CultureInfo("en-US")) != "y" && answer.ToLower(new CultureInfo("en-US")) != "n")
                                    {
                                        Console.WriteLine("Incorrect answer.");
                                        continue;
                                    }

                                    break;
                                }
                            }

                            if (answer.Trim().ToLower(new CultureInfo("en-US")) == "y")
                            {
                                using (FileStream stream = new FileStream(path, FileMode.Create))
                                {
                                    fileCabinetServiceShapshot.SaveToCsv(stream);
                                }
                            }
                            else if (answer.Trim().ToLower(new CultureInfo("en-US")) == "n")
                            {
                                return;
                            }
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Console.WriteLine($"Export failed: can't open file {path}");
                        }

                        Console.WriteLine($"All records are exported to file {path}");
                    }

                    break;

                case "xml":
                    {
                        string path = null;

                        try
                        {
                            path = parametersArray[1];

                            if (!path.Contains(".xml", StringComparison.Ordinal))
                            {
                                throw new ArgumentException("Incorrect file name.");
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            return;
                        }

                        try
                        {
                            string answer = string.Empty;
                            if (!File.Exists(path))
                            {
                                using (FileStream stream = new FileStream(path, FileMode.Create))
                                {
                                    fileCabinetServiceShapshot.SaveToXml(stream);
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    Console.Write($"File is exist - rewrite {path}? [Y/n] ");
                                    answer = Console.ReadLine();
                                    if (answer.ToLower(new CultureInfo("en-US")) != "y" && answer.ToLower(new CultureInfo("en-US")) != "n")
                                    {
                                        Console.WriteLine("Incorrect answer.");
                                        continue;
                                    }

                                    break;
                                }
                            }

                            if (answer.Trim().ToLower(new CultureInfo("en-US")) == "y")
                            {
                                using (FileStream stream = new FileStream(path, FileMode.Create))
                                {
                                    fileCabinetServiceShapshot.SaveToXml(stream);
                                }
                            }
                            else if (answer.Trim().ToLower(new CultureInfo("en-US")) == "n")
                            {
                                return;
                            }
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Console.WriteLine($"Export failed: can't open file {path}");
                        }

                        Console.WriteLine($"All records are exported to file {path}");
                    }

                    break;
                default:
                    {
                        try
                        {
                            throw new ArgumentException("Incorrect file format.");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            return;
                        }
                    }
            }
        }
    }
}