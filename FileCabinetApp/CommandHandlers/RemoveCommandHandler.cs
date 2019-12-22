using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Remove command.
    /// </summary>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public RemoveCommandHandler(IFIleCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

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
                if (id > Service.GetStat(out int deletedCount) || id <= 0)
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

            if (Service.GetRecords().Where(c => c.Id == id - 1).Any())
            {
                Service.Remove(id - 1);
                Console.WriteLine($"Record #{id} is removed.");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
            }
        }
    }
}
