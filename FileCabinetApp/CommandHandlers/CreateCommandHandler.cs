using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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

            FileCabinetRecord record = Program.validator.ValidateParametersProgram();

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
