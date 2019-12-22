using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFIleCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(request.Command, StringComparison.InvariantCultureIgnoreCase));
            if (index < 0)
            {
                Console.WriteLine($"There is no '{request.Command}' command.");
                Console.WriteLine();
            }

            if (request.Command.ToLower(new CultureInfo("en-US")) == "purge")
            {
                Purge(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Purge(string parameters)
        {
            if (service is FileCabinetFilesystemService)
            {
                service.Purge(out int count, out int before);
                Console.WriteLine($"Data file processing is complited:  {count} of {before} records were purged.");
            }
            else
            {
                Console.WriteLine("This command cannot be executed in the current storage type.");
            }
        }
    }
}
