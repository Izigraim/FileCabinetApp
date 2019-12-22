using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private static Action<bool> isRunning;

        public ExitCommandHandler(Action<bool> running)
        {
            isRunning = running;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "exit")
            {
                Exit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning(false);
        }
    }
}
