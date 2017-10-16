using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX.Domain
{
    /// <summary>
    /// Provides the definition of Memento aggregates interface
    /// </summary>
    public interface IAggregate
    {
        /// <summary>
        /// Gets the aggregate Id
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the id of the timeline the instance belongs to
        /// </summary>
        Guid? TimelineId { get; }

        /// <summary>
        /// Specifies whether the aggregate has pending changes
        /// </summary>
        bool HasPendingChanges { get; }

        /// <summary>
        /// Gets the aggregate's uncommitted events list
        /// </summary>
        /// <returns>The uncommitted events list</returns>
        IEnumerable<DomainEvent> GetUncommittedEvents();

        /// <summary>
        /// Gets or sets the list of the occurred events
        /// </summary>
        IList<DomainEvent> OccurredEvents { get; set; }

        /// <summary>
        /// Clears the aggregate's uncommitted event list
        /// </summary>
        void ClearUncommittedEvents();

        /// <summary>
        /// Marks an uncommitted events as occurred
        /// </summary>
        /// <param name="event">The event to be marked as occurred</param>
        void MarkEventAsSaved(DomainEvent @event);

        /// <summary>
        /// Replay the specified list of events
        /// </summary>
        /// <param name="occurredEvents">The list of events to be replayed</param>
        void ReplayEvents(IEnumerable<DomainEvent> occurredEvents);
    }
}
