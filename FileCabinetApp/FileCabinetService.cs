using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Service class.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">Validator.</param>
        public FileCabinetService(IRecordValidator validator)
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

            record.Id = this.list.Count;

            var recordToAdd = record;

            this.list.Add(recordToAdd);

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

            if (this.dateOfBirthDictionary.ContainsKey(recordToAdd.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))))
            {
                this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))].Add(recordToAdd);
            }
            else
            {
                this.dateOfBirthDictionary.Add(recordToAdd.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US")), new List<FileCabinetRecord>() { recordToAdd });
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

            this.firstNameDictionary[this.list[record.Id].FirstName].Remove(this.list[record.Id]);
            if (this.firstNameDictionary[this.list[record.Id].FirstName].Count == 0)
            {
                this.firstNameDictionary.Remove(this.list[record.Id].FirstName);
            }

            if (this.firstNameDictionary.ContainsKey(recordEdited.FirstName))
            {
                this.firstNameDictionary[record.FirstName].Add(recordEdited);
            }
            else
            {
                this.firstNameDictionary.Add(recordEdited.FirstName, new List<FileCabinetRecord>() { recordEdited });
            }

            this.lastNameDictionary[this.list[record.Id].LastName].Remove(this.list[record.Id]);
            if (this.lastNameDictionary[this.list[record.Id].LastName].Count == 0)
            {
                this.lastNameDictionary.Remove(this.list[record.Id].LastName);
            }

            if (this.lastNameDictionary.ContainsKey(recordEdited.LastName))
            {
                this.lastNameDictionary[record.LastName].Add(recordEdited);
            }
            else
            {
                this.lastNameDictionary.Add(recordEdited.LastName, new List<FileCabinetRecord>() { recordEdited });
            }

            this.dateOfBirthDictionary[this.list[record.Id].DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))].Remove(this.list[record.Id]);
            if (this.dateOfBirthDictionary[this.list[record.Id].DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))].Count == 0)
            {
                this.dateOfBirthDictionary.Remove(this.list[record.Id].DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US")));
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))))
            {
                this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))].Add(recordEdited);
            }
            else
            {
                this.dateOfBirthDictionary.Add(recordEdited.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US")), new List<FileCabinetRecord>() { recordEdited });
            }

            this.list.RemoveAt(record.Id);
            this.list.Insert(record.Id, recordEdited);
        }

        /// <summary>
        /// Search for a record by first name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            try
            {
                if (!this.firstNameDictionary.ContainsKey(firstName))
                {
                    throw new ArgumentException("There is no record(s) with this first name.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            ReadOnlyCollection<FileCabinetRecord> findedFirstNameCollection = new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);

            return findedFirstNameCollection;
        }

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="lastname">Last name.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastname)
        {
            try
            {
                if (!this.lastNameDictionary.ContainsKey(lastname))
                {
                    throw new ArgumentException("There is no record(s) with this first name.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            ReadOnlyCollection<FileCabinetRecord> findedLastNameCollection = new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastname]);

            return findedLastNameCollection;
        }

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            ReadOnlyCollection<FileCabinetRecord> findedDateOfBirthCollection = new ReadOnlyCollection<FileCabinetRecord>(this.list.FindAll(c => c.DateOfBirth == DateTime.Parse(dateOfBirth, CultureInfo.CurrentCulture)));
            try
            {
                if (findedDateOfBirthCollection == null)
                {
                    throw new ArgumentException("There are no records with this first name.", nameof(dateOfBirth));
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return findedDateOfBirthCollection;
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

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
