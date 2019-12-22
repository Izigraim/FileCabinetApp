using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class DateOfBirthValidation : IRecordValidator
    {
        private int yearFrom;

        public DateOfBirthValidation(int yearFrom)
        {
            this.yearFrom = yearFrom;
        }

        public bool ValidateParameters(FileCabinetRecord record)
        {
            if (record.DateOfBirth.Year < this.yearFrom || record.DateOfBirth > DateTime.Now)
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
