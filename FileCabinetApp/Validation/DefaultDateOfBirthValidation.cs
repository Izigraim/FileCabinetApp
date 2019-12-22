using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class DefaultDateOfBirthValidation : IRecordValidator
    {
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record.DateOfBirth < new DateTime(1950, 1, 1) || record.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Incorrect date.", nameof(record));
            }

            return true;
        }

        public FileCabinetRecord ValidateParametersProgram()
        {
            throw new NotImplementedException();
        }
    }
}
