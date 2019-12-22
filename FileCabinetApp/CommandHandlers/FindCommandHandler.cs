using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Find command.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Type of services.</param>
        /// <param name="printer">Type of printing.</param>
        public FindCommandHandler(IFIleCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
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

            if (request.Command.ToLower(new CultureInfo("en-US")) == "find")
            {
                this.Find(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Find(string parameters)
        {
            try
            {
                if (parameters.Where(c => c == ' ').Count() > 1 || !parameters.Where(c => c == ' ').Any() || parameters[0] == ' ')
                {
                    throw new ArgumentException("Incorrect command format.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            string[] findParameters = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ReadOnlyCollection<FileCabinetRecord> findedRecords = null;

            switch (findParameters[0].ToLower(CultureInfo.CreateSpecificCulture("en-US")))
            {
                case "firstname":
                    findedRecords = Service.FindByFirstName(findParameters[1].Trim('"'));
                    break;
                case "lastname":
                    findedRecords = Service.FindByLastName(findParameters[1].Trim('"'));
                    break;
                case "dateofbirth":
                    findedRecords = Service.FindByDateOfBirth(findParameters[1].Trim('"'));
                    break;
            }

            this.printer(findedRecords);
        }
    }
}
