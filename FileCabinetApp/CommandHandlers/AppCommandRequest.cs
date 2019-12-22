using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Request class.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Name of command.</param>
        /// <param name="parameters">Command's parameters.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets a command.
        /// </summary>
        /// <value>
        /// A command.
        /// </value>
        public string Command { get; }

        /// <summary>
        /// Gets parameters.
        /// </summary>
        /// <value>
        /// Parameters.
        /// </value>
        public string Parameters { get; }
    }
}
