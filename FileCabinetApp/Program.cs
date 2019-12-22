using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// Class with main method.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Ilya Vrublevsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static IRecordValidator validator;
        private static IFIleCabinetService fileCabinetService;

        private static bool isRunning = true;

        /// <summary>
        /// Gets or sets type of validation.
        /// </summary>
        /// <value>
        /// Type of validation.
        /// </value>
        public static string ValidationType { get; set; } = "default";

        /// <summary>
        /// Gets or sets type of storage.
        /// </summary>
        /// <value>
        /// Type of storage.
        /// </value>
        public static string StorageType { get; set; } = "memory";

        /// <summary>
        /// Gets a validator.
        /// </summary>
        /// <value>
        /// A validator.
        /// </value>
        public static IRecordValidator Validator
        {
            get
            {
                return validator;
            }
        }

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
                validator = new ValidatorBuilder().CreateDefault(2, 60, 2, 60, 1950);
            }
            else
            {
                validator = new ValidatorBuilder().CreateCustom(0, 1000, 0, 10000, 0);
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

                var commandHandler = CreateCommandHandlers();
                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commandHandler.Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            IRecordPrinter recordPrinter = new DefaultRecordPrinter();

            var create = new CreateCommandHandler(fileCabinetService);
            var exit = new ExitCommandHandler(Exit);
            var stat = new StatCommanHandler(fileCabinetService);
            var help = new PrintHelpCommandHandler();
            var list = new ListCommandHandler(fileCabinetService, DefaultRecordPrint);
            var edit = new EditCommandHandler(fileCabinetService);
            var find = new FindCommandHandler(fileCabinetService, DefaultRecordPrint);
            var export = new ExportCommandHandler(fileCabinetService);
            var import = new ImportCommandHandler(fileCabinetService);
            var remove = new RemoveCommandHandler(fileCabinetService);
            var purge = new PurgeCommandHandler(fileCabinetService);

            create.SetNext(exit).SetNext(stat).SetNext(help).SetNext(list).SetNext(edit).SetNext(find).SetNext(export).SetNext(import).SetNext(remove).SetNext(purge);

            return create;
        }

        private static void Exit(bool running)
        {
            isRunning = running;
        }

        private static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> records)
        {
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id + 1}, {record.Sex}, {record.FirstName}, {record.LastName}, {record.Age}, {record.Salary}, {record.DateOfBirth:yyyy-MMM-dd}");
            }
        }
    }
}