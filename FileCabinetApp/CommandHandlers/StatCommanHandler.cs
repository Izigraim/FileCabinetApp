using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommanHandler : ServiceCommandHandlerBase
    {
        public StatCommanHandler(IFIleCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "stat")
            {
                Stat(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Stat(string parameters)
        {
            var recordsCount = service.GetStat(out int deletedCount);
            Console.WriteLine($"{recordsCount} record(s).\n{deletedCount} records are deleted.");
        }
    }
}
