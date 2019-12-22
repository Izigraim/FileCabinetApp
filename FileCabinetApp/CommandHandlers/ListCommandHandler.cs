using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// List command.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        /// <param name="printer">Type of printing.</param>
        public ListCommandHandler(IFIleCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command.ToLower(new CultureInfo("en-US")) == "list")
            {
                this.List();
            }
            else
            {
                base.Handle(request);
            }
        }

        private void List()
        {
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = Service.GetRecords();
            if (fileCabinetRecords.Count == 0)
            {
                Console.WriteLine("There are 0 records.");
                return;
            }

            this.printer(fileCabinetRecords);
        }
    }
}
