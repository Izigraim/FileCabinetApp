using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    public class FIleCabinetRecordXmlReader
    {
        private StreamReader reader;

        public FIleCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        public List<FileCabinetRecord> ReadAll()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            FileCabinetRecord[] records = (FileCabinetRecord[])serializer.Deserialize(this.reader);
            return records.ToList();
        }
    }
}
