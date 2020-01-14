using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Iterator for worjing with <see cref="FileCabinetMemoryService"/>.
    /// </summary>
    public class MemoryIteratorCollection : IEnumerable<FileCabinetRecord>
    {
        private List<FileCabinetRecord> records = new List<FileCabinetRecord>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIteratorCollection"/> class.
        /// </summary>
        /// <param name="records">List of records.</param>
        public MemoryIteratorCollection(List<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>
        /// Get enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> object that can be used to iterate through collection.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new MemoryIteratorEnumerator(this.records);
        }

        /// <summary>
        /// Get enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> object that can be used to iterate through collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MemoryIteratorEnumerator(this.records);
        }

        /// <summary>
        /// Enumerator class.
        /// </summary>
        public sealed class MemoryIteratorEnumerator : IEnumerator<FileCabinetRecord>
        {
            private List<FileCabinetRecord> list = new List<FileCabinetRecord>();
            private int currentIndex;

            /// <summary>
            /// Initializes a new instance of the <see cref="MemoryIteratorEnumerator"/> class.
            /// </summary>
            /// <param name="list">List of records.</param>
            public MemoryIteratorEnumerator(List<FileCabinetRecord> list)
            {
                this.list = list;
                this.currentIndex = -1;
            }

            /// <summary>
            /// Gets current element.
            /// </summary>
            /// <value>
            /// Current element.
            /// </value>
            public FileCabinetRecord Current
            {
                get
                {
                    if (this.currentIndex == -1 || this.currentIndex == this.list.Count)
                    {
                        throw new InvalidOperationException();
                    }

                    return this.list[this.currentIndex];
                }
            }

            /// <summary>
            /// Gets current element.
            /// </summary>
            /// <value>
            /// Current element.
            /// </value>
            object IEnumerator.Current
            {
                get
                {
                    if (this.currentIndex == -1 || this.currentIndex == this.list.Count)
                    {
                        throw new InvalidOperationException();
                    }

                    return this.list[this.currentIndex];
                }
            }

            /// <summary>
            /// Implement IDisposable.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Checking whether we can go to the next item in the collection.
            /// </summary>
            /// <returns>True, if we can, false - if not.</returns>
            public bool MoveNext()
            {
                if (this.currentIndex < this.list.Count - 1)
                {
                    this.currentIndex++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Reset position in collection.
            /// </summary>
            public void Reset()
            {
                this.currentIndex = -1;
            }
        }
    }
}
