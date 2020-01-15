using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Validation;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Edit command.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public EditCommandHandler(IFIleCabinetService service)
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

            if (request.Command.ToLower(new CultureInfo("en-US")) == "update")
            {
                Update(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }


        private static void Update(string parameters)
        {
            parameters = parameters.ToLower(new CultureInfo("en-US"));

            if (!parameters.Contains("set", StringComparison.Ordinal) || !parameters.Contains("where", StringComparison.Ordinal))
            {
                Console.WriteLine("Incorrect command format.");
                return;
            }

            List<FileCabinetRecord> recordsToEdit = new List<FileCabinetRecord>();
            List<FileCabinetRecord> records = Service.GetRecords().ToList<FileCabinetRecord>();

            string[] parametersArray = parameters.Split(' ');
            string criteria = parameters.Substring(parameters.IndexOf("where", StringComparison.Ordinal) + 5, parameters.Length - parameters.IndexOf("where", StringComparison.Ordinal) - 5);
            string[] criteriaArray = criteria.Split("and");

            foreach (string parameter in criteriaArray)
            {
                string[] parameterArray = parameter.Trim(' ').Split('=');
                switch (parameterArray[0].ToLower(new CultureInfo("en-US")).Trim(' '))
                {
                    case "id":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();

                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();

                                }
                            }
                            else
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();

                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();

                                }
                            }
                        }

                        break;

                    case "firstname":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.FirstName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.FirstName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.FirstName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.FirstName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                }
                            }
                        }

                        break;

                    case "lastname":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.LastName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.LastName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.LastName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.LastName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                }
                            }
                        }

                        break;

                    case "dateofbirth":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                        }

                        break;

                    case "age":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                        }

                        break;

                    case "salary":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                        }

                        break;

                    case "sex":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToEdit.Count == 0)
                                {
                                    recordsToEdit = records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToEdit = recordsToEdit.Intersect<FileCabinetRecord>(records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
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

            string[] fields = parameters.Substring(parameters.IndexOf("set", StringComparison.Ordinal) + 4, parameters.IndexOf("where", StringComparison.Ordinal) - 4).Split('=', ',');

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].ToLower(new CultureInfo("en-US")).Trim(' ');
            }

            foreach (var recordEdited in recordsToEdit)
            {
                FileCabinetRecord record = recordEdited;

                for (int i = 0; i < fields.Length; i += 2)
                {
                    switch (fields[i].ToLower(new CultureInfo("en-US")).Trim(' '))
                    {
                        case "sex":
                            {
                                if (fields[i + 1][0] == '\'' && fields[i + 1][fields[i + 1].Length - 1] == '\'')
                                {
                                    record.Sex = Convert.ToChar(fields[i + 1].Trim(' ')[1..^1], new CultureInfo("en-US"));
                                }
                                else
                                {
                                    record.Sex = Convert.ToChar(fields[i + 1].Trim(' '), new CultureInfo("en-US"));
                                }
                            }

                            break;

                        case "firstname":
                            {
                                if (fields[i + 1][0] == '\'' && fields[i + 1][fields[i + 1].Length - 1] == '\'')
                                {
                                    record.FirstName = fields[i + 1].Trim(' ')[1..^1];
                                }
                                else
                                {
                                    record.FirstName = fields[i + 1].Trim(' ');
                                }
                            }

                            break;

                        case "lastname":
                            {
                                if (fields[i + 1][0] == '\'' && fields[i + 1][fields[i + 1].Length - 1] == '\'')
                                {
                                    record.LastName = fields[i + 1].Trim(' ')[1..^1];
                                }
                                else
                                {
                                    record.LastName = fields[i + 1].Trim(' ');
                                }
                            }

                            break;

                        case "age":
                            {
                                if (fields[i + 1][0] == '\'' && fields[i + 1][fields[i + 1].Length - 1] == '\'')
                                {
                                    record.Age = Convert.ToInt16(fields[i + 1].Trim(' ')[1..^1], new CultureInfo("en-US"));
                                }
                                else
                                {
                                    record.Age = Convert.ToInt16(fields[i + 1].Trim(' '), new CultureInfo("en-US"));
                                }
                            }

                            break;

                        case "salary":
                            {
                                if (fields[i + 1][0] == '\'' && fields[i + 1][fields[i + 1].Length - 1] == '\'')
                                {
                                    record.Salary = Convert.ToDecimal(fields[i + 1].Trim(' ')[1..^1], new CultureInfo("en-US"));
                                }
                                else
                                {
                                    record.Salary = Convert.ToDecimal(fields[i + 1].Trim(' '), new CultureInfo("en-US"));
                                }
                            }

                            break;

                        case "dateofbirth":
                            {
                                if (fields[i + 1][0] == '\'' && fields[i + 1][fields[i + 1].Length - 1] == '\'')
                                {
                                    record.DateOfBirth = DateTime.Parse(fields[i + 1].Trim(' ')[1..^1], new CultureInfo("en-US"));
                                }
                                else
                                {
                                    record.DateOfBirth = DateTime.Parse(fields[i + 1].Trim(' '), new CultureInfo("en-US"));
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

                if (!Program.Validator.ValidateParameters(record))
                {
                    Console.WriteLine("The field values do not match the validation settings.");
                    return;
                }
                else
                {
                    Service.EditRecord(record);
                }
            }

            if (recordsToEdit.Count == 0)
            {
                Console.WriteLine("Records with these parameters were not found.");
            }
            else if (recordsToEdit.Count == 1)
            {
                Console.WriteLine($"Record #{recordsToEdit[0].Id + 1} is updated.");
            }
            else
            {
                Console.Write("Records ");
                foreach (var record in recordsToEdit.OrderBy(c => c.Id))
                {
                    Console.Write($"#{record.Id + 1}");
                    if (recordsToEdit.Last() != record)
                    {
                        Console.Write($", ");
                    }
                }

                Console.WriteLine(" are updated.");
            }
        }
    }
}
