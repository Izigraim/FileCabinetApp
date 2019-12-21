using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// CsvWriter class.
    /// </summary>
    public class RecordGeneratorCsvWriter
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordGeneratorCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">StreamWriter.</param>
        public RecordGeneratorCsvWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write list of generated data to CSV file.
        /// </summary>
        /// <param name="list">List of generated data.</param>
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
