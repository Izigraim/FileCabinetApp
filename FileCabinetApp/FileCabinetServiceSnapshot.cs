using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Shapshot class.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Array of records.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Save records to .csv file.
        /// </summary>
        /// <param name="stream">Stream.</param>
        public void SaveToCsv(FileStream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                FileCabinetRecordCsvWriter fileCabinetRecordCsvWriter = new FileCabinetRecordCsvWriter(writer);

                for (int i = 0; i < this.records.Length; i++)
                {
                    fileCabinetRecordCsvWriter.Write(this.records[i]);
                }
            }
        }
    }
}
