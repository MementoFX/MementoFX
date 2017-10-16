using System;
using System.Collections.Generic;
using System.Text;

namespace MementoFX.Domain
{
    /// <summary>
    /// Defines a memento storing the state of a MementoFX's aggregate
    /// </summary>
    public interface IAggregateMemento
    {
        /// <summary>
        /// The type name of the aggregate the memento belongs to
        /// </summary>
        string AggregateFullTypeName { get; set; }

        /// <summary>
        /// The list of events to be saved inside a snapshot
        /// </summary>
        IEnumerable<DomainEvent> OccurredEvents { get; }
    }
}
