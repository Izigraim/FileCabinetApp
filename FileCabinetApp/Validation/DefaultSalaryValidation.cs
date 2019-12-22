using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// DefaultSalary validation.
    /// </summary>
    public class DefaultSalaryValidation : IRecordValidator
    {
        /// <inheritdoc/>
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Salary < 0)
            {
                throw new ArgumentException("Incorrect 'salary' format.");
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
