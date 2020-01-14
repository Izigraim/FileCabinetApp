using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Iterator for worjing with <see cref="FileCabinetMemoryService"/>.
    /// </summary>
    public class MemoryIterator : IRecordIterator
    {
        private int currentIndex;
        private List<FileCabinetRecord> records = new List<FileCabinetRecord>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        /// <param name="records">List of records.</param>
        public MemoryIterator(List<FileCabinetRecord> records)
        {
            this.currentIndex = 0;
            this.records = records;
        }

        /// <inheritdoc/>
        public FileCabinetRecord GetNext()
        {
            return this.records[this.currentIndex++];
        }

        /// <inheritdoc/>
        public bool HasMore()
        {
            return this.records.Count > this.currentIndex;
        }
    }
}
