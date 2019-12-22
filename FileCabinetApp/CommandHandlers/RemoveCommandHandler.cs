using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        public RemoveCommandHandler(IFIleCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "remove")
            {
                Remove(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Remove(string parameters)
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
                service.Remove(id - 1);
                Console.WriteLine($"Record #{id} is removed.");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
            }
        }
    }
}
