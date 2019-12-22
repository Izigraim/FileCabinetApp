using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// FIrstName validation.
    /// </summary>
    public class FirstNameValidation : IRecordValidator
    {
        private int minLenght;
        private int maxLenght;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidation"/> class.
        /// </summary>
        /// <param name="minLenght">Min lenght.</param>
        /// <param name="maxLenght">Max lenght.</param>
        public FirstNameValidation(int minLenght, int maxLenght)
        {
            this.minLenght = minLenght;
            this.maxLenght = maxLenght;
        }

        /// <inheritdoc/>
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.FirstName == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.FirstName.Length > this.maxLenght || record.FirstName.Length < this.minLenght || record.FirstName.Contains(' ', StringComparison.Ordinal))
            {
                throw new ArgumentException("Incorrect first name format.", nameof(record));
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
