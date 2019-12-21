using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// File system service class.
    /// </summary>
    public class FileCabinetFilesystemService : IFIleCabinetService
    {
        private const int RecordSize = 276;

        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">FileStream.</param>
        /// <param name="validator">Validator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;

            using (FileStream fs = new FileStream("cabinet-records.db", FileMode.Open))
            {
                int recordsCount = (int)fs.Length / RecordSize;
                fs.Seek(0, SeekOrigin.Begin);

                UTF8Encoding temp = new UTF8Encoding(true);
                byte[] recordByte = new byte[RecordSize];

                for (int i = 0; i < recordsCount; i++)
                {
                    fs.Read(recordByte, 0, RecordSize);

                    FileCabinetRecord record = new FileCabinetRecord();

                    byte[] arrayId = new byte[4];
                    Array.Copy(recordByte, 0, arrayId, 0, 4);
                    record.Id = Convert.ToInt32(temp.GetString(arrayId), new CultureInfo("en-US"));

                    byte[] arraySex = new byte[2];
                    Array.Copy(recordByte, 4, arraySex, 0, 2);
                    record.Sex = Convert.ToChar(temp.GetString(arraySex)[0], new CultureInfo("en-US"));

                    byte[] arrayFirstName = new byte[120];
                    Array.Copy(recordByte, 6, arrayFirstName, 0, 120);
                    string firstNameTmp = temp.GetString(arrayFirstName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.FirstName += firstNameTmp[j];
                    }

                    byte[] arrayLastName = new byte[120];
                    Array.Copy(recordByte, 126, arrayLastName, 0, 120);
                    string lastNameTmp = temp.GetString(arrayLastName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.LastName += lastNameTmp[j];
                    }

                    byte[] arrayAge = new byte[2];
                    Array.Copy(recordByte, 246, arrayAge, 0, 2);
                    record.Age = Convert.ToInt16(temp.GetString(arrayAge), new CultureInfo("en-US"));

                    byte[] arraySalary = new byte[16];
                    Array.Copy(recordByte, 248, arraySalary, 0, 16);
                    record.Salary = Convert.ToDecimal(temp.GetString(arraySalary), new CultureInfo("en-US"));

                    byte[] arrayYear = new byte[4];
                    Array.Copy(recordByte, 264, arrayYear, 0, 4);
                    int year = Convert.ToInt32(temp.GetString(arrayYear), new CultureInfo("en-US"));

                    byte[] arrayMonth = new byte[4];
                    Array.Copy(recordByte, 268, arrayMonth, 0, 4);
                    int month = Convert.ToInt32(temp.GetString(arrayMonth), new CultureInfo("en-US"));

                    byte[] arrayDay = new byte[4];
                    Array.Copy(recordByte, 272, arrayDay, 0, 4);
                    int day = Convert.ToInt32(temp.GetString(arrayDay), new CultureInfo("en-US"));

                    string date = month.ToString(new CultureInfo("en-US")) + "/" + day.ToString(new CultureInfo("en-US")) + "/" + year.ToString(new CultureInfo("en-US"));
                    record.DateOfBirth = DateTime.Parse(date, new CultureInfo("en-US"));

                    this.list.Add(record);
                }
            }
        }

        /// <inheritdoc/>
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

            bool rewriteFlag = false;
            int index = 0;
            if (this.list.Where(c => c.Id == record.Id).Count() == 1)
            {
                index = this.list.FindIndex(c => c.Id == record.Id);
                this.list.RemoveAt(index);
                this.list.Insert(index, record);
                rewriteFlag = true;
            }
            else
            {
                record.Id = this.list.Count;
                this.list.Add(record);
            }

            var recordToAdd = record;

            byte[] arrayRecord = new byte[RecordSize];

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

            if (rewriteFlag)
            {
                using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.Open))
                {
                    fileStream.Seek(index * RecordSize, SeekOrigin.Begin);
                    fileStream.Write(arrayRecord, 0, arrayRecord.Length);
                }
            }
            else
            {
                using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.Open))
                {
                    fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                    fileStream.Write(arrayRecord, 0, arrayRecord.Length);
                }
            }

            return recordToAdd.Id;
        }

        /// <inheritdoc/>
        public void EditRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            int editIndex = record.Id * RecordSize;

            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.Open))
            {
                byte[] arrayRecord = new byte[RecordSize];

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

                fileStream.Seek(editIndex, SeekOrigin.Begin);
                fileStream.Write(arrayRecord, 0, arrayRecord.Length);
            }
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate))
            {
                int recordsCount = (int)fileStream.Length / RecordSize;
                fileStream.Seek(0, SeekOrigin.Begin);

                UTF8Encoding temp = new UTF8Encoding(true);
                byte[] recordByte = new byte[RecordSize];

                for (int i = 0; i < recordsCount; i++)
                {
                    fileStream.Read(recordByte, 0, RecordSize);

                    FileCabinetRecord record = new FileCabinetRecord();

                    byte[] arrayId = new byte[4];
                    Array.Copy(recordByte, 0, arrayId, 0, 4);
                    record.Id = Convert.ToInt32(temp.GetString(arrayId), new CultureInfo("en-US"));

                    byte[] arraySex = new byte[2];
                    Array.Copy(recordByte, 4, arraySex, 0, 2);
                    record.Sex = Convert.ToChar(temp.GetString(arraySex)[0], new CultureInfo("en-US"));

                    byte[] arrayFirstName = new byte[120];
                    Array.Copy(recordByte, 6, arrayFirstName, 0, 120);
                    string firstNameTmp = temp.GetString(arrayFirstName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.FirstName += firstNameTmp[j];
                    }

                    byte[] arrayLastName = new byte[120];
                    Array.Copy(recordByte, 126, arrayLastName, 0, 120);
                    string lastNameTmp = temp.GetString(arrayLastName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.LastName += lastNameTmp[j];
                    }

                    byte[] arrayAge = new byte[2];
                    Array.Copy(recordByte, 246, arrayAge, 0, 2);
                    record.Age = Convert.ToInt16(temp.GetString(arrayAge), new CultureInfo("en-US"));

                    byte[] arraySalary = new byte[16];
                    Array.Copy(recordByte, 248, arraySalary, 0, 16);
                    record.Salary = Convert.ToDecimal(temp.GetString(arraySalary), new CultureInfo("en-US"));

                    byte[] arrayYear = new byte[4];
                    Array.Copy(recordByte, 264, arrayYear, 0, 4);
                    int year = Convert.ToInt32(temp.GetString(arrayYear), new CultureInfo("en-US"));

                    byte[] arrayMonth = new byte[4];
                    Array.Copy(recordByte, 268, arrayMonth, 0, 4);
                    int month = Convert.ToInt32(temp.GetString(arrayMonth), new CultureInfo("en-US"));

                    byte[] arrayDay = new byte[4];
                    Array.Copy(recordByte, 272, arrayDay, 0, 4);
                    int day = Convert.ToInt32(temp.GetString(arrayDay), new CultureInfo("en-US"));

                    string date = month.ToString(new CultureInfo("en-US")) + "/" + day.ToString(new CultureInfo("en-US")) + "/" + year.ToString(new CultureInfo("en-US"));
                    record.DateOfBirth = DateTime.Parse(date, new CultureInfo("en-US"));

                    if (record.DateOfBirth == DateTime.Parse(dateOfBirth, new CultureInfo("en-US")))
                    {
                        records.Add(record);
                    }
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate))
            {
                int recordsCount = (int)fileStream.Length / RecordSize;
                fileStream.Seek(0, SeekOrigin.Begin);

                UTF8Encoding temp = new UTF8Encoding(true);
                byte[] recordByte = new byte[RecordSize];

                for (int i = 0; i < recordsCount; i++)
                {
                    fileStream.Read(recordByte, 0, RecordSize);

                    FileCabinetRecord record = new FileCabinetRecord();

                    byte[] arrayId = new byte[4];
                    Array.Copy(recordByte, 0, arrayId, 0, 4);
                    record.Id = Convert.ToInt32(temp.GetString(arrayId), new CultureInfo("en-US"));

                    byte[] arraySex = new byte[2];
                    Array.Copy(recordByte, 4, arraySex, 0, 2);
                    record.Sex = Convert.ToChar(temp.GetString(arraySex)[0], new CultureInfo("en-US"));

                    byte[] arrayFirstName = new byte[120];
                    Array.Copy(recordByte, 6, arrayFirstName, 0, 120);
                    string firstNameTmp = temp.GetString(arrayFirstName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.FirstName += firstNameTmp[j];
                    }

                    byte[] arrayLastName = new byte[120];
                    Array.Copy(recordByte, 126, arrayLastName, 0, 120);
                    string lastNameTmp = temp.GetString(arrayLastName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.LastName += lastNameTmp[j];
                    }

                    byte[] arrayAge = new byte[2];
                    Array.Copy(recordByte, 246, arrayAge, 0, 2);
                    record.Age = Convert.ToInt16(temp.GetString(arrayAge), new CultureInfo("en-US"));

                    byte[] arraySalary = new byte[16];
                    Array.Copy(recordByte, 248, arraySalary, 0, 16);
                    record.Salary = Convert.ToDecimal(temp.GetString(arraySalary), new CultureInfo("en-US"));

                    byte[] arrayYear = new byte[4];
                    Array.Copy(recordByte, 264, arrayYear, 0, 4);
                    int year = Convert.ToInt32(temp.GetString(arrayYear), new CultureInfo("en-US"));

                    byte[] arrayMonth = new byte[4];
                    Array.Copy(recordByte, 268, arrayMonth, 0, 4);
                    int month = Convert.ToInt32(temp.GetString(arrayMonth), new CultureInfo("en-US"));

                    byte[] arrayDay = new byte[4];
                    Array.Copy(recordByte, 272, arrayDay, 0, 4);
                    int day = Convert.ToInt32(temp.GetString(arrayDay), new CultureInfo("en-US"));

                    string date = month.ToString(new CultureInfo("en-US")) + "/" + day.ToString(new CultureInfo("en-US")) + "/" + year.ToString(new CultureInfo("en-US"));
                    record.DateOfBirth = DateTime.Parse(date, new CultureInfo("en-US"));

                    if (record.FirstName == firstName)
                    {
                        records.Add(record);
                    }
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastname)
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate))
            {
                int recordsCount = (int)fileStream.Length / RecordSize;
                fileStream.Seek(0, SeekOrigin.Begin);

                UTF8Encoding temp = new UTF8Encoding(true);
                byte[] recordByte = new byte[RecordSize];

                for (int i = 0; i < recordsCount; i++)
                {
                    fileStream.Read(recordByte, 0, RecordSize);

                    FileCabinetRecord record = new FileCabinetRecord();

                    byte[] arrayId = new byte[4];
                    Array.Copy(recordByte, 0, arrayId, 0, 4);
                    record.Id = Convert.ToInt32(temp.GetString(arrayId), new CultureInfo("en-US"));

                    byte[] arraySex = new byte[2];
                    Array.Copy(recordByte, 4, arraySex, 0, 2);
                    record.Sex = Convert.ToChar(temp.GetString(arraySex)[0], new CultureInfo("en-US"));

                    byte[] arrayFirstName = new byte[120];
                    Array.Copy(recordByte, 6, arrayFirstName, 0, 120);
                    string firstNameTmp = temp.GetString(arrayFirstName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.FirstName += firstNameTmp[j];
                    }

                    byte[] arrayLastName = new byte[120];
                    Array.Copy(recordByte, 126, arrayLastName, 0, 120);
                    string lastNameTmp = temp.GetString(arrayLastName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.LastName += lastNameTmp[j];
                    }

                    byte[] arrayAge = new byte[2];
                    Array.Copy(recordByte, 246, arrayAge, 0, 2);
                    record.Age = Convert.ToInt16(temp.GetString(arrayAge), new CultureInfo("en-US"));

                    byte[] arraySalary = new byte[16];
                    Array.Copy(recordByte, 248, arraySalary, 0, 16);
                    record.Salary = Convert.ToDecimal(temp.GetString(arraySalary), new CultureInfo("en-US"));

                    byte[] arrayYear = new byte[4];
                    Array.Copy(recordByte, 264, arrayYear, 0, 4);
                    int year = Convert.ToInt32(temp.GetString(arrayYear), new CultureInfo("en-US"));

                    byte[] arrayMonth = new byte[4];
                    Array.Copy(recordByte, 268, arrayMonth, 0, 4);
                    int month = Convert.ToInt32(temp.GetString(arrayMonth), new CultureInfo("en-US"));

                    byte[] arrayDay = new byte[4];
                    Array.Copy(recordByte, 272, arrayDay, 0, 4);
                    int day = Convert.ToInt32(temp.GetString(arrayDay), new CultureInfo("en-US"));

                    string date = month.ToString(new CultureInfo("en-US")) + "/" + day.ToString(new CultureInfo("en-US")) + "/" + year.ToString(new CultureInfo("en-US"));
                    record.DateOfBirth = DateTime.Parse(date, new CultureInfo("en-US"));

                    if (record.LastName == lastname)
                    {
                        records.Add(record);
                    }
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> fileCabinetRecords = new List<FileCabinetRecord>();

            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate))
            {
                int recordsCount = (int)fileStream.Length / RecordSize;
                fileStream.Seek(0, SeekOrigin.Begin);

                UTF8Encoding temp = new UTF8Encoding(true);
                byte[] recordByte = new byte[RecordSize];

                for (int i = 0; i < recordsCount; i++)
                {
                    fileStream.Read(recordByte, 0, RecordSize);

                    FileCabinetRecord record = new FileCabinetRecord();

                    byte[] arrayId = new byte[4];
                    Array.Copy(recordByte, 0, arrayId, 0, 4);
                    record.Id = Convert.ToInt32(temp.GetString(arrayId), new CultureInfo("en-US"));

                    byte[] arraySex = new byte[2];
                    Array.Copy(recordByte, 4, arraySex, 0, 2);
                    record.Sex = Convert.ToChar(temp.GetString(arraySex)[0], new CultureInfo("en-US"));

                    byte[] arrayFirstName = new byte[120];
                    Array.Copy(recordByte, 6, arrayFirstName, 0, 120);
                    string firstNameTmp = temp.GetString(arrayFirstName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.FirstName += firstNameTmp[j];
                    }

                    byte[] arrayLastName = new byte[120];
                    Array.Copy(recordByte, 126, arrayLastName, 0, 120);
                    string lastNameTmp = temp.GetString(arrayLastName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.LastName += lastNameTmp[j];
                    }

                    byte[] arrayAge = new byte[2];
                    Array.Copy(recordByte, 246, arrayAge, 0, 2);
                    record.Age = Convert.ToInt16(temp.GetString(arrayAge), new CultureInfo("en-US"));

                    byte[] arraySalary = new byte[16];
                    Array.Copy(recordByte, 248, arraySalary, 0, 16);
                    record.Salary = Convert.ToDecimal(temp.GetString(arraySalary), new CultureInfo("en-US"));

                    byte[] arrayYear = new byte[4];
                    Array.Copy(recordByte, 264, arrayYear, 0, 4);
                    int year = Convert.ToInt32(temp.GetString(arrayYear), new CultureInfo("en-US"));

                    byte[] arrayMonth = new byte[4];
                    Array.Copy(recordByte, 268, arrayMonth, 0, 4);
                    int month = Convert.ToInt32(temp.GetString(arrayMonth), new CultureInfo("en-US"));

                    byte[] arrayDay = new byte[4];
                    Array.Copy(recordByte, 272, arrayDay, 0, 4);
                    int day = Convert.ToInt32(temp.GetString(arrayDay), new CultureInfo("en-US"));

                    string date = month.ToString(new CultureInfo("en-US")) + "/" + day.ToString(new CultureInfo("en-US")) + "/" + year.ToString(new CultureInfo("en-US"));
                    record.DateOfBirth = DateTime.Parse(date, new CultureInfo("en-US"));

                    fileCabinetRecords.Add(record);
                }
            }

            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecordsCollection = new ReadOnlyCollection<FileCabinetRecord>(fileCabinetRecords);
            return fileCabinetRecordsCollection;
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            int size = 0;
            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate))
            {
                size = (int)fileStream.Length / RecordSize;
            }

            return size;
        }

        /// <inheritdoc/>
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
    }
}
