using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public interface ICommandHandler
    {
        public ICommandHandler SetNext(ICommandHandler nexHandler);

        public void Handle(AppCommandRequest request);
    }
}
