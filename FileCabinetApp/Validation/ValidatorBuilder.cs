using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators;

        public ValidatorBuilder()
        {
            this.validators = new List<IRecordValidator>();
        }

        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidation(min, max));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidation(min, max));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(int from)
        {
            this.validators.Add(new DateOfBirthValidation(from));
            return this;
        }

        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
