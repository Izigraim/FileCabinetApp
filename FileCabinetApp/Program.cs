﻿using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Ilya Vrublevsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly FileCabinetService FileCabinetService = new FileCabinetService();

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
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
            var recordsCount = Program.FileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            char sex;
            while (true)
            {
                try
                {
                    Console.Write("Sex(m/w): ");
                    sex = Convert.ToChar(Console.ReadLine(), culture);

                    if (sex != 'w' && sex != 'm')
                    {
                        throw new ArgumentException("Incorrect sex format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Write only one symbol.");
                }
            }

            string firstName;
            while (true)
            {
                try
                {
                    Console.Write("First name: ");
                    firstName = Console.ReadLine().Trim(' ');

                    if (firstName.Length > 60 || firstName.Length < 2 || firstName.Contains(' ', StringComparison.Ordinal))
                    {
                        throw new ArgumentException("Incorrect first name format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            string lastName;
            while (true)
            {
                try
                {
                    Console.Write("Last name: ");
                    lastName = Console.ReadLine().Trim(' ');

                    if (lastName.Length > 60 || lastName.Length < 2 || lastName.Contains(' ', StringComparison.Ordinal))
                    {
                        throw new ArgumentException("Incorrect last name format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            short age;
            while (true)
            {
                try
                {
                    Console.Write("Age: ");
                    age = Convert.ToInt16(Console.ReadLine(), culture);

                    if (age > (DateTime.Now.Year - 1950) || age < 0)
                    {
                        throw new ArgumentException("Incorrect age format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect age symbols.");
                }
            }

            decimal salary;
            while (true)
            {
                try
                {
                    Console.Write("Salary: ");
                    salary = Convert.ToDecimal(Console.ReadLine(), culture);
                    if (salary < 0)
                    {
                        throw new ArgumentException("Incorrect 'salary' format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect salary symbols.");
                }
            }

            DateTime dateTime;
            while (true)
            {
                try
                {
                    Console.Write("Date of birth: ");
                    dateTime = DateTime.Parse(Console.ReadLine(), culture);
                    if (dateTime < new DateTime(1950, 1, 1) || dateTime > DateTime.Now)
                    {
                        throw new ArgumentException("Incorrect date.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect DateTime symbols.");
                }
            }

            if (Program.FileCabinetService.CreateRecord(sex, firstName, lastName, age, salary, dateTime) == -1)
            {
                Console.WriteLine("An error occured creating the record.");
            }
            else
            {
                var recordsCount = Program.FileCabinetService.GetStat();
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
                if (id > Program.FileCabinetService.GetStat() || id <= 0)
                {
                    Console.WriteLine($"#{id} record is not found.");
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Incorrext formar of ID");
                return;
            }

            char sex;
            while (true)
            {
                try
                {
                    Console.Write("Sex(m/w): ");
                    sex = Convert.ToChar(Console.ReadLine(), culture);

                    if (sex != 'w' && sex != 'm')
                    {
                        throw new ArgumentException("Incorrect sex format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Write only one symbol.");
                }
            }

            string firstName;
            while (true)
            {
                try
                {
                    Console.Write("First name: ");
                    firstName = Console.ReadLine().Trim(' ');

                    if (firstName.Length > 60 || firstName.Length < 2 || firstName.Contains(' ', StringComparison.Ordinal))
                    {
                        throw new ArgumentException("Incorrect first name format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            string lastName;
            while (true)
            {
                try
                {
                    Console.Write("Last name: ");
                    lastName = Console.ReadLine().Trim(' ');

                    if (lastName.Length > 60 || lastName.Length < 2 || lastName.Contains(' ', StringComparison.Ordinal))
                    {
                        throw new ArgumentException("Incorrect last name format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            short age;
            while (true)
            {
                try
                {
                    Console.Write("Age: ");
                    age = Convert.ToInt16(Console.ReadLine(), culture);

                    if (age > (DateTime.Now.Year - 1950) || age < 0)
                    {
                        throw new ArgumentException("Incorrect age format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect age symbols.");
                }
            }

            decimal salary;
            while (true)
            {
                try
                {
                    Console.Write("Salary: ");
                    salary = Convert.ToDecimal(Console.ReadLine(), culture);
                    if (salary < 0)
                    {
                        throw new ArgumentException("Incorrect 'salary' format.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect salary symbols.");
                }
            }

            DateTime dateTime;
            while (true)
            {
                try
                {
                    Console.Write("Date of birth: ");
                    dateTime = DateTime.Parse(Console.ReadLine(), culture);
                    if (dateTime < new DateTime(1950, 1, 1) || dateTime > DateTime.Now)
                    {
                        throw new ArgumentException("Incorrect date.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect DateTime symbols.");
                }
            }

            Program.FileCabinetService.EditRecord(id - 1, sex, firstName, lastName, age, salary, dateTime);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] fileCabinetRecords = Program.FileCabinetService.GetRecords();
            for (int i = 0; i < fileCabinetRecords.Length; i++)
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

            FileCabinetRecord[] findedRecords = Array.Empty<FileCabinetRecord>();

            switch (findParameters[0].ToLower(CultureInfo.CreateSpecificCulture("en-US")))
            {
                case "firstname":
                    findedRecords = FileCabinetService.FindByFirstName(findParameters[1].Trim('"'));
                    break;
                case "lastname":
                    findedRecords = FileCabinetService.FindByLastName(findParameters[1].Trim('"'));
                    break;
                case "dateofbirth":
                    findedRecords = FileCabinetService.FindByDateOfBirth(findParameters[1].Trim('"'));
                    break;
            }

            if (findedRecords == Array.Empty<FileCabinetRecord>())
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
    }
}