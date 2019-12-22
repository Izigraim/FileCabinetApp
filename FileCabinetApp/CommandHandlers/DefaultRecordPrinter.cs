using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Default type of print.
    /// </summary>
    public class DefaultRecordPrinter : IRecordPrinter
    {
        /// <inheritdoc/>
        public void Print(ReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id + 1}, {record.Sex}, {record.FirstName}, {record.LastName}, {record.Age}, {record.Salary}, {record.DateOfBirth:yyyy-MMM-dd}");
            }
        }
    }
}
