using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Extension to ValidatorBuilder.
    /// </summary>
    public static class ValidatorBuilderExtension
    {
        /// <summary>
        /// Create ValidatorBuilder with default parameters.
        /// </summary>
        /// <param name="builder">ValidatorBuilder.</param>
        /// <param name="minFirstName">Min lenght of FirtsName.</param>
        /// <param name="maxFirstName">Max lenght of FirstName.</param>
        /// <param name="minLastName">Min lenght of LastName.</param>
        /// <param name="maxLastName">Max lenght of LastName.</param>
        /// <param name="from">Start year.</param>
        /// <returns>Validator.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder, int minFirstName, int maxFirstName, int minLastName, int maxLastName, int from)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.ValidateFirstName(minFirstName, maxFirstName).ValidateLastName(minLastName, maxLastName).ValidateDateOfBirth(from).Create();
        }

        /// <summary>
        /// Create ValidatorBuilder with custom parameters.
        /// </summary>
        /// <param name="builder">ValidatorBuilder.</param>
        /// <param name="minFirstName">Min lenght of FirtsName.</param>
        /// <param name="maxFirstName">Max lenght of FirstName.</param>
        /// <param name="minLastName">Min lenght of LastName.</param>
        /// <param name="maxLastName">Max lenght of LastName.</param>
        /// <param name="from">Start year.</param>
        /// <returns>Validator.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder, int minFirstName, int maxFirstName, int minLastName, int maxLastName, int from)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.ValidateFirstName(minFirstName, maxFirstName).ValidateLastName(minLastName, maxLastName).ValidateDateOfBirth(from).Create();
        }
    }
}
