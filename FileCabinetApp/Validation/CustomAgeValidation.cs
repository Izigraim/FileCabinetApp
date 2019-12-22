using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// CustomAgeValidation.
    /// </summary>
    public class CustomAgeValidation : IRecordValidator
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
