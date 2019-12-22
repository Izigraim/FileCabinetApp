using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Stat command.
    /// </summary>
    public class StatCommanHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommanHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public StatCommanHandler(IFIleCabinetService service)
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

            if (request.Command.ToLower(new CultureInfo("en-US")) == "stat")
            {
                Stat();
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Stat()
        {
            var recordsCount = Service.GetStat(out int deletedCount);
            Console.WriteLine($"{recordsCount} record(s).\n{deletedCount} records are deleted.");
        }
    }
}
