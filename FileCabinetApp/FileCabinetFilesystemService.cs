using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// File system service class.
    /// </summary>
    public class FileCabinetFilesystemService : IFIleCabinetService
    {
        private const int RecordSize = 278;

        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;
        private readonly SortedDictionary<DateTime, List<long>> dateOfBirthDictionary = new SortedDictionary<DateTime, List<long>>();
        private readonly SortedDictionary<string, List<long>> firstNameDictionaty = new SortedDictionary<string, List<long>>();
        private readonly SortedDictionary<string, List<long>> lastNameDictionary = new SortedDictionary<string, List<long>>();

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

                    record.FirstName = record.FirstName.Trim(' ');

                    byte[] arrayLastName = new byte[120];
                    Array.Copy(recordByte, 126, arrayLastName, 0, 120);
                    string lastNameTmp = temp.GetString(arrayLastName).Trim(' ');
                    for (int j = 0; j < firstNameTmp.IndexOf('\0', StringComparison.Ordinal); j++)
                    {
                        record.LastName += lastNameTmp[j];
                    }

                    record.LastName = record.LastName.Trim(' ');

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

                    byte[] arrayIsDeleted = new byte[2];
                    Array.Copy(recordByte, 276, arrayIsDeleted, 0, 2);
                    short isDeleted = Convert.ToInt16(temp.GetString(arrayIsDeleted), new CultureInfo("en-US"));

                    if (isDeleted == 0)
                    {
                        this.list.Add(record);

                        if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
                        {
                            this.dateOfBirthDictionary[record.DateOfBirth].Add(i * RecordSize);
                        }
                        else
                        {
                            this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<long> { i * RecordSize });
                        }

                        if (this.firstNameDictionaty.ContainsKey(record.FirstName))
                        {
                            this.firstNameDictionaty[record.FirstName].Add(i * RecordSize);
                        }
                        else
                        {
                            this.firstNameDictionaty.Add(record.FirstName, new List<long> { i * RecordSize });
                        }

                        if (this.lastNameDictionary.ContainsKey(record.LastName))
                        {
                            this.lastNameDictionary[record.LastName].Add(i * RecordSize);
                        }
                        else
                        {
                            this.lastNameDictionary.Add(record.LastName, new List<long> { i * RecordSize });
                        }
                    }
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

            if (record.Id == 0)
            {
                record.Id = this.GetStat(out int a);
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Add(record.Id * RecordSize);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<long> { record.Id * RecordSize });
            }

            if (this.firstNameDictionaty.ContainsKey(record.FirstName))
            {
                this.firstNameDictionaty[record.FirstName].Add(record.Id * RecordSize);
            }
            else
            {
                this.firstNameDictionaty.Add(record.FirstName, new List<long> { record.Id * RecordSize });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary[record.LastName].Add(record.Id * RecordSize);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName, new List<long> { record.Id * RecordSize });
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

            record.FirstName = record.FirstName.Trim(' ');
            byte[] arrayFirstName = new byte[120];
            arrayFirstName = new UTF8Encoding(true).GetBytes(record.FirstName);
            arrayFirstName.CopyTo(arrayRecord, 6);

            record.LastName = record.LastName.Trim(' ');
            byte[] arrayLastName = new byte[120];
            arrayLastName = new UTF8Encoding(true).GetBytes(record.LastName);
            arrayLastName.CopyTo(arrayRecord, 126);

            byte[] arrayAge = new byte[2];
            arrayAge = new UTF8Encoding(true).GetBytes(record.Age.ToString(new CultureInfo("en-US")));
            arrayAge.CopyTo(arrayRecord, 246);

            byte[] arraySalary = new byte[16];
            arraySalary = new UTF8Encoding(true).GetBytes(record.Salary.ToString(new CultureInfo("en-US")));
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

            byte[] arrayIsDeleted = new byte[2];
            arrayIsDeleted = new UTF8Encoding(true).GetBytes(0.ToString(new CultureInfo("en-US")));
            arrayIsDeleted.CopyTo(arrayRecord, 276);

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

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Add(record.Id * RecordSize);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<long> { record.Id * RecordSize });
            }

            if (this.firstNameDictionaty.ContainsKey(record.FirstName))
            {
                this.firstNameDictionaty[record.FirstName].Add(record.Id * RecordSize);
            }
            else
            {
                this.firstNameDictionaty.Add(record.FirstName, new List<long> { record.Id * RecordSize });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary[record.LastName].Add(record.Id * RecordSize);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName, new List<long> { record.Id * RecordSize });
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

                record.FirstName = record.FirstName.Trim(' ');
                byte[] arrayFirstName = new byte[120];
                arrayFirstName = new UTF8Encoding(true).GetBytes(record.FirstName);
                arrayFirstName.CopyTo(arrayRecord, 6);

                record.LastName = record.LastName.Trim(' ');
                byte[] arrayLastName = new byte[120];
                arrayLastName = new UTF8Encoding(true).GetBytes(record.LastName);
                arrayLastName.CopyTo(arrayRecord, 126);

                byte[] arrayAge = new byte[2];
                arrayAge = new UTF8Encoding(true).GetBytes(record.Age.ToString(new CultureInfo("en-US")));
                arrayAge.CopyTo(arrayRecord, 246);

                byte[] arraySalary = new byte[16];
                arraySalary = new UTF8Encoding(true).GetBytes(record.Salary.ToString(new CultureInfo("en-US")));
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

                byte[] arrayIsDeleted = new byte[2];
                arrayIsDeleted = new UTF8Encoding(true).GetBytes(0.ToString(new CultureInfo("en-US")));
                arrayIsDeleted.CopyTo(arrayRecord, 276);

                fileStream.Seek(editIndex, SeekOrigin.Begin);
                fileStream.Write(arrayRecord, 0, arrayRecord.Length);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(DateTime.Parse(dateOfBirth, new CultureInfo("en-US"))))
            {
                return new FileSystemIteratorCollection(this.dateOfBirthDictionary[DateTime.Parse(dateOfBirth, new CultureInfo("en-US"))]);
            }
            else
            {
                return new FileSystemIteratorCollection(new List<long> { });
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionaty.ContainsKey(firstName))
            {
                return new FileSystemIteratorCollection(this.firstNameDictionaty[firstName]);
            }
            else
            {
                return new FileSystemIteratorCollection(new List<long> { });
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastname)
        {
            if (this.lastNameDictionary.ContainsKey(lastname))
            {
                return new FileSystemIteratorCollection(this.lastNameDictionary[lastname]);
            }
            else
            {
                return new FileSystemIteratorCollection(new List<long> { });
            }
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

                    byte[] arrayIsDeleted = new byte[2];
                    Array.Copy(recordByte, 276, arrayIsDeleted, 0, 2);
                    short isDeleted = Convert.ToInt16(temp.GetString(arrayIsDeleted), new CultureInfo("en-US"));

                    if (isDeleted == 0)
                    {
                        fileCabinetRecords.Add(record);
                    }
                }
            }

            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecordsCollection = new ReadOnlyCollection<FileCabinetRecord>(fileCabinetRecords);
            return fileCabinetRecordsCollection;
        }

        /// <inheritdoc/>
        public int GetStat(out int deletedCount)
        {
            int size = 0;
            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate))
            {
                size = (int)fileStream.Length / RecordSize;
                int recordsCount = (int)fileStream.Length / RecordSize;
                fileStream.Seek(0, SeekOrigin.Begin);

                deletedCount = 0;

                UTF8Encoding temp = new UTF8Encoding(true);
                byte[] recordByte = new byte[RecordSize];

                for (int i = 0; i < recordsCount; i++)
                {
                    fileStream.Read(recordByte, 0, RecordSize);

                    byte[] arrayIsDeleted = new byte[2];
                    Array.Copy(recordByte, 276, arrayIsDeleted, 0, 2);
                    short isDeleted = Convert.ToInt16(temp.GetString(arrayIsDeleted), new CultureInfo("en-US"));

                    if (isDeleted == 1)
                    {
                        deletedCount++;
                    }
                }
            }

            return size;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        /// <inheritdoc/>
        public void Remove(int id)
        {
            int offset = id * RecordSize;
            FileCabinetRecord record = this.list[id];
            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.Open))
            {
                fileStream.Seek(offset + 276, SeekOrigin.Begin);
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
                arraySalary = new UTF8Encoding(true).GetBytes(record.Salary.ToString(new CultureInfo("en-US")));
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

                byte[] arrayIsDeleted = new byte[2];
                arrayIsDeleted = new UTF8Encoding(true).GetBytes(1.ToString(new CultureInfo("en-US")));
                arrayIsDeleted.CopyTo(arrayRecord, 276);

                fileStream.Seek(offset, SeekOrigin.Begin);
                fileStream.Write(arrayRecord, 0, arrayRecord.Length);
            }
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
        public void Purge(out int count, out int before)
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            int beforePurge;
            int afterPurge;

            using (FileStream fs = new FileStream("cabinet-records.db", FileMode.Open))
            {
                int recordsCount = (int)fs.Length / RecordSize;
                beforePurge = recordsCount;
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

                    byte[] arrayIsDeleted = new byte[2];
                    Array.Copy(recordByte, 276, arrayIsDeleted, 0, 2);
                    short isDeleted = Convert.ToInt16(temp.GetString(arrayIsDeleted), new CultureInfo("en-US"));

                    if (isDeleted == 0)
                    {
                        records.Add(record);
                    }
                }
            }

            using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.Create))
            {
                int i = 0;
                foreach (var record in records)
                {
                    record.Id = i;
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
                    arraySalary = new UTF8Encoding(true).GetBytes(record.Salary.ToString(new CultureInfo("en-US")));
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

                    byte[] arrayIsDeleted = new byte[2];
                    arrayIsDeleted = new UTF8Encoding(true).GetBytes(0.ToString(new CultureInfo("en-US")));
                    arrayIsDeleted.CopyTo(arrayRecord, 276);

                    fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                    fileStream.Write(arrayRecord, 0, arrayRecord.Length);

                    i++;
                }

                afterPurge = (int)fileStream.Length / RecordSize;
            }

            count = beforePurge - afterPurge;
            before = beforePurge;
        }

        /// <inheritdoc/>
        public List<FileCabinetRecord> Memoization(string parameters)
        {
            return null;
        }

        /// <inheritdoc/>
        public void Memoization(string parameters, List<FileCabinetRecord> selectedRecords)
        {
            return;
        }
    }
}
