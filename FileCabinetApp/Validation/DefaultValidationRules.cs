using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation
{
    public class DefaultValidationRules
    {
        public FirstNameSettingDefault FirstName { get; set; }

        public LastNameSettingDefault LastName { get; set; }

        public DateOfBirthSettingDefault DateOfBirth { get; set; }
    }

    public class FirstNameSettingDefault
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }

    public class LastNameSettingDefault
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }

    public class DateOfBirthSettingDefault
    {
        public int YearFrom { get; set; }
    }
}
