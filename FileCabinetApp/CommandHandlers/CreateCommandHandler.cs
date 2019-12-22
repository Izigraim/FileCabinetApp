using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.Validation;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Create command class.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public CreateCommandHandler(IFIleCabinetService service)
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

            if (request.Command.ToLower(new CultureInfo("en-US")) == "create")
            {
                Create();
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Create()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            FileCabinetRecord record = Program.Validator.ValidateParametersProgram();

            if (Service.CreateRecord(record) == -1)
            {
                Console.WriteLine("An error occured creating the record.");
            }
            else
            {
                var recordsCount = Service.GetStat(out int deletedCount);
                Console.WriteLine($"Record #{recordsCount} created.");
            }
        }
    }
}
