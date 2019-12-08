using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        public bool ValidateParameters(FileCabinetRecord record);

        public FileCabinetRecord ValidateParametersProgram();
    }
}
