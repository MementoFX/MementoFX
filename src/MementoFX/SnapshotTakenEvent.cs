using Memento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento
{
    /// <summary>
    /// Defines an event representing the snapshot of an aggregate
    /// </summary>
    public class SnapshotTakenEvent : DomainEvent
    {
        /// <summary>
        /// Gets or sets the shapshot
        /// </summary>
        public IAggregateMemento Memento { get; private set; }

        /// <summary>
        /// The aggregate's full CLR type name
        /// </summary>
        public string AggregateFullTypeName { get; set; }

        /// <summary>
        /// Creates a snapshot instance
        /// </summary>
        /// <param name="memento">The aggregate instance to be used as the snapshot</param>
        public SnapshotTakenEvent(IAggregateMemento memento)
        {
            Memento = memento ?? throw new ArgumentNullException(nameof(memento));
            AggregateFullTypeName = memento.AggregateFullTypeName;
        }
    }
}
