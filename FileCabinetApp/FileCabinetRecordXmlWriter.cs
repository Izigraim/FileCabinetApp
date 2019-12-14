using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// XmlWriter class.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.writer = writer;
        }

        /// <summary>
        /// Write records to .xml file.
        /// </summary>
        /// <param name="records">Array of records.</param>
        public void Write(FileCabinetRecord[] records)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            serializer.Serialize(this.writer, records);
        }
    }
}
