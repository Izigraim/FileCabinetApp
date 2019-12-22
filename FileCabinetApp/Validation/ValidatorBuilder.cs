using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// ValidatorBuiler.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBuilder"/> class.
        /// </summary>
        public ValidatorBuilder()
        {
            this.validators = new List<IRecordValidator>();
        }

        /// <summary>
        /// Add validator to list.
        /// </summary>
        /// <param name="min">Min lenght.</param>
        /// <param name="max">Max lenght.</param>
        /// <returns>Instance of current Builder.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidation(min, max));
            return this;
        }

        /// <summary>
        /// Add validator to list.
        /// </summary>
        /// <param name="min">Min lenght.</param>
        /// <param name="max">Max lenght.</param>
        /// <returns>Instance of current Builder.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidation(min, max));
            return this;
        }

        /// <summary>
        /// Add validiator to list.
        /// </summary>
        /// <param name="from">Start year.</param>
        /// <returns>Instance of current Builder.</returns>
        public ValidatorBuilder ValidateDateOfBirth(int from)
        {
            this.validators.Add(new DateOfBirthValidation(from));
            return this;
        }

        /// <summary>
        /// Create a validator.
        /// </summary>
        /// <returns>Validator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
