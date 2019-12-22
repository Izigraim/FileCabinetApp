using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        // private IRecordPrinter printer;
        private Action<IEnumerable<FileCabinetRecord>> printer;

        public ListCommandHandler(IFIleCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower(new CultureInfo("en-US")) == "list")
            {
                this.List(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = service.GetRecords();
            if (fileCabinetRecords.Count == 0)
            {
                Console.WriteLine("There are 0 records.");
                return;
            }

            this.printer(fileCabinetRecords);
        }
    }
}
