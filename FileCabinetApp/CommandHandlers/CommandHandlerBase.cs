using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        protected const int CommandHelpIndex = 0;
        protected const int DescriptionHelpIndex = 1;
        protected const int ExplanationHelpIndex = 2;

        protected static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the count of records", "The 'stat' command prints the count of records." },
            new string[] { "create", "create a record", "The 'create' command create a record." },
            new string[] { "list", "returns list of records", "The 'list' command returns list of records." },
            new string[] { "edit", "edit a record", "The 'edit' command edit a record" },
            new string[] { "find", "find a record or records by property", "The 'find' command find a record or records by property." },
            new string[] { "export", "export data to file", "The 'export' command export a records to file" },
            new string[] { "import", "import data from file", "The 'import' command import records from file" },
            new string[] { "remove", "remove a record", "The 'remove' command remove a record with selected ID." },
            new string[] { "purge", "purge a file with records", "The 'purge' command remove records marked as deleted from file." },
        };

        protected static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
        };

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

        public ICommandHandler SetNext(ICommandHandler nextHandler)
        {
            this.nextHandler = nextHandler;
            return nextHandler;
        }

        private static void PrintHelp(string parameters) { }

        private static void Exit(string parameters) { }

        private static void Stat(string parameters) { }

        private static void Create(string parameters) { }

        private static void List(string parameters) { }

        private static void Edit(string parameters) { }

        private static void Find(string parameters) { }

        private static void Export(string parameters) { }

        private static void Import(string parameters) { }

        private static void Remove(string parameters) { }

        private static void Purge(string parameters) { }

    }
}
