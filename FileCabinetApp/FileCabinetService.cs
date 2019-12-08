using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Create new record.
        /// </summary>
        /// <param name="sex">Sex.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="age">Age.</param>
        /// <param name="salary">Salary.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Identifier of created record or '-1'.</returns>
        public int CreateRecord(char sex, string firstName, string lastName, short age, decimal salary, DateTime dateOfBirth)
        {
            try
            {
                if (firstName == null)
                {
                    throw new ArgumentNullException(nameof(firstName));
                }

                if (firstName.Length > 60 || firstName.Length < 2 || firstName.Contains(' ', StringComparison.Ordinal))
                {
                    throw new ArgumentException("Incorrect first name format.", nameof(firstName));
                }

                if (lastName == null)
                {
                    throw new ArgumentNullException(nameof(lastName));
                }

                if (lastName.Length > 60 || lastName.Length < 2 || lastName.Contains(' ', StringComparison.Ordinal))
                {
                    throw new ArgumentException("Incorrect last name format.", nameof(lastName));
                }

                if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
                {
                    throw new ArgumentException("Incorrect date.", nameof(dateOfBirth));
                }

                if (sex != 'w' && sex != 'm')
                {
                    throw new ArgumentException("Incorrect sex format.", nameof(sex));
                }

                if (age > (DateTime.Now.Year - 1950) || age < 0)
                {
                    throw new ArgumentException("Incorrect age format.", nameof(age));
                }

                if (salary < 0)
                {
                    throw new ArgumentException("Incorrect 'salary' format.");
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count,
                Sex = sex,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Salary = salary,
                DateOfBirth = dateOfBirth,
            };
            this.list.Add(record);

            if (this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary[firstName].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord>() { record });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary[lastName].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord>() { record });
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))))
            {
                this.dateOfBirthDictionary[dateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US")), new List<FileCabinetRecord>() { record });
            }

            return record.Id;
        }

        /// <summary>
        /// Editing a record.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="sex">Sex.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="age">Age.</param>
        /// <param name="salary">Salary.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        public void EditRecord(int id, char sex, string firstName, string lastName, short age, decimal salary, DateTime dateOfBirth)
        {
            var record = new FileCabinetRecord
            {
                Id = id,
                Sex = sex,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Salary = salary,
                DateOfBirth = dateOfBirth,
            };

            this.firstNameDictionary[this.list[id].FirstName].Remove(this.list[id]);
            if (this.firstNameDictionary[this.list[id].FirstName].Count == 0)
            {
                this.firstNameDictionary.Remove(this.list[id].FirstName);
            }

            if (this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary[firstName].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord>() { record });
            }

            this.lastNameDictionary[this.list[id].LastName].Remove(this.list[id]);
            if (this.lastNameDictionary[this.list[id].LastName].Count == 0)
            {
                this.lastNameDictionary.Remove(this.list[id].LastName);
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary[firstName].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord>() { record });
            }

            this.dateOfBirthDictionary[this.list[id].DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))].Remove(this.list[id]);
            if (this.lastNameDictionary[this.list[id].LastName].Count == 0)
            {
                this.lastNameDictionary.Remove(this.list[id].LastName);
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))))
            {
                this.dateOfBirthDictionary[dateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US"))].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US")), new List<FileCabinetRecord>() { record });
            }

            this.list.RemoveAt(id);
            this.list.Insert(id, record);
        }

        /// <summary>
        /// Search for a record by first name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
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
                return Array.Empty<FileCabinetRecord>();
            }

            FileCabinetRecord[] findedFirstNames = this.firstNameDictionary[firstName].ToArray();

            return findedFirstNames;
        }

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="lastname">Last name.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public FileCabinetRecord[] FindByLastName(string lastname)
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
                return Array.Empty<FileCabinetRecord>();
            }

            FileCabinetRecord[] findedFirstNames = this.lastNameDictionary[lastname].ToArray();

            return findedFirstNames;
        }

        /// <summary>
        /// Search for a record by last name.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Finded record or <see cref="ArgumentException"/>.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            FileCabinetRecord[] findedDateOfBirth = this.list.FindAll(c => c.DateOfBirth == DateTime.Parse(dateOfBirth, CultureInfo.CurrentCulture)).ToArray();

            try
            {
                if (findedDateOfBirth == Array.Empty<FileCabinetRecord>() || findedDateOfBirth == null || findedDateOfBirth.Length == 0)
                {
                    throw new ArgumentException("There are no records with this first name.", nameof(dateOfBirth));
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return Array.Empty<FileCabinetRecord>();
            }

            return findedDateOfBirth;
        }

        /// <summary>
        /// Gets array of all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] fileCabinetRecords = new FileCabinetRecord[this.GetStat()];

            for (int i = 0; i < this.GetStat(); i++)
            {
                fileCabinetRecords[i] = this.list[i];
            }

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
