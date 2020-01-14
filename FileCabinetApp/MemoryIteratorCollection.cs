using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Iterator for worjing with <see cref="FileCabinetMemoryService"/>.
    /// </summary>
    public class MemoryIteratorCollection : IEnumerable<FileCabinetRecord>
    {
        private List<FileCabinetRecord> records = new List<FileCabinetRecord>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIteratorCollection"/> class.
        /// </summary>
        /// <param name="records">List of records.</param>
        public MemoryIteratorCollection(List<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/>.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            return this.GetRecords(this.records);
        }

        /// <summary>
        /// Get enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> object that can be used to iterate through collection.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return this.GetRecords().GetEnumerator();
        }

        /// <summary>
        /// Get enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> object that can be used to iterate through collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetRecords().GetEnumerator();
        }

        private IEnumerable<FileCabinetRecord> GetRecords(List<FileCabinetRecord> list)
        {
            foreach (FileCabinetRecord record in list)
            {
                yield return record;
            }
        }
    }
}
