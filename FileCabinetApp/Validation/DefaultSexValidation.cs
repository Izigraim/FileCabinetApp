using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// DefaultSex validation.
    /// </summary>
    public class DefaultSexValidation : IRecordValidator
    {
        /// <inheritdoc/>
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Sex != 'w' && record.Sex != 'm')
            {
                throw new ArgumentException("Incorrect sex format.", nameof(record));
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
