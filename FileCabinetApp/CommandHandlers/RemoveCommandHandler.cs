using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Remove command.
    /// </summary>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public RemoveCommandHandler(IFIleCabinetService service)
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

            if (request.Command.ToLower(new CultureInfo("en-US")) == "delete")
            {
                if (request.Parameters == "-h" || request.Parameters == "--help")
                {
                    Console.WriteLine("\t'delete' - deletes record(s) that match the search criteria.\n\tCommand format: delete where [fieldName]=[value] and [fieldName]=[value]\n\tSearch criteria support the 'and' operator for more accurate record search.");
                }
                else
                {
                    try
                    {
                        Delete(request.Parameters);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("No records with this ID");
                    }
                }
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Delete(string parameters)
        {
            string[] parametersString = parameters.ToLower(new CultureInfo("en-US")).Split(' ');

            if (!parametersString.Contains<string>("where"))
            {
                Console.WriteLine("Incorrect command format.");
                return;
            }

            string fieldsAndValues = parameters.Substring(parameters.IndexOf(' ', StringComparison.Ordinal), parameters.Length - parameters.IndexOf(' ', StringComparison.Ordinal));

            List<FileCabinetRecord> recordsToDelete = new List<FileCabinetRecord>();
            List<FileCabinetRecord> records = Service.GetRecords().ToList<FileCabinetRecord>();

            string[] fieldsAndValuesArray = fieldsAndValues.ToLower(new CultureInfo("en-US")).Split("and");
            foreach (string parameter in fieldsAndValuesArray)
            {
                string[] parameterArray = parameter.Trim(' ').Split('=');
                switch (parameterArray[0].ToLower(new CultureInfo("en-US")).Trim(' '))
                {
                    case "id":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                        }

                        break;

                    case "firstname":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.FirstName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.FirstName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.FirstName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.FirstName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                }
                            }
                        }

                        break;

                    case "lastname":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.LastName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.LastName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.LastName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.LastName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                }
                            }
                        }

                        break;

                    case "dateofbirth":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                        }

                        break;

                    case "age":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                        }

                        break;

                    case "salary":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                        }

                        break;

                    case "sex":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                }
                            }
                            else
                            {
                                if (recordsToDelete.Count == 0)
                                {
                                    recordsToDelete = records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else
                                {
                                    recordsToDelete = recordsToDelete.Intersect<FileCabinetRecord>(records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
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

            recordsToDelete = recordsToDelete.OrderByDescending(c => c.Id).ToList();

            foreach (var record in recordsToDelete)
            {
                Service.Remove(record.Id);
            }

            if (recordsToDelete.Count == 0)
            {
                Console.WriteLine("Records with these parameters were not found.");
            }
            else if (recordsToDelete.Count == 1)
            {
                Console.WriteLine($"Record #{recordsToDelete[0].Id + 1} is deleted.");
            }
            else
            {
                Console.Write("Records ");
                foreach (var record in recordsToDelete.OrderBy(c => c.Id))
                {
                    Console.Write($"#{record.Id + 1}");
                    if (recordsToDelete.OrderBy(c => c.Id).Last() != record)
                    {
                        Console.Write($", ");
                    }
                }

                Console.WriteLine(" are deleted.");
            }
        }
    }
}
