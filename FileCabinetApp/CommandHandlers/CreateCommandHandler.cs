using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.Validation;

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        public CreateCommandHandler(IFIleCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "create")
            {
                Create(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Create(string parameters)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            FileCabinetRecord record = null;

            if (Program.validator is DefaultValidation)
            {
                record = new DefaultValidation().ValidateParametersProgram();
            }
            else
            {
                record = new CustomValidation().ValidateParametersProgram();
            }

            if (service.CreateRecord(record) == -1)
            {
                Console.WriteLine("An error occured creating the record.");
            }
            else
            {
                var recordsCount = service.GetStat(out int deletedCount);
                Console.WriteLine($"Record #{recordsCount} created.");
            }
        }
    }
}
