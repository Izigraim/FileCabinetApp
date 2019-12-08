using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Validation interface.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validation for module.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <returns>If all correct - true, otherwise - false.</returns>
        public bool ValidateParameters(FileCabinetRecord record);

        /// <summary>
        /// Validation for User's input.
        /// </summary>
        /// <returns>Record.</returns>
        public FileCabinetRecord ValidateParametersProgram();
    }
}
