using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        public virtual void Handle(AppCommandRequest request)
        {
        }

        public void SetNext(ICommandHandler nextHandler)
        {
            this.nextHandler = nextHandler;
        }
    }
}
