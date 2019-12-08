using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for custom validation.
    /// </summary>
    public static class FileCabinetCustomService
    {
        /// <summary>
        /// Custom validation for module.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <returns>If all correct - true, otherwise - false.</returns>
        public static bool ValidateParameters(FileCabinetRecord record)
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

                if (record.LastName == null)
                {
                    throw new ArgumentNullException(nameof(record));
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
        /// Custom validation for User's input.
        /// </summary>
        /// <returns>Recod.</returns>
        public static FileCabinetRecord ValidateParametersProgram()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            char sex;
            while (true)
            {
                try
                {
                    Console.Write("Sex(m/w): ");
                    sex = Convert.ToChar(Console.ReadLine(), culture);
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
