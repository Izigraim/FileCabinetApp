using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class DefaultSexValidation : IRecordValidator
    {
        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record.Sex != 'w' && record.Sex != 'm')
            {
                throw new ArgumentException("Incorrect sex format.", nameof(record));
            }

            return true;
        }
    }
}
