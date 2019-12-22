using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Validation;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Edit command.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public EditCommandHandler(IFIleCabinetService service)
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
                FileCabinetRecord record = Program.Validator.ValidateParametersProgram();

                record.Id = id - 1;

                Service.EditRecord(record);
                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
            }
        }
    }
}
