using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX
{
    /// <summary>
    /// Provides the base implementation for Memento's domain events
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// Gets or sets the event Id
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Get or set the time at which the event occurred
        /// </summary>
        public DateTime TimeStamp { get; internal set; }

        /// <summary>
        /// Gets or sets the TimelineId
        /// </summary>
        public Guid? TimelineId { get; set; }

        /// <summary>
        /// Creates a DomainEvent instance
        /// </summary>
        public DomainEvent()
        {
            this.Id = Guid.NewGuid();
            this.TimeStamp = DateTime.Now;
        }
    }
}
