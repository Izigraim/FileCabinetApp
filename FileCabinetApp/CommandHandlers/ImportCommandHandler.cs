using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ImportCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "import")
            {
                Import(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Import(string parameters)
        {
            try
            {
                if (parameters == null || parameters.Trim(' ').Where(c => c == ' ').Count() != 1)
                {
                    throw new ArgumentException("Incorrect parameters.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            string[] parametersArray = parameters.Split(' ');

            try
            {
                if (parametersArray[0].ToLower(new CultureInfo("en-US")) != "csv" && parametersArray[0].ToLower(new CultureInfo("en-US")) != "xml")
                {
                    throw new ArgumentException("Incorrect file format.");
                }

                if (!File.Exists(parametersArray[1]))
                {
                    throw new ArgumentException($"Import error: file {parametersArray[1]} is not exist.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            switch (parametersArray[0].ToLower(new CultureInfo("en-US")))
            {
                case "csv":
                    {
                        var snapshot = Program.fileCabinetService.MakeSnapshot();
                        using StreamReader reader = new StreamReader(File.Open(parametersArray[1], FileMode.Open));
                        snapshot.LoadFromCsv(reader);
                        Program.fileCabinetService.Restore(snapshot);
                        Console.WriteLine($"{snapshot.Records.Count} records were imported from {parametersArray[1]}");
                    }

                    break;

                case "xml":
                    {
                        var snapshot = Program.fileCabinetService.MakeSnapshot();
                        using StreamReader reader = new StreamReader(File.Open(parametersArray[1], FileMode.Open));
                        snapshot.LoadFromXml(reader);
                        Program.fileCabinetService.Restore(snapshot);
                        Console.WriteLine($"{snapshot.Records.Count} records were imported from {parametersArray[1]}");
                    }

                    break;
            }
        }
    }
}
