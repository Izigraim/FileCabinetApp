using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Service class with time measurement.
    /// </summary>
    public class ServiceMeter : IFIleCabinetService
    {
        private IFIleCabinetService fileCabinetService;
        private Stopwatch stopwatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="fileCabinetService">File cabinet service.</param>
        public ServiceMeter(IFIleCabinetService fileCabinetService)
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

            this.stopwatch = Stopwatch.StartNew();

            this.fileCabinetService.CreateRecord(record);

            this.stopwatch.Stop();
            Console.WriteLine($"Create method execution is {this.stopwatch.ElapsedTicks} ticks.");

            return record.Id;
        }

        /// <inheritdoc/>
        public void EditRecord(FileCabinetRecord record)
        {
            this.stopwatch = Stopwatch.StartNew();

            this.fileCabinetService.EditRecord(record);

            this.stopwatch.Stop();
            Console.WriteLine($"Edit method execution is {this.stopwatch.ElapsedTicks} ticks.");
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.stopwatch = Stopwatch.StartNew();

            IEnumerable<FileCabinetRecord> list = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);

            this.stopwatch.Stop();
            Console.WriteLine($"Find by date of birth method execution is {this.stopwatch.ElapsedTicks} ticks.");
            return list;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch = Stopwatch.StartNew();

            IEnumerable<FileCabinetRecord> list = this.fileCabinetService.FindByFirstName(firstName);

            this.stopwatch.Stop();
            Console.WriteLine($"Find by first name method execution is {this.stopwatch.ElapsedTicks} ticks.");
            return list;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastname)
        {
            this.stopwatch = Stopwatch.StartNew();

            IEnumerable<FileCabinetRecord> list = this.fileCabinetService.FindByLastName(lastname);

            this.stopwatch.Stop();
            Console.WriteLine($"Find by last name method execution is {this.stopwatch.ElapsedTicks} ticks.");
            return list;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stopwatch = Stopwatch.StartNew();

            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.GetRecords();

            this.stopwatch.Stop();
            Console.WriteLine($"List method execution is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecords;
        }

        /// <inheritdoc/>
        public int GetStat(out int deletedCount)
        {
            this.stopwatch = Stopwatch.StartNew();

            int stat = this.fileCabinetService.GetStat(out deletedCount);

            this.stopwatch.Stop();
            Console.WriteLine($"Stat method execution is {this.stopwatch.ElapsedTicks} ticks.");
            return stat;
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
            this.stopwatch = Stopwatch.StartNew();

            this.fileCabinetService.Purge(out count, out before);

            this.stopwatch.Stop();
            Console.WriteLine($"Purge method execution is {this.stopwatch.ElapsedTicks} ticks.");
        }

        /// <inheritdoc/>
        public void Remove(int id)
        {
            this.stopwatch = Stopwatch.StartNew();

            this.fileCabinetService.Remove(id);

            this.stopwatch.Stop();
            Console.WriteLine($"Remove method execution is {this.stopwatch.ElapsedTicks} ticks.");
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopwatch = Stopwatch.StartNew();

            this.fileCabinetService.Restore(snapshot);

            this.stopwatch.Stop();
            Console.WriteLine($"Restore method execution is {this.stopwatch.ElapsedTicks} ticks.");
        }
    }
}
