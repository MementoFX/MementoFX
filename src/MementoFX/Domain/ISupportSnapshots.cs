using System;
using System.Collections.Generic;
using System.Text;

namespace Memento.Domain
{
    /// <summary>
    /// Defines an aggregate supporting snapshots
    /// </summary>
    public interface ISupportSnapshots
    {
        /// <summary>
        /// Creates the memento needed by an aggregate snapshot
        /// </summary>
        /// <returns>The memento</returns>
        IAggregateMemento CreateMemento();

        /// <summary>
        /// Restore an aggregate instance given a memento
        /// </summary>
        /// <param name="memento">The memento to be used to restore the aggregate</param>
        void Restore(IAggregateMemento memento);
    }
}
