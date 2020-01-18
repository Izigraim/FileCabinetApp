using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Purge command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        public PurgeCommandHandler(IFIleCabinetService service)
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

            if (request.Command.ToLower(new CultureInfo("en-US")) == "purge")
            {
                if (request.Parameters == "-h" || request.Parameters == "--help")
                {
                    Console.WriteLine("\t'purge' - remove records marked as deleted from file.\n\tDoes not require any additional parameters.");
                }
                else if (string.IsNullOrEmpty(request.Parameters))
                {
                    Purge();
                }
                else
                {
                    Console.WriteLine("This command does not accept parameters.");
                }
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Purge()
        {
            if (Service is FileCabinetFilesystemService)
            {
                Service.Purge(out int count, out int before);
                Console.WriteLine($"Data file processing is complited:  {count} of {before} records were purged.");
            }
            else
            {
                Console.WriteLine("This command cannot be executed in the current storage type.");
            }
        }
    }
}
