using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for default validation.
    /// </summary>
    public class DefaultValidation : IRecordValidator
    {
        /// <summary>
        /// Default validation for module.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <returns>If all correct - true, otherwise - false.</returns>
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            try
            {
                if (record.FirstName == null)
                {
                    throw new ArgumentNullException(nameof(record));
                }

                if (record.FirstName.Length > 60 || record.FirstName.Length < 2 || record.FirstName.Contains(' ', StringComparison.Ordinal))
                {
                    throw new ArgumentException("Incorrect first name format.", nameof(record));
                }

                if (record.LastName == null)
                {
                    throw new ArgumentNullException(nameof(record));
                }

                if (record.LastName.Length > 60 || record.LastName.Length < 2 || record.LastName.Contains(' ', StringComparison.Ordinal))
                {
                    throw new ArgumentException("Incorrect last name format.", nameof(record));
                }

                if (record.DateOfBirth < new DateTime(1950, 1, 1) || record.DateOfBirth > DateTime.Now)
                {
                    throw new ArgumentException("Incorrect date.", nameof(record));
                }

                if (record.Sex != 'w' && record.Sex != 'm')
                {
                    throw new ArgumentException("Incorrect sex format.", nameof(record));
                }

                if (record.Age > (DateTime.Now.Year - 1950) || record.Age < 0)
                {
                    throw new ArgumentException("Incorrect age format.", nameof(record));
                }

                if (record.Salary < 0)
                {
                    throw new ArgumentException("Incorrect 'salary' format.");
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validation for User's input.
        /// </summary>
        /// <returns>Record.</returns>
        public FileCabinetRecord ValidateParametersProgram()
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

            var record = new FileCabinetRecord
            {
                Sex = sex,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Salary = salary,
                DateOfBirth = dateTime,
            };

            return record;
        }
    }
}
