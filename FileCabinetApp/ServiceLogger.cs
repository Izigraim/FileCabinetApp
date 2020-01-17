using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Service class with logging function.
    /// </summary>
    public class ServiceLogger : IFIleCabinetService
    {
        private IFIleCabinetService fileCabinetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="fileCabinetService">File cabinet service.</param>
        public ServiceLogger(IFIleCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        /// <inheritdoc/>
        public int CreateRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            int returned = this.fileCabinetService.CreateRecord(record);

            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling Create() with Sex = '{record.Sex}', FirstName = '{record.FirstName}', LastName = '{record.LastName}', Age = '{record.Age}', Salary = '{record.Salary}', DateOfBirth = '{record.DateOfBirth.ToString(new CultureInfo("en-US"))}'");
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Create() returned '{returned}'");
            }

            return returned;
        }

        /// <inheritdoc/>
        public void EditRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.fileCabinetService.EditRecord(record);

            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling Edit() with Sex = '{record.Sex}', FirstName = '{record.FirstName}', LastName = '{record.LastName}', Age = '{record.Age}', Salary = '{record.Salary}', DateOfBirth = '{record.DateOfBirth.ToString(new CultureInfo("en-US"))}'");
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling FindByDateOfBirth() with DateOfBirth = {dateOfBirth}");
            }

            return this.fileCabinetService.FindByDateOfBirth(dateOfBirth);
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling FindByFirstName() with FirstName = {firstName}");
            }

            return this.fileCabinetService.FindByFirstName(firstName);
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastname)
        {
            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling FindByLastName() with LastName = {lastname}");
            }

            return this.fileCabinetService.FindByLastName(lastname);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling List()");
            }

            return this.fileCabinetService.GetRecords();
        }

        /// <inheritdoc/>
        public int GetStat(out int deletedCount)
        {
            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling Stat()");
            }

            return this.fileCabinetService.GetStat(out deletedCount);
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return this.fileCabinetService.MakeSnapshot();
        }

        /// <inheritdoc/>
        public List<FileCabinetRecord> Memoization(string parameters)
        {
            return this.fileCabinetService.Memoization(parameters);
        }

        /// <inheritdoc/>
        public void Memoization(string parameters, List<FileCabinetRecord> selectedRecords)
        {
            this.fileCabinetService.Memoization(parameters, selectedRecords);
        }

        /// <inheritdoc/>
        public void Purge(out int count, out int before)
        {
            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling Purge()");
            }

            this.fileCabinetService.Purge(out count, out before);
        }

        /// <inheritdoc/>
        public void Remove(int id)
        {
            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling Remove() with ID = {id}");
            }

            this.fileCabinetService.Remove(id);
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            using (StreamWriter sw = new StreamWriter("logs.txt", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"))} - Calling Import() with DateOfBirth");
            }

            this.fileCabinetService.Restore(snapshot);
        }
    }
}
