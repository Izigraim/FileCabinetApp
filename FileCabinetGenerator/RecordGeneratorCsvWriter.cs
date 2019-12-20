using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    public class RecordGeneratorCsvWriter
    {
        private StreamWriter writer;

        public RecordGeneratorCsvWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void Write(IEnumerable<FileCabinetRecord> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            foreach (FileCabinetRecord record in list)
            {
                this.writer.WriteLine(record?.Id + "," + record.Sex + "," + record.FirstName + "," + record.LastName + "," + record.Age.ToString(new CultureInfo("en-US")) + "," + record.Salary.ToString() + "," + record.DateOfBirth.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
            }
        }
    }
}
