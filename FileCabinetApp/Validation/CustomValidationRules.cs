using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Class with custom validation setting readed from validation-rules.json.
    /// </summary>
    public class CustomValidationRules
    {
        /// <summary>
        /// Gets or sets first name settings.
        /// </summary>
        /// <value>
        /// First name settings.
        /// </value>
        public FirstNameSettingCustom FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name settings.
        /// </summary>
        /// <value>
        /// last name settings.
        /// </value>
        public LastNameSettingCustom LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth settings.
        /// </summary>
        /// <value>
        /// date of birth settings.
        /// </value>
        public DateOfBirthSettingCustom DateOfBirth { get; set; }
    }

    /// <summary>
    /// First name validation settings.
    /// </summary>
    public class FirstNameSettingCustom
    {
        /// <summary>
        /// Gets or sets minimum lenght.
        /// </summary>
        /// <value>
        /// Minimum lenght.
        /// </value>
        public int Min { get; set; }

        /// <summary>
        /// Gets or sets maximum lenght.
        /// </summary>
        /// <value>
        /// Maximum lenght.
        /// </value>
        public int Max { get; set; }
    }

    /// <summary>
    /// Last name validation settings.
    /// </summary>
    public class LastNameSettingCustom
    {
        /// <summary>
        /// Gets or sets minimum lenght.
        /// </summary>
        /// <value>
        /// Minimum lenght.
        /// </value>
        public int Min { get; set; }

        /// <summary>
        /// Gets or sets maximum lenght.
        /// </summary>
        /// <value>
        /// Maximum lenght.
        /// </value>
        public int Max { get; set; }
    }

    /// <summary>
    /// Date of birth validation settings.
    /// </summary>
    public class DateOfBirthSettingCustom
    {
        /// <summary>
        /// Gets or sets minimum year.
        /// </summary>
        /// <value>
        /// Minimum year.
        /// </value>
        public int YearFrom { get; set; }
    }
}
