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
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="lastname">Last name.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastname);

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// Gets array of all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat();

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
    }
}
