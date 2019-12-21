using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// XmlReader class.
    /// </summary>
    public class FIleCabinetRecordXmlReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FIleCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">StreamReader.</param>
        public FIleCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Read all records from XML file.
        /// </summary>
        /// <returns>List of readed records.</returns>
        public List<FileCabinetRecord> ReadAll()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            FileCabinetRecord[] records = (FileCabinetRecord[])serializer.Deserialize(this.reader);
            return records.ToList();
        }
    }
}
