using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected static IFIleCabinetService service;

        public ServiceCommandHandlerBase(IFIleCabinetService service1)
        {
            service = service1;
        }
    }
}
