using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    public class RecordGeneratorXmlWriter
    {
        private StreamWriter writer;

        public RecordGeneratorXmlWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void Write(FileCabinetRecord[] list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            serializer.Serialize(this.writer, list);
        }
    }
}
