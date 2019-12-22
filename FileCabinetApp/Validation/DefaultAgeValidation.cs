using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// DefaultAge validation.
    /// </summary>
    public class DefaultAgeValidation : IRecordValidator
    {
        /// <inheritdoc/>
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Age > (DateTime.Now.Year - 1950) || record.Age < 0)
            {
                throw new ArgumentException("Incorrect age format.", nameof(record));
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
