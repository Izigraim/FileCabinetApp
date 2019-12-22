using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Validation;

namespace FileCabinetApp.CommandHandlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        public EditCommandHandler(IFIleCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "edit")
            {
                Edit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }

        }

        private static void Edit(string parameters)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            int id;
            try
            {
                id = Convert.ToInt32(parameters, culture);
                if (id > service.GetStat(out int deletedCount) || id <= 0)
                {
                    Console.WriteLine($"#{id} record is not found.");
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Incorrext format of ID");
                return;
            }

            if (service.GetRecords().Where(c => c.Id == id - 1).Any())
            {
                FileCabinetRecord record = null;

                if (Program.validator is DefaultValidation)
                {
                    record = new DefaultValidation().ValidateParametersProgram();
                }
                else
                {
                    record = new CustomValidation().ValidateParametersProgram();
                }

                record.Id = id - 1;

                service.EditRecord(record);
                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
            }
        }
    }
}
