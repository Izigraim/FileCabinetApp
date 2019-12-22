using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        public CompositeValidator(List<IRecordValidator> validators)
        {
            this.validators = validators;
        }

        public bool ValidateParameters(FileCabinetRecord record)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(record);
            }

            return true;
        }
    }
}
