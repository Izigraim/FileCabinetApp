using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// CustomSalatyValidation.
    /// </summary>
    public class CustomSalaryValidation : IRecordValidator
    {
        /// <inheritdoc/>
        public bool ValidateParameters(FileCabinetRecord record)
        {
            return true;
        }

        /// <inheritdoc/>
        public FileCabinetRecord ValidateParametersProgram()
        {
            throw new NotImplementedException();
        }
    }
}
