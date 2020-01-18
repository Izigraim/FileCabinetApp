using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Class with default validation setting readed from validation-rules.json.
    /// </summary>
    public class DefaultValidationRules
    {
        /// <summary>
        /// Gets or sets first name settings.
        /// </summary>
        /// <value>
        /// First name settings.
        /// </value>
        public FirstNameSettingDefault FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name settings.
        /// </summary>
        /// <value>
        /// last name settings.
        /// </value>
        public LastNameSettingDefault LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth settings.
        /// </summary>
        /// <value>
        /// date of birth settings.
        /// </value>
        public DateOfBirthSettingDefault DateOfBirth { get; set; }
    }
}
