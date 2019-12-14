using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// CsvWriter class.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream.</param>
        public FileCabinetRecordCsvWriter(StreamWriter writer)
        {
            this.writer = writer;
            writer.WriteLine("Id,Sex,FirstName,LastName,Age,Salary,DateOfBirth");
        }

        /// <summary>
        /// Write one record to file.
        /// </summary>
        /// <param name="record">Record.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine((record?.Id + 1) + "," + record.Sex + "," + record.FirstName + "," + record.LastName + "," + record.Age.ToString(new CultureInfo("en-US")) + "," + record.Salary.ToString() + "," + record.DateOfBirth.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
        }
    }
}
