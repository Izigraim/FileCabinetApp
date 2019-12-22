using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Class for default validation.
    /// </summary>
    public class DefaultValidation : CompositeValidator
    {
        public DefaultValidation()
            : base(new List<IRecordValidator> {
            new FirstNameValidation(2, 60),
            new LastNameValidation(2, 60),
            new DateOfBirthValidation(1950),
            })
        {
        }

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
                new DefaultSexValidation().ValidateParameters(record);

                new DefaultAgeValidation().ValidateParameters(record);

                new DefaultSalaryValidation().ValidateParameters(record);
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

            Console.Write("Sex(m/w): ");
            char sex = this.ReadInput<char>(this.CharConverter, this.SexValidator);

            Console.Write("First name: ");
            string firstName = this.ReadInput<string>(this.StringConverter, this.FirstNameValidator);

            Console.Write("Last name: ");
            string lastName = this.ReadInput<string>(this.StringConverter, this.LastNameValidator);

            Console.Write("Age: ");
            short age = Convert.ToInt16(this.ReadInput<string>(this.ShortConverter, this.AgeValidation), new CultureInfo("en-US"));

            Console.Write("Salary: ");
            decimal salary = Convert.ToDecimal(this.ReadInput<string>(this.DecimalConverter, this.SalaryValidation), new CultureInfo("en-US"));

            Console.Write("Date of Birth: ");
            DateTime dateOfBirth = Convert.ToDateTime(this.ReadInput<string>(this.DateTimeConverter, this.DateOfBirthValidator), new CultureInfo("en-US"));

            var record = new FileCabinetRecord
            {
                Sex = sex,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Salary = salary,
                DateOfBirth = dateOfBirth,
            };

            return record;
        }

        private Tuple<bool, string, string> DateTimeConverter(string valueString)
        {
            DateTime value;

            try
            {
                value = Convert.ToDateTime(valueString, new CultureInfo("en-US"));
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                return (false, " ", valueString).ToTuple();
            }

            return (true, " ", valueString).ToTuple();
        }

        private Tuple<bool, string> DateOfBirthValidator(string dateOfBirthString)
        {
            DateTime dateTime = Convert.ToDateTime(dateOfBirthString, new CultureInfo("en-US"));

            try
            {
                if (dateTime < new DateTime(1950, 1, 1) || dateTime > DateTime.Now)
                {
                    throw new ArgumentException("Incorrect date.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return (false, dateOfBirthString).ToTuple();
            }

            return (true, dateOfBirthString).ToTuple();
        }

        private Tuple<bool, string, string> DecimalConverter(string valueString)
        {
            decimal value;

            try
            {
                value = Convert.ToDecimal(valueString, new CultureInfo("en-US"));
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                return (false, " ", valueString).ToTuple();
            }

            return (true, " ", valueString).ToTuple();
        }

        private Tuple<bool, string> SalaryValidation(string salaryString)
        {
            decimal salary = Convert.ToDecimal(salaryString, new CultureInfo("en-US"));
            try
            {
                if (salary < 0)
                {
                    throw new ArgumentException("Incorrect 'salary' format.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return (false, salaryString).ToTuple();
            }

            return (true, salaryString).ToTuple();
        }

        private Tuple<bool, string, string> ShortConverter(string valueString)
        {
            short value;
            try
            {
                value = Convert.ToInt16(valueString, new CultureInfo("en-US"));

                if (value > 32767 || value < -32768)
                {
                    throw new FormatException(nameof(value));
                }
            }
            catch (FormatException)
            {
               return (false, " ", valueString).ToTuple();
            }

            return (true, " ", valueString).ToTuple();
        }

        private Tuple<bool, string> AgeValidation(string ageString)
        {
            short age = Convert.ToInt16(ageString, new CultureInfo("en-US"));
            try
            {
                if (age > (DateTime.Now.Year - 1950) || age < 0)
                {
                    throw new ArgumentException("Incorrect age format.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return (false, age.ToString(new CultureInfo("en-US"))).ToTuple();
            }

            return (true, age.ToString(new CultureInfo("en-US"))).ToTuple();
        }

        private Tuple<bool, string, string> StringConverter(string value)
        {
            return (true, " ", value).ToTuple();
        }

        private Tuple<bool, string> FirstNameValidator(string firstName)
        {
            try
            {
                if (firstName.Length > 60 || firstName.Length < 2 || firstName.Contains(' ', StringComparison.Ordinal))
                {
                    throw new ArgumentException("Incorrect first name format.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return (false, firstName).ToTuple();
            }

            return (true, firstName).ToTuple();
        }

        private Tuple<bool, string> LastNameValidator(string lastName)
        {
            try
            {
                if (lastName.Length > 60 || lastName.Length < 2 || lastName.Contains(' ', StringComparison.Ordinal))
                {
                    throw new ArgumentException("Incorrect last name format.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return (false, lastName).ToTuple();
            }

            return (true, lastName).ToTuple();
        }

        private Tuple<bool, string, char> CharConverter(string value)
        {
            char[] valueChar;
            try
            {
                valueChar = value.ToCharArray();
            }
            catch (FormatException)
            {
                return (false, " ", ' ').ToTuple();
            }

            return (true, " ", valueChar[0]).ToTuple();
        }

        private Tuple<bool, string> SexValidator(char sex)
        {
            try
            {
                if (sex != 'w' && sex != 'm')
                {
                    throw new ArgumentException("Incorrect sex format.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return (false, " ").ToTuple();
            }

            return (true, sex.ToString(new CultureInfo("en-US"))).ToTuple();
        }

        private T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
