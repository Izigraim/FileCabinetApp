using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class CustomSalaryValidation : IRecordValidator
    {
        public bool ValidateParameters(FileCabinetRecord record)
        {
            return true;
        }
    }
}
