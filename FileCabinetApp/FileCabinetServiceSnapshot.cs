using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

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
        /// Gets readed records.
        /// </summary>
        /// <value>
        /// Readed records.
        /// </value>
        public List<FileCabinetRecord> Records { get; private set; }

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

        /// <summary>
        /// Save records to .xml file.
        /// </summary>
        /// <param name="stream">FileStream.</param>
        public void SaveToXml(FileStream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                FileCabinetRecordXmlWriter fileCabinetRecordXmlWriter = new FileCabinetRecordXmlWriter(writer);
                fileCabinetRecordXmlWriter.Write(this.records);
            }
        }

        /// <summary>
        /// Load records from CSV file.
        /// </summary>
        /// <param name="reader">StreamReader.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            FileCabinetRecordCsvReader fileCabinetRecordCsvReader = new FileCabinetRecordCsvReader(reader);
            this.Records = fileCabinetRecordCsvReader.ReadAll();
        }
    }
}
