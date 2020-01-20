using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Import command.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public ImportCommandHandler(IFIleCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command.ToLower(new CultureInfo("en-US")) == "import")
            {
                if (request.Parameters == "-h" || request.Parameters == "--help")
                {
                    Console.WriteLine("\t'import' - imports data from a file with specified format.\n\tCommand format: import [csv/xml] [filePath].[csv/xml]");
                }
                else
                {
                    Import(request.Parameters);
                }
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
                        var snapshot = Service.MakeSnapshot();
                        using StreamReader reader = new StreamReader(File.Open(parametersArray[1], FileMode.Open));
                        snapshot.LoadFromCsv(reader);
                        Service.Restore(snapshot);
                        Console.WriteLine($"{snapshot.Records.Count} records were imported from {parametersArray[1]}");
                    }

                    break;

                case "xml":
                    {
                        var snapshot = Service.MakeSnapshot();
                        using StreamReader reader = new StreamReader(File.Open(parametersArray[1], FileMode.Open));
                        snapshot.LoadFromXml(reader);
                        Service.Restore(snapshot);
                        Console.WriteLine($"{snapshot.Records.Count} records were imported from {parametersArray[1]}");
                    }

                    break;
            }
        }
    }
}
