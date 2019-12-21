using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// CsvReader class.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">StreamReader.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Read all records from CSV file.
        /// </summary>
        /// <returns>List of readed records.</returns>
        public List<FileCabinetRecord> ReadAll()
        {
            string line;
            List<FileCabinetRecord> list = new List<FileCabinetRecord>();
            while ((line = this.reader.ReadLine()) != null)
            {
                FileCabinetRecord record = new FileCabinetRecord();

                string[] lineArray = line.Split(',');
                record.Id = Convert.ToInt32(lineArray[0], new CultureInfo("en-US"));
                record.Sex = Convert.ToChar(lineArray[1], new CultureInfo("en-US"));
                record.FirstName = lineArray[2];
                record.LastName = lineArray[3];
                record.Age = Convert.ToInt16(lineArray[4], new CultureInfo("en-US"));
                record.Salary = Convert.ToDecimal(lineArray[5] + "." + lineArray[6], new CultureInfo("en-US"));
                record.DateOfBirth = DateTime.Parse(lineArray[7], new CultureInfo("en-US"));

                list.Add(record);
            }

            return list;
        }
    }
}
