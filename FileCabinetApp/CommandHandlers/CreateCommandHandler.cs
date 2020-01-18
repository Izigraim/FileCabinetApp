using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Validation;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Create command class.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public CreateCommandHandler(IFIleCabinetService service)
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

            if (request.Command.ToLower(new CultureInfo("en-US")) == "create")
            {
                if (request.Parameters == "-h" || request.Parameters == "--help")
                {
                    Console.WriteLine("\t'create' - creates a new record with the values entered from the keybord.\n\tDoes not require any additional parameters.");
                }
                else if (string.IsNullOrEmpty(request.Parameters))
                {
                    Create();
                }
                else
                {
                    Console.WriteLine("This command does not accept parameters.");
                }
            }
            else if (request.Command.ToLower(new CultureInfo("en-US")) == "insert")
            {
                if (request.Parameters == "-h" || request.Parameters == "--help")
                {
                    Console.WriteLine("\t'insert' - create a record with fields and values in one line.\n\tCommand format: insert ([field1],[field2]...[fieldN]) values ([value1],[value2]...[valueN]).\n\tIf a recod with the specified ID exists, it can be overwritten.");
                }
                else
                {
                    try
                    {
                        Insert(request.Parameters);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Incorrect command format.");
                    }
                }
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Create()
        {
            FileCabinetRecord record = Program.Validator.ValidateParametersProgram();
            record.Id = Service.GetStat(out int deletedCount);

            if (Service.CreateRecord(record) == -1)
            {
                Console.WriteLine("An error occured creating the record.");
            }
            else
            {
                var recordsCount = Service.GetStat(out deletedCount);
                Console.WriteLine($"Record #{recordsCount} created.");
            }
        }

        private static void Insert(string parameters)
        {
            if (parameters.Where(c => c == '(').Count() != 2 || parameters.Where(c => c == ')').Count() != 2)
            {
                Console.WriteLine("Incorrect command format.");
                return;
            }

            string[] parametersString = parameters.ToLower(new CultureInfo("en-US")).Split(' ');

            if (!parametersString.Contains<string>("values"))
            {
                Console.WriteLine("Incorrect command format.");
                return;
            }

            string[] fields = parameters.Substring(parameters.IndexOf('(', StringComparison.Ordinal) + 1, parameters.IndexOf(')', StringComparison.Ordinal) - 1).Split(',');
            string[] values = parameters.Substring(parameters.LastIndexOf('(') + 1, parameters.Length - parameters.LastIndexOf('(') - 2).Split(',');

            if (fields.Length != 7 && values.Length != 7)
            {
                Console.WriteLine("Incorrect command format.");
                return;
            }

            FileCabinetRecord record = new FileCabinetRecord();

            for (int i = 0; i < fields.Length; i++)
            {
                switch (fields[i].ToLower(new CultureInfo("en-US")).Trim(' '))
                {
                    case "id":
                        {
                            if (values[i][0] == '\'' && values[i][values[i].Length - 1] == '\'')
                            {
                                record.Id = Convert.ToInt32(values[i].Trim(' ')[1..^1], new CultureInfo("en-US"));
                            }
                            else
                            {
                                record.Id = Convert.ToInt32(values[i].Trim(' '), new CultureInfo("en-US"));
                            }

                            record.Id--;
                            if (record.Id == -1)
                            {
                                record.Id = 0;
                            }
                        }

                        break;

                    case "sex":
                        {
                            if (values[i][0] == '\'' && values[i][values[i].Length - 1] == '\'')
                            {
                                record.Sex = Convert.ToChar(values[i].Trim(' ')[1..^1], new CultureInfo("en-US"));
                            }
                            else
                            {
                                record.Sex = Convert.ToChar(values[i].Trim(' '), new CultureInfo("en-US"));
                            }
                        }

                        break;

                    case "firstname":
                        {
                            if (values[i][0] == '\'' && values[i][values[i].Length - 1] == '\'')
                            {
                                record.FirstName = values[i].Trim(' ')[1..^1];
                            }
                            else
                            {
                                record.FirstName = values[i].Trim(' ');
                            }
                        }

                        break;

                    case "lastname":
                        {
                            if (values[i][0] == '\'' && values[i][values[i].Length - 1] == '\'')
                            {
                                record.LastName = values[i].Trim(' ')[1..^1];
                            }
                            else
                            {
                                record.LastName = values[i].Trim(' ');
                            }
                        }

                        break;

                    case "age":
                        {
                            if (values[i][0] == '\'' && values[i][values[i].Length - 1] == '\'')
                            {
                                record.Age = Convert.ToInt16(values[i].Trim(' ')[1..^1], new CultureInfo("en-US"));
                            }
                            else
                            {
                                record.Age = Convert.ToInt16(values[i].Trim(' '), new CultureInfo("en-US"));
                            }
                        }

                        break;

                    case "salary":
                        {
                            if (values[i][0] == '\'' && values[i][values[i].Length - 1] == '\'')
                            {
                                record.Salary = Convert.ToDecimal(values[i].Trim(' ')[1..^1], new CultureInfo("en-US"));
                            }
                            else
                            {
                                record.Salary = Convert.ToDecimal(values[i].Trim(' '), new CultureInfo("en-US"));
                            }
                        }

                        break;

                    case "dateofbirth":
                        {
                            if (values[i][0] == '\'' && values[i][values[i].Length - 1] == '\'')
                            {
                                record.DateOfBirth = DateTime.Parse(values[i].Trim(' ')[1..^1], new CultureInfo("en-US"));
                            }
                            else
                            {
                                record.DateOfBirth = DateTime.Parse(values[i].Trim(' '), new CultureInfo("en-US"));
                            }
                        }

                        break;
                    default:
                        {
                            Console.WriteLine("Incorrect command format");
                            return;
                        }
                }
            }

            if (Service.GetRecords().Where(c => c.Id == record.Id).Any())
            {
                string[] answers = { "yes", "y", "+", "да", "д" };
                Console.WriteLine("A record with this ID already exists. Want to rewrite it?");
                string answer = Console.ReadLine();
                if (!answers.Contains<string>(answer))
                {
                    Console.WriteLine("A record will not be added.");
                    return;
                }
            }

            if (Program.Validator.ValidateParameters(record))
            {
                Service.CreateRecord(record);
                var recordsCount = Service.GetStat(out int deletedCount);
                Console.WriteLine($"Record #{recordsCount} created.");
            }
            else
            {
                Console.WriteLine("The field values do not match the validation settings.");
            }
        }
    }
}
