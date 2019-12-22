using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// LastName validation.
    /// </summary>
    public class LastNameValidation : IRecordValidator
    {
        private int minLenght;
        private int maxLenght;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidation"/> class.
        /// </summary>
        /// <param name="minLenght">Min lenght.</param>
        /// <param name="maxLenght">Max lenght.</param>
        public LastNameValidation(int minLenght, int maxLenght)
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

            if (record.LastName == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.LastName.Length > this.maxLenght || record.LastName.Length < this.minLenght || record.LastName.Contains(' ', StringComparison.Ordinal))
            {
                throw new ArgumentException("Incorrect last name format.", nameof(record));
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
