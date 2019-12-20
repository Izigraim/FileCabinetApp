using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// XmlWriter class.
    /// </summary>
    public class RecordGeneratorXmlWriter
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordGeneratorXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">StreamWriter.</param>
        public RecordGeneratorXmlWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write array of generated data to XML file.
        /// </summary>
        /// <param name="array">Array of generated data.</param>
        public void Write(FileCabinetRecord[] array)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            serializer.Serialize(this.writer, array);
        }
    }
}
