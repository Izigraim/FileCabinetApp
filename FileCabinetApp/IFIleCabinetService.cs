using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Extract interface to FileCabinetService.
    /// </summary>
    public interface IFIleCabinetService
    {
        /// <summary>
        /// Create new record.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <returns>Identifier of created record or '-1'.</returns>
        public int CreateRecord(FileCabinetRecord record);

        /// <summary>
        /// Editing a record.
        /// </summary>
        /// <param name="record">Record.</param>
        public void EditRecord(FileCabinetRecord record);

        /// <summary>
        /// Search for a record by first name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Iterator.</returns>
        public IRecordIterator FindByFirstName(string firstName);

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="lastname">Last name.</param>
        /// <returns>Iterator.</returns>
        public IRecordIterator FindByLastName(string lastname);

        /// <summary>
        /// Search for a record by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Iterator.</returns>
        public IRecordIterator FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// Gets array of all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <param name="deletedCount">Count of deleted records.</param>
        /// <returns>Number of records.</returns>
        public int GetStat(out int deletedCount);

        /// <summary>
        /// Make snapshot method.
        /// </summary>
        /// <returns>Instance of snapshot class.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restore imported records.
        /// </summary>
        /// <param name="snapshot">Snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Remove record by ID.
        /// </summary>
        /// <param name="id">ID of removed record.</param>
        public void Remove(int id);

        /// <summary>
        /// Clears the file of deleted records.
        /// </summary>
        /// <param name="count">Count of purged records.</param>
        /// <param name="before">Count of records before purge.</param>
        public void Purge(out int count, out int before);
    }
}
