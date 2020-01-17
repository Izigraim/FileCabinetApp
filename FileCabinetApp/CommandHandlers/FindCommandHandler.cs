using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Find command.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        /// <param name="printer">Type of printing.</param>
        public FindCommandHandler(IFIleCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command.ToLower(new CultureInfo("en-US")) == "select")
            {
                try
                {
                    this.Select(request.Parameters);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect parameter(s) format.");
                }
            }
            else
            {
                base.Handle(request);
            }
        }

        private static List<FileCabinetRecord> CriteriaReader(string[] criteriaArray, string criteria, List<FileCabinetRecord> records, List<FileCabinetRecord> selectedRecords)
        {
            List<string> operators = new List<string>();
            foreach (string s in criteria.Split(' '))
            {
                if (s.Trim(' ').ToLower(new CultureInfo("en-US")) == "and" || s.Trim(' ').ToLower(new CultureInfo("en-US")) == "or")
                {
                    operators.Add(s.Trim(' ').ToLower(new CultureInfo("en-US")));
                }
            }

            int operatorNumber = 1;
            foreach (string parameter in criteriaArray)
            {
                string[] parameterArray = parameter.Trim(' ').Split('=');
                switch (parameterArray[0].ToLower(new CultureInfo("en-US")).Trim(' '))
                {
                    case "id":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                            else
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.Id + 1 == Convert.ToInt32(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                        }

                        break;

                    case "firstname":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.FirstName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.FirstName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.FirstName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                            else
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.FirstName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.FirstName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.FirstName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                        }

                        break;

                    case "lastname":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.LastName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.LastName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.LastName == parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                            else
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.LastName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' ')).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.LastName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.LastName == parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                        }

                        break;

                    case "dateofbirth":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                            else
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.DateOfBirth == DateTime.Parse(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                        }

                        break;

                    case "age":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                            else
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.Age == Convert.ToInt16(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                        }

                        break;

                    case "salary":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                            else
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.Salary == Convert.ToDecimal(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                        }

                        break;

                    case "sex":
                        {
                            if (parameterArray[1].Trim(' ')[0] == '\'' && parameterArray[1].Trim(' ')[parameterArray[1].Trim(' ').Length - 1] == '\'')
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                            else
                            {
                                if (selectedRecords.Count == 0)
                                {
                                    selectedRecords = records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US"))).ToList();
                                }
                                else if (operators[operatorNumber - 1] == "and")
                                {
                                    selectedRecords = selectedRecords.Intersect<FileCabinetRecord>(records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else if (operators[operatorNumber - 1] == "or")
                                {
                                    selectedRecords = selectedRecords.Union<FileCabinetRecord>(records.Where(c => c.Sex == Convert.ToChar(parameterArray[1].Trim(' ')[1..^1].ToLower(new CultureInfo("en-US")).Trim(' '), new CultureInfo("en-US")))).ToList();
                                    operatorNumber++;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect command format");
                                    return null;
                                }
                            }
                        }

                        break;

                    case "":
                        {
                            selectedRecords = records;
                        }

                        break;

                    default:
                        {
                            Console.WriteLine("Incorrect command format");
                            return null;
                        }
                }
            }

            return selectedRecords;
        }

        private static void PrintTable(string[] fields, List<FileCabinetRecord> selectedRecords)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string field in fields)
            {
                sb.Append("+");
                sb.Append(new string('-', MaxColumnWidth(selectedRecords, field) + 2));
            }

            sb.Append("+");
            sb.AppendLine();

            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append("|");
                sb.Append(" ");
                fields[i] = fields[i].PadRight(MaxColumnWidth(selectedRecords, fields[i]) + 1);
                sb.Append(fields[i]);
            }

            sb.Append("|");
            sb.AppendLine();

            foreach (string field in fields)
            {
                sb.Append("+");
                sb.Append(new string('-', MaxColumnWidth(selectedRecords, field) + 1));
            }

            sb.Append("+");
            sb.AppendLine();

            foreach (var record in selectedRecords)
            {
                foreach (string field in fields)
                {
                    sb.Append("|");
                    switch (field.Trim(' '))
                    {
                        case "Id":
                            {
                                string id = (record.Id + 1).ToString(new CultureInfo("en-US")).PadLeft(MaxColumnWidth(selectedRecords, field));
                                sb.Append(id);
                                sb.Append(" ");
                            }

                            break;

                        case "FirstName":
                            {
                                sb.Append(" ");
                                string firstname = record.FirstName.ToString(new CultureInfo("en-US")).PadRight(MaxColumnWidth(selectedRecords, field));
                                sb.Append(firstname);
                            }

                            break;

                        case "LastName":
                            {
                                sb.Append(" ");
                                string lastname = record.LastName.ToString(new CultureInfo("en-US")).PadRight(MaxColumnWidth(selectedRecords, field));
                                sb.Append(lastname);
                            }

                            break;

                        case "DateOfBirth":
                            {
                                string dateofbirth = record.DateOfBirth.ToString("MM/dd/yyyy", new CultureInfo("en-US")).PadLeft(MaxColumnWidth(selectedRecords, field));
                                sb.Append(dateofbirth);
                                sb.Append(" ");
                            }

                            break;

                        case "Age":
                            {
                                string age = record.Age.ToString(new CultureInfo("en-US")).PadLeft(MaxColumnWidth(selectedRecords, field));
                                sb.Append(age);
                                sb.Append(" ");
                            }

                            break;

                        case "Salary":
                            {
                                string salary = record.Salary.ToString(new CultureInfo("en-US")).PadLeft(MaxColumnWidth(selectedRecords, field));
                                sb.Append(salary);
                                sb.Append(" ");
                            }

                            break;

                        case "Sex":
                            {
                                sb.Append(" ");
                                string sex = record.Sex.ToString(new CultureInfo("en-US")).PadRight(MaxColumnWidth(selectedRecords, field));
                                sb.Append(sex);
                            }

                            break;
                    }
                }

                sb.Append("|");
                sb.AppendLine();
            }

            foreach (string field in fields)
            {
                sb.Append("+");
                sb.Append(new string('-', MaxColumnWidth(selectedRecords, field) + 1));
            }

            sb.Append("+");

            Console.WriteLine(sb);
        }

        private static int MaxColumnWidth(List<FileCabinetRecord> list, string field)
        {
            int maxWidth = field.Length;
            switch (field)
            {
                case "Id":
                    {
                        foreach (var record in list)
                        {
                            if (record.Id.ToString(new CultureInfo("en-US")).Length > maxWidth)
                            {
                                maxWidth = record.Id.ToString(new CultureInfo("en-US")).Length;
                            }
                        }
                    }

                    break;

                case "FirstName":
                    {
                        foreach (var record in list)
                        {
                            if (record.FirstName.Length > maxWidth)
                            {
                                maxWidth = record.FirstName.Length;
                            }
                        }
                    }

                    break;

                case "LastName":
                    {
                        foreach (var record in list)
                        {
                            if (record.LastName.Length > maxWidth)
                            {
                                maxWidth = record.LastName.Length;
                            }
                        }
                    }

                    break;

                case "Age":
                    {
                        foreach (var record in list)
                        {
                            if (record.Age.ToString(new CultureInfo("en-US")).Length > maxWidth)
                            {
                                maxWidth = record.Age.ToString(new CultureInfo("en-US")).Length;
                            }
                        }
                    }

                    break;

                case "Salary":
                    {
                        foreach (var record in list)
                        {
                            if (record.Salary.ToString(new CultureInfo("en-US")).Length > maxWidth)
                            {
                                maxWidth = record.Salary.ToString(new CultureInfo("en-US")).Length;
                            }
                        }
                    }

                    break;

                case "DateOfBirth":
                    {
                        foreach (var record in list)
                        {
                            if (record.DateOfBirth.ToString("MM/dd/yyyy", new CultureInfo("en-US")).Length > maxWidth)
                            {
                                maxWidth = record.DateOfBirth.ToString("MM/dd/yyyy", new CultureInfo("en-US")).Length;
                            }
                        }
                    }

                    break;

                case "Sex":
                    {
                        foreach (var record in list)
                        {
                            if (record.Sex.ToString(new CultureInfo("en-US")).Length > maxWidth)
                            {
                                maxWidth = record.Sex.ToString(new CultureInfo("en-US")).Length;
                            }
                        }
                    }

                    break;
            }

            return maxWidth;
        }

        private void Select(string parameters)
        {
            parameters = parameters.ToLower(new CultureInfo("en-US"));

            bool flagMemoization = false;

            List<FileCabinetRecord> selectedRecords;

            if (Service is FileCabinetMemoryService)
            {
                selectedRecords = Service.Memoization(parameters);
                if (selectedRecords.Count > 0)
                {
                    flagMemoization = true;
                }
            }
            else
            {
                selectedRecords = new List<FileCabinetRecord>();
            }

            List<FileCabinetRecord> records = Service.GetRecords().ToList();

            string criteria = string.Empty;
            string[] criteriaArray = new string[] { string.Empty };

            if (parameters.Contains("where", StringComparison.Ordinal))
            {
                criteria = parameters.Substring(parameters.IndexOf("where", StringComparison.Ordinal) + 5, parameters.Length - parameters.IndexOf("where", StringComparison.Ordinal) - 5);
                criteriaArray = criteria.Split(new string[] { "and", "or" }, StringSplitOptions.None);
            }

            string[] fields = new string[] { string.Empty };
            if (parameters.Contains("where", StringComparison.Ordinal))
            {
                fields = parameters.Substring(0, parameters.IndexOf("where", StringComparison.Ordinal)).Split(',');
            }
            else
            {
                fields = parameters.Split(',');
            }

            string allFields = "id sex firstname lastname age salary dateofbirth";
            if (string.IsNullOrEmpty(fields[0]))
            {
                fields = allFields.Split(' ');
            }

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].ToLower(new CultureInfo("en-US")).Trim(' ');

                if (fields[i] == "id")
                {
                    fields[i] = "Id";
                }
                else if (fields[i] == "firstname")
                {
                    fields[i] = "FirstName";
                }
                else if (fields[i] == "lastname")
                {
                    fields[i] = "LastName";
                }
                else if (fields[i] == "sex")
                {
                    fields[i] = "Sex";
                }
                else if (fields[i] == "salary")
                {
                    fields[i] = "Salary";
                }
                else if (fields[i] == "age")
                {
                    fields[i] = "Age";
                }
                else if (fields[i] == "dateofbirth")
                {
                    fields[i] = "DateOfBirth";
                }
                else
                {
                    Console.WriteLine("Incorrect field name.");
                    return;
                }
            }

            if (flagMemoization)
            {
                PrintTable(fields, selectedRecords);
                return;
            }

            if (string.IsNullOrEmpty(criteria))
            {
                selectedRecords = records;
            }
            else
            {
                selectedRecords = CriteriaReader(criteriaArray, criteria, records, selectedRecords);
            }

            if (selectedRecords.Count == 0)
            {
                Console.WriteLine("Records with the specified parameters were not found.");
                return;
            }

            if (Service is FileCabinetMemoryService)
            {
                Service.Memoization(parameters, selectedRecords);
            }

            PrintTable(fields, selectedRecords);
        }
    }
}
