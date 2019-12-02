using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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
                Id = this.list.Count + 1,
                Sex = sex,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Salary = salary,
                DateOfBirth = dateOfBirth,
            };
            this.list.Add(record);

            return record.Id;
        }

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

            this.list.RemoveAt(id);
            this.list.Insert(id, record);
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            FileCabinetRecord[] findedFirstNames = this.list.FindAll(c => c.FirstName.ToLower(CultureInfo.CreateSpecificCulture("en-US")) == firstName.ToLower(CultureInfo.CreateSpecificCulture("en-US"))).ToArray();

            try
            {
                if (findedFirstNames == Array.Empty<FileCabinetRecord>() || findedFirstNames == null || findedFirstNames.Length == 0)
                {
                    throw new ArgumentException("There are no records with this first name.", nameof(firstName));
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return Array.Empty<FileCabinetRecord>();
            }

            return findedFirstNames;
        }

        public FileCabinetRecord[] FindByLastName(string lastname)
        {
            FileCabinetRecord[] findedLastNames = this.list.FindAll(c => c.LastName.ToLower(CultureInfo.CreateSpecificCulture("en-US")) == lastname.ToLower(CultureInfo.CreateSpecificCulture("en-US"))).ToArray();

            try
            {
                if (findedLastNames == Array.Empty<FileCabinetRecord>() || findedLastNames == null || findedLastNames.Length == 0)
                {
                    throw new ArgumentException("There are no records with this first name.", nameof(lastname));
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return Array.Empty<FileCabinetRecord>();
            }

            return findedLastNames;
        }

        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] fileCabinetRecords = new FileCabinetRecord[this.GetStat()];

            for (int i = 0; i < this.GetStat(); i++)
            {
                fileCabinetRecords[i] = this.list[i];
            }

            return fileCabinetRecords;
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
