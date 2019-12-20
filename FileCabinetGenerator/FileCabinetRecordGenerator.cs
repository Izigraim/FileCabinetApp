using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    public class FileCabinetRecordGenerator
    {
        private int startId;
        private Random random;

        public FileCabinetRecordGenerator(int startId)
        {
            this.startId = startId;
            this.random = new Random();
        }

        public FileCabinetRecord Generate()
        {
            FileCabinetRecord record = new FileCabinetRecord();

            record.Id = this.startId;

            char[] sex = { 'w', 'm' };
            record.Sex = sex[this.random.Next(0, 2)];

            string name = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i <= this.random.Next(2, 61); i++)
            {
                record.FirstName += name[this.random.Next(0, name.Length)];
            }

            for (int i = 0; i <= this.random.Next(2, 61); i++)
            {
                record.LastName += name[this.random.Next(0, name.Length)];
            }

            record.Salary = Math.Round(Convert.ToDecimal(this.random.NextDouble() * 5000), 2);

            while (true)
            {
                try
                {
                    record.DateOfBirth = DateTime.Parse(this.random.Next(1, 13).ToString(new CultureInfo("en-US")) + "/" + this.random.Next(1, 32).ToString(new CultureInfo("en-US")) + "/" + this.random.Next(1950, 2020).ToString(new CultureInfo("en-US")), new CultureInfo("en-US"));
                }
                catch (Exception ex)
                {
                    continue;
                }

                break;
            }

            record.Age = Convert.ToInt16(DateTime.Now.Year - record.DateOfBirth.Year);

            this.startId++;

            return record;
        }
    }
}
