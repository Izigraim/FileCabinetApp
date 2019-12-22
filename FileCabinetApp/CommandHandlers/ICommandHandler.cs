using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// CommandHandler interface.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Set next command.
        /// </summary>
        /// <param name="nexHandler">Next.</param>
        /// <returns>ICommandHandler.</returns>
        public ICommandHandler SetNext(ICommandHandler nexHandler);

        /// <summary>
        /// Handle a request.
        /// </summary>
        /// <param name="request">Request.</param>
        public void Handle(AppCommandRequest request);
    }
}
