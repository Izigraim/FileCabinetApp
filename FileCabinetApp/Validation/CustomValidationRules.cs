using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validation
{
    public class CustomValidationRules
    {
        public FirstNameSettingCustom FirstName { get; set; }

        public LastNameSettingCustom LastName { get; set; }

        public DateOfBirthSettingCustom DateOfBirth { get; set; }
    }

    public class FirstNameSettingCustom
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }

    public class LastNameSettingCustom
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }

    public class DateOfBirthSettingCustom
    {
        public int YearFrom { get; set; }
    }
}
