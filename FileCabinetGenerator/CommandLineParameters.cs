using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetGenerator
{
    public static class CommandLineParameters
    {
        public static bool GeneratorValidation(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length == 4)
            {
                string[] outputType = args[0].Split('=');
                string[] outputFile = args[1].Split('=');
                string[] recordsAmount = args[2].Split('=');
                string[] startId = args[3].Split('=');

                try
                {
                    if (outputType[0].ToLower(new CultureInfo("en-US")) != "--output-type" || (outputType[1].ToLower(new CultureInfo("en-US")) != "csv" && outputType[1].ToLower(new CultureInfo("en-US")) != "xml"))
                    {
                        throw new ArgumentException("Incorrect parameter 'output-type'");
                    }
                    else if (outputFile[0].ToLower(new CultureInfo("en-US")) != "--output")
                    {
                        throw new ArgumentException("Incorrect parameter 'output'");
                    }
                    else if (recordsAmount[0].ToLower(new CultureInfo("en-US")) != "--records-amount" || Convert.ToInt32(recordsAmount[1], new CultureInfo("en-US")) < 0)
                    {
                        throw new ArgumentException("Incorrect parameter 'records-amount'");
                    }
                    else if (startId[0].ToLower(new CultureInfo("en-US")) != "--start-id" || Convert.ToInt32(startId[1], new CultureInfo("en-US")) < 0)
                    {
                        throw new ArgumentException("Incorrect parameter 'start-id'");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else if (args.Length == 8)
            {
                try
                {
                    if (args[0].ToLower(new CultureInfo("en-US")) != "-t" || (args[1].ToLower(new CultureInfo("en-US")) != "csv" && args[1].ToLower(new CultureInfo("en-US")) != "xml"))
                    {
                        throw new ArgumentException("Incorrect parameter 'output-type'");
                    }
                    else if (args[2].ToLower(new CultureInfo("en-US")) != "-o")
                    {
                        throw new ArgumentException("Incorrect parameter 'output'");
                    }
                    else if (args[4].ToLower(new CultureInfo("en-US")) != "-a" || Convert.ToInt32(args[5], new CultureInfo("en-US")) < 0)
                    {
                        throw new ArgumentException("Incorrect parameter 'records-amount'");
                    }
                    else if (args[6].ToLower(new CultureInfo("en-US")) != "-i" || Convert.ToInt32(args[7], new CultureInfo("en-US")) < 0)
                    {
                        throw new ArgumentException("Incorrect parameter 'start-id'");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Incorrect parameters.");
                return false;
            }

            return true;
        }
    }
}
