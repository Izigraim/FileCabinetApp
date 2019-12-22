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
                    findedRecords = Program.fileCabinetService.FindByFirstName(findParameters[1].Trim('"'));
                    break;
                case "lastname":
                    findedRecords = Program.fileCabinetService.FindByLastName(findParameters[1].Trim('"'));
                    break;
                case "dateofbirth":
                    findedRecords = Program.fileCabinetService.FindByDateOfBirth(findParameters[1].Trim('"'));
                    break;
            }

            foreach (FileCabinetRecord record in findedRecords)
            {
                Console.WriteLine($"#{record.Id + 1}, {record.Sex}, {record.FirstName}, {record.LastName}, {record.Age}, {record.Salary}, {record.DateOfBirth:yyyy-MMM-dd}");
            }
        }
    }
}
