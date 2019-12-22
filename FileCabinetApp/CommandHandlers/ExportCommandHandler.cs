using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        public ExportCommandHandler(IFIleCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "export")
            {
                Export(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
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

            var fileCabinetServiceShapshot = service.MakeSnapshot();

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
