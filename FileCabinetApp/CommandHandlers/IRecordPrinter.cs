using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Printer interface.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Print collection of records.
        /// </summary>
        /// <param name="records">Records to print.</param>
        public void Print(ReadOnlyCollection<FileCabinetRecord> records);
    }
}
