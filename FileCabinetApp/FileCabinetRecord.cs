using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public char Sex { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public short Age { get; set; }

        public decimal? Salary { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
