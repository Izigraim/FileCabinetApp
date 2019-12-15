using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFIleCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

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


            byte[] arrayRecord = new byte[276];

            byte[] arrayId = new byte[4];
            arrayId = new UTF8Encoding(true).GetBytes(record.Id.ToString(new CultureInfo("en-US")));
            arrayId.CopyTo(arrayRecord, 0);

            byte[] arraySex = new byte[2];
            arraySex = new UTF8Encoding(true).GetBytes(record.Sex.ToString(new CultureInfo("en-US")));
            arraySex.CopyTo(arrayRecord, 4);

            byte[] arrayFirstName = new byte[120];
            arrayFirstName = new UTF8Encoding(true).GetBytes(record.FirstName);
            arrayFirstName.CopyTo(arrayRecord, 6);

            byte[] arrayLastName = new byte[120];
            arrayLastName = new UTF8Encoding(true).GetBytes(record.LastName);
            arrayLastName.CopyTo(arrayRecord, 126);

            byte[] arrayAge = new byte[2];
            arrayAge = new UTF8Encoding(true).GetBytes(record.Age.ToString(new CultureInfo("en-US")));
            arrayAge.CopyTo(arrayRecord, 246);

            byte[] arraySalary = new byte[16];
            arraySalary = new UTF8Encoding(true).GetBytes(record.Salary?.ToString(new CultureInfo("en-US")));
            arraySalary.CopyTo(arrayRecord, 248);

            byte[] arrayYear = new byte[4];
            arrayYear = new UTF8Encoding(true).GetBytes(record.DateOfBirth.Year.ToString(new CultureInfo("en-US")));
            arrayYear.CopyTo(arrayRecord, 264);

            byte[] arrayMonth = new byte[4];
            arrayMonth = new UTF8Encoding(true).GetBytes(record.DateOfBirth.Month.ToString(new CultureInfo("en-US")));
            arrayMonth.CopyTo(arrayRecord, 268);

            byte[] arrayDay = new byte[4];
            arrayDay = new UTF8Encoding(true).GetBytes(record.DateOfBirth.Day.ToString(new CultureInfo("en-US")));
            arrayDay.CopyTo(arrayRecord, 272);

            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.Open))
            {
                fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                fileStream.Write(arrayRecord, 0, arrayRecord.Length);
            }

            return recordToAdd.Id;
        }

        public void EditRecord(FileCabinetRecord record)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastname)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
