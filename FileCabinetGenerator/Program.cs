using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class with main method.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Tuple<bool, int, int, string, string> tuple = CommandLineParameters.GeneratorValidation(args);
            if (tuple.Item1)
            {
                List<FileCabinetRecord> list = new List<FileCabinetRecord>();

                FileCabinetRecordGenerator fileCabinetRecordGenerator = new FileCabinetRecordGenerator(tuple.Item2);

                for (int i = 0; i < tuple.Item3; i++)
                {
                    list.Add(fileCabinetRecordGenerator.Generate());
                }

                if (tuple.Item5.ToLower(new CultureInfo("en-US")) == "csv")
                {
                    string path = tuple.Item4;

                    if (!path.Contains(".csv", StringComparison.Ordinal))
                    {
                        path += ".csv";
                    }

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Create)))
                        {
                            RecordGeneratorCsvWriter csvWriter = new RecordGeneratorCsvWriter(writer);
                            csvWriter.Write(list);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (tuple.Item5.ToLower(new CultureInfo("en-US")) == "xml")
                {
                    string path = tuple.Item4;

                    if (!path.Contains(".xml", StringComparison.Ordinal))
                    {
                        path += ".xml";
                    }

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Create)))
                        {
                            RecordGeneratorXmlWriter csvWriter = new RecordGeneratorXmlWriter(writer);
                            csvWriter.Write(list.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
