using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Iterator for working with <see cref="FileCabinetFilesystemService"/>.
    /// </summary>
    public class FileSystemIteratorCollection : IEnumerable<FileCabinetRecord>
    {
        private const int RecordSize = 278;
        private List<long> cache = new List<long>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemIteratorCollection"/> class.
        /// </summary>
        /// <param name="cache">List of offsets for elements on file.</param>
        public FileSystemIteratorCollection(List<long> cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// Get enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> object that can be used to iterate through collection.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new FileSystemIteratorEnumerator(this.cache);
        }

        /// <summary>
        /// Get enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> object that can be used to iterate through collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FileSystemIteratorEnumerator(this.cache);
        }

        /// <summary>
        /// Enumerator class.
        /// </summary>
        public sealed class FileSystemIteratorEnumerator : IEnumerator<FileCabinetRecord>
        {
            private List<long> list = new List<long>();
            private int currentIndex;

            /// <summary>
            /// Initializes a new instance of the <see cref="FileSystemIteratorEnumerator"/> class.
            /// </summary>
            /// <param name="list">List of offsets.</param>
            public FileSystemIteratorEnumerator(List<long> list)
            {
                this.list = list;
                this.currentIndex = -1;
            }

            /// <summary>
            /// Gets current element.
            /// </summary>
            /// <value>
            /// Current element.
            /// </value>
            public FileCabinetRecord Current
            {
                get
                {
                    FileCabinetRecord record = new FileCabinetRecord();

                    using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate))
                    {
                        UTF8Encoding temp = new UTF8Encoding(true);
                        byte[] recordByte = new byte[RecordSize];

                        fileStream.Seek(this.list[this.currentIndex], SeekOrigin.Begin);
                        fileStream.Read(recordByte, 0, RecordSize);

                        byte[] arrayIsDeleted = new byte[2];
                        Array.Copy(recordByte, 276, arrayIsDeleted, 0, 2);
                        short isDeleted = Convert.ToInt16(new UTF8Encoding(true).GetString(arrayIsDeleted), new CultureInfo("en-US"));

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
                    }

                    return record;
                }
            }

            /// <summary>
            /// Gets current element.
            /// </summary>
            /// <value>
            /// Current element.
            /// </value>
            object IEnumerator.Current
            {
                get
                {
                    FileCabinetRecord record = new FileCabinetRecord();

                    using (FileStream fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate))
                    {
                        UTF8Encoding temp = new UTF8Encoding(true);
                        byte[] recordByte = new byte[RecordSize];

                        fileStream.Seek(this.list[this.currentIndex], SeekOrigin.Begin);
                        fileStream.Read(recordByte, 0, RecordSize);

                        byte[] arrayIsDeleted = new byte[2];
                        Array.Copy(recordByte, 276, arrayIsDeleted, 0, 2);
                        short isDeleted = Convert.ToInt16(new UTF8Encoding(true).GetString(arrayIsDeleted), new CultureInfo("en-US"));

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
                    }

                    return record;
                }
            }

            /// <summary>
            /// Implement IDisposable.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Checking whether we can go to the next item in the collection.
            /// </summary>
            /// <returns>True, if we can, false - if not.</returns>
            public bool MoveNext()
            {
                if (this.currentIndex < this.list.Count - 1)
                {
                    this.currentIndex++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Reset position in collection.
            /// </summary>
            public void Reset()
            {
                this.currentIndex = -1;
            }
        }
    }
}
