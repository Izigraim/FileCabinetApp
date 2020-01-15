using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// Memory service class.
    /// </summary>
    public class FileCabinetMemoryService : IFIleCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">Validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Create new record.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <returns>Identifier of created record or '-1'.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (this.validator.ValidateParameters(record) == false)
            {
                throw new ArgumentException("Data validation is not successful.", nameof(record));
            }

            if (this.list.Where(c => c.Id == record.Id).Count() == 1)
            {
                int index = this.list.FindIndex(c => c.Id == record.Id);
                this.list.RemoveAt(index);
                this.list.Insert(index, record);
            }
            else
            {
                record.Id = this.list.Count;
                this.list.Add(record);
            }

            var recordToAdd = record;

            if (this.firstNameDictionary.ContainsKey(recordToAdd.FirstName))
            {
                this.firstNameDictionary[record.FirstName].Add(recordToAdd);
            }
            else
            {
                this.firstNameDictionary.Add(recordToAdd.FirstName, new List<FileCabinetRecord>() { recordToAdd });
            }

            if (this.lastNameDictionary.ContainsKey(recordToAdd.LastName))
            {
                this.lastNameDictionary[record.LastName].Add(recordToAdd);
            }
            else
            {
                this.lastNameDictionary.Add(recordToAdd.LastName, new List<FileCabinetRecord>() { recordToAdd });
            }

            if (this.dateOfBirthDictionary.ContainsKey(recordToAdd.DateOfBirth.ToString(new CultureInfo("en-US"))))
            {
                this.dateOfBirthDictionary[record.DateOfBirth.ToString(new CultureInfo("en-US"))].Add(recordToAdd);
            }
            else
            {
                this.dateOfBirthDictionary.Add(recordToAdd.DateOfBirth.ToString(new CultureInfo("en-US")), new List<FileCabinetRecord>() { recordToAdd });
            }

            return recordToAdd.Id;
        }

        /// <summary>
        /// Editing a record.
        /// </summary>
        /// <param name="record">Record.</param>
        public void EditRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (this.validator.ValidateParameters(record) == false)
            {
                throw new ArgumentException("Data validation is not successful.", nameof(record));
            }

            var recordEdited = new FileCabinetRecord
            {
                Id = record.Id,
                Sex = record.Sex,
                FirstName = record.FirstName,
                LastName = record.LastName,
                Age = record.Age,
                Salary = record.Salary,
                DateOfBirth = record.DateOfBirth,
            };

            foreach (var key in this.firstNameDictionary.Keys)
            {
                if (this.firstNameDictionary[key].Contains(record))
                {
                    this.firstNameDictionary[key].Remove(record);
                }
            }

            if (this.firstNameDictionary.ContainsKey(recordEdited.FirstName))
            {
                this.firstNameDictionary[record.FirstName].Add(recordEdited);
            }
            else
            {
                this.firstNameDictionary.Add(recordEdited.FirstName, new List<FileCabinetRecord>() { recordEdited });
            }


            foreach (var key in this.lastNameDictionary.Keys)
            {
                if (this.lastNameDictionary[key].Contains(record))
                {
                    this.lastNameDictionary[key].Remove(record);
                }
            }

            if (this.lastNameDictionary.ContainsKey(recordEdited.LastName))
            {
                this.lastNameDictionary[record.LastName].Add(recordEdited);
            }
            else
            {
                this.lastNameDictionary.Add(recordEdited.LastName, new List<FileCabinetRecord>() { recordEdited });
            }

            foreach (var key in this.dateOfBirthDictionary.Keys)
            {
                if (this.dateOfBirthDictionary[key].Contains(record))
                {
                    this.dateOfBirthDictionary[key].Remove(record);
                }
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth.ToString(new CultureInfo("en-US"))))
            {
                this.dateOfBirthDictionary[record.DateOfBirth.ToString(new CultureInfo("en-US"))].Add(recordEdited);
            }
            else
            {
                this.dateOfBirthDictionary.Add(recordEdited.DateOfBirth.ToString(new CultureInfo("en-US")), new List<FileCabinetRecord>() { recordEdited });
            }

            this.list.RemoveAt(record.Id);
            this.list.Insert(record.Id, recordEdited);
        }

        /// <summary>
        /// Search for a record by first name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return new MemoryIteratorCollection(this.firstNameDictionary[firstName]);
            }
            else
            {
                return new MemoryIteratorCollection(new List<FileCabinetRecord> { });
            }
        }

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="lastname">Last name.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastname)
        {
            if (this.lastNameDictionary.ContainsKey(lastname))
            {
                return new MemoryIteratorCollection(this.lastNameDictionary[lastname]);
            }
            else
            {
                return new MemoryIteratorCollection(new List<FileCabinetRecord> { });
            }
        }

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            dateOfBirth = DateTime.Parse(dateOfBirth, new CultureInfo("en-US")).ToString(new CultureInfo("en-US"));

            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                return new MemoryIteratorCollection(this.dateOfBirthDictionary[dateOfBirth]);
            }
            else
            {
                return new MemoryIteratorCollection(new List<FileCabinetRecord> { });
            }
        }

        /// <summary>
        /// Gets array of all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = new ReadOnlyCollection<FileCabinetRecord>(this.list);
            return fileCabinetRecords;
        }

        /// <inheritdoc/>
        public int GetStat(out int deletedCount)
        {
            deletedCount = 0;
            return this.list.Count;
        }

        /// <summary>
        /// Make snapshot method.
        /// </summary>
        /// <returns>Instance of snapshot class.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            foreach (var record in snapshot.Records)
            {
                this.CreateRecord(record);
            }
        }

        /// <inheritdoc/>
        public void Remove(int id)
        {
            FileCabinetRecord record = this.list[id];
            this.list.RemoveAt(id);

            this.firstNameDictionary[record.FirstName].Remove(record);
            this.lastNameDictionary[record.LastName].Remove(record);
            this.dateOfBirthDictionary[record.DateOfBirth.ToString(new CultureInfo("en-US"))].Remove(record);
        }

        /// <inheritdoc/>
        public void Purge(out int count, out int before)
        {
            throw new NotImplementedException();
        }
    }
}
