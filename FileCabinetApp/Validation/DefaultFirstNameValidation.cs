using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class DefaultFirstNameValidation : IRecordValidator
    {
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record.FirstName == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.FirstName.Length > 60 || record.FirstName.Length < 2 || record.FirstName.Contains(' ', StringComparison.Ordinal))
            {
                throw new ArgumentException("Incorrect first name format.", nameof(record));
            }

            return true;
        }

        public FileCabinetRecord ValidateParametersProgram()
        {
            throw new NotImplementedException();
        }
    }
}
