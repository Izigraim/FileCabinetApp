using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// DateOfBirth validation.
    /// </summary>
    public class DateOfBirthValidation : IRecordValidator
    {
        private int yearFrom;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidation"/> class.
        /// </summary>
        /// <param name="yearFrom">Start of years.</param>
        public DateOfBirthValidation(int yearFrom)
        {
            this.yearFrom = yearFrom;
        }

        /// <inheritdoc/>
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.DateOfBirth.Year < this.yearFrom || record.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Incorrect date.", nameof(record));
            }

            return true;
        }

        /// <inheritdoc/>
        public FileCabinetRecord ValidateParametersProgram()
        {
            throw new NotImplementedException();
        }
    }
}
