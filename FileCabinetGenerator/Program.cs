using System;
using System.Globalization;
using System.IO;

namespace FileCabinetGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CommandLineParameters.GeneratorValidation(args);
        }
    }
}
