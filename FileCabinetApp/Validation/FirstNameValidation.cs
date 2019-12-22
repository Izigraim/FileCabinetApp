using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class FirstNameValidation : IRecordValidator
    {
        private int minLenght;
        private int maxLenght;

        public FirstNameValidation(int minLenght, int maxLenght)
        {
            this.minLenght = minLenght;
            this.maxLenght = maxLenght;
        }

        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record.FirstName == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.FirstName.Length > this.maxLenght || record.FirstName.Length < this.minLenght || record.FirstName.Contains(' ', StringComparison.Ordinal))
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
