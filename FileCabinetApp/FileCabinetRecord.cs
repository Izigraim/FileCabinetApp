using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Record.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        /// <value>
        /// ID.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Sex.
        /// </summary>
        /// <value>
        /// Sex.
        /// </value>
        public char Sex { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        /// <value>
        /// FirstName.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        /// <value>
        /// LastName.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets Age.
        /// </summary>
        /// <value>
        /// Age.
        /// </value>
        public short Age { get; set; }

        /// <summary>
        /// Gets or sets Salary.
        /// </summary>
        /// <value>
        /// Salary.
        /// </value>
        public decimal? Salary { get; set; }

        /// <summary>
        /// Gets or sets DateOfBirth.
        /// </summary>
        /// <value>
        /// DateOfBirth.
        /// </value>
        public DateTime DateOfBirth { get; set; }
    }
}
