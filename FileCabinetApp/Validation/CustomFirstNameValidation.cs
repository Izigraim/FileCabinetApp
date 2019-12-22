using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class CustomFirstNameValidation : IRecordValidator
    {
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record.FirstName == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            return true;
        }

        public FileCabinetRecord ValidateParametersProgram()
        {
            throw new NotImplementedException();
        }
    }
}
