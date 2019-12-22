using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class CustomDateOfBirthValidation : IRecordValidator
    {
        public bool ValidateParameters(FileCabinetRecord record)
        {
            return true;
        }

        public FileCabinetRecord ValidateParametersProgram()
        {
            throw new NotImplementedException();
        }
    }
}
