using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class DefautLastNameValidation : IRecordValidator
    {
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record.LastName == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.LastName.Length > 60 || record.LastName.Length < 2 || record.LastName.Contains(' ', StringComparison.Ordinal))
            {
                throw new ArgumentException("Incorrect last name format.", nameof(record));
            }

            return true;
        }

        public FileCabinetRecord ValidateParametersProgram()
        {
            throw new NotImplementedException();
        }
    }
}
