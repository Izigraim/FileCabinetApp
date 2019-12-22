using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Exit command.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private static Action<bool> isRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="running">Application state.</param>
        public ExitCommandHandler(Action<bool> running)
        {
            isRunning = running;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command.ToLower(new CultureInfo("en-US")) == "exit")
            {
                Exit();
            }
            else
            {
                base.Handle(request);
            }
        }

        private static void Exit()
        {
            Console.WriteLine("Exiting an application...");
            isRunning(false);
        }
    }
}
