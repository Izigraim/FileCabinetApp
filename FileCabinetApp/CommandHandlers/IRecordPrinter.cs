using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public interface IRecordPrinter
    {
        public void Print(ReadOnlyCollection<FileCabinetRecord> records);
    }
}
