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
}
