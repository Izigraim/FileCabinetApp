using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base class for command handler.
    /// </summary>
    public class CommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// CommandHelpIndex.
        /// </summary>
        protected const int CommandHelpIndex = 0;

        /// <summary>
        /// Description index.
        /// </summary>
        protected const int DescriptionHelpIndex = 1;

        /// <summary>
        /// Explanation index.
        /// </summary>
        protected const int ExplanationHelpIndex = 2;

        private ICommandHandler nextHandler;

        /// <inheritdoc/>
        public virtual void Handle(AppCommandRequest request)
        {
            if (this.nextHandler != null)
            {
                this.nextHandler.Handle(request);
            }
            else
            {
                return;
            }
        }

        /// <inheritdoc/>
        public ICommandHandler SetNext(ICommandHandler nextHandler)
        {
            this.nextHandler = nextHandler;
            return nextHandler;
        }
    }
}
