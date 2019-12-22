using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class LastNameValidation : IRecordValidator
    {
        private int minLenght;
        private int maxLenght;

        public LastNameValidation(int minLenght, int maxLenght)
        {
            this.minLenght = minLenght;
            this.maxLenght = maxLenght;
        }

        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record.LastName == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.LastName.Length > this.maxLenght || record.LastName.Length < this.minLenght || record.LastName.Contains(' ', StringComparison.Ordinal))
            {
                throw new ArgumentException("Incorrect last name format.", nameof(record));
            }

            return true;
        }
    }
}
