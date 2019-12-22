using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : CommandHandlerBase
    {
        private static IFIleCabinetService service;

        public FindCommandHandler(IFIleCabinetService service1)
        {
            service = service1;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "find")
            {
                Find(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Find(string parameters)
        {
            try
            {
                if (parameters.Where(c => c == ' ').Count() > 1 || !parameters.Where(c => c == ' ').Any() || parameters[0] == ' ')
                {
                    throw new ArgumentException("Incorrect command format.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            string[] findParameters = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ReadOnlyCollection<FileCabinetRecord> findedRecords = null;

            switch (findParameters[0].ToLower(CultureInfo.CreateSpecificCulture("en-US")))
            {
                case "firstname":
                    findedRecords = service.FindByFirstName(findParameters[1].Trim('"'));
                    break;
                case "lastname":
                    findedRecords = service.FindByLastName(findParameters[1].Trim('"'));
                    break;
                case "dateofbirth":
                    findedRecords = service.FindByDateOfBirth(findParameters[1].Trim('"'));
                    break;
            }

            foreach (FileCabinetRecord record in findedRecords)
            {
                Console.WriteLine($"#{record.Id + 1}, {record.Sex}, {record.FirstName}, {record.LastName}, {record.Age}, {record.Salary}, {record.DateOfBirth:yyyy-MMM-dd}");
            }
        }
    }
}
