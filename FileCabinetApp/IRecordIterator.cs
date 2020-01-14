using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Common iterator interface.
    /// </summary>
    public interface IRecordIterator
    {
        /// <summary>
        /// Get next item.
        /// </summary>
        /// <returns><see cref="FileCabinetRecord"/>.</returns>
        public FileCabinetRecord GetNext();

        /// <summary>
        /// Check if the following element exists.
        /// </summary>
        /// <returns>True - if exists, false - if not.</returns>
        public bool HasMore();
    }
}
