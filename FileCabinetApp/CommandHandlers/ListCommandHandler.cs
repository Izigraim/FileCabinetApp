using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : CommandHandlerBase
    {
        private static IFIleCabinetService service;

        public ListCommandHandler(IFIleCabinetService service1)
        {
            service = service1;
        }
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "list")
            {
                List(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = service.GetRecords();
            if (fileCabinetRecords.Count == 0)
            {
                Console.WriteLine("There are 0 records.");
                return;
            }

            for (int i = 0; i < fileCabinetRecords.Count; i++)
            {
                Console.WriteLine($"#{fileCabinetRecords[i].Id + 1}, {fileCabinetRecords[i].Sex}, {fileCabinetRecords[i].FirstName}, {fileCabinetRecords[i].LastName}, {fileCabinetRecords[i].Age}, {fileCabinetRecords[i].Salary}, {fileCabinetRecords[i].DateOfBirth:yyyy-MMM-dd}");
            }
        }
    }
}
