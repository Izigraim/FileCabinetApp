using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// ServiceCommandHandler base class.
    /// </summary>
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        private static IFIleCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service1">Type of services.</param>
        public ServiceCommandHandlerBase(IFIleCabinetService service1)
        {
            service = service1;
        }

        /// <summary>
        /// Gets a service.
        /// </summary>
        /// <value>
        /// A service.
        /// </value>
        public static IFIleCabinetService Service
        {
            get
            {
                return service;
            }
        }
    }
}
