using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (CommandLineParameters.GeneratorValidation(args))
            {
                List<FileCabinetRecord> list = new List<FileCabinetRecord>();

                int startId = 0;
                int amount = 0;
                if (args.Length == 4)
                {
                    string[] start = args[3].Split('=');
                    startId = Convert.ToInt32(start[1], new CultureInfo("en-US"));

                    string[] amountString = args[2].Split('=');
                    amount = Convert.ToInt32(amountString[1], new CultureInfo("en-US"));
                }

                if (args.Length == 8)
                {
                    startId = Convert.ToInt32(args[7], new CultureInfo("en-US"));
                    amount = Convert.ToInt32(args[5], new CultureInfo("en-US"));
                }

                FileCabinetRecordGenerator fileCabinetRecordGenerator = new FileCabinetRecordGenerator(startId);

                for (int i = 0; i < amount; i++)
                {
                    list.Add(fileCabinetRecordGenerator.Generate());
                }
            }
        }
    }
}
