using Memento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Persistence
{
    /// <summary>
    /// Represents an event store
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Retrieves all events of a type which satisfy a condition
        /// </summary>
        /// <typeparam name="T">The type of the events to retrieve</typeparam>
        /// <param name="filter">The condition events must satisfy in order to be retrieved.</param>
        /// <returns>The events which satisfy the specified condition</returns>
        IEnumerable<T> Find<T>(Func<T,bool> filter) where T : DomainEvent;

        /// <summary>
        /// Retrieves the desired events from the store
        /// </summary>
        /// <param name="aggregateId">The aggregate id</param>
        /// <param name="pointInTime">The point in time up to which the events have to be retrieved</param>
        /// <param name="eventDescriptors">The descriptors defining the events to be retrieved</param>
        /// <param name="timelineId">The id of the timeline from which to retrieve the events</param>
        /// <returns>The list of the retrieved events</returns>
        IEnumerable<DomainEvent> RetrieveEvents(Guid aggregateId, DateTime pointInTime, IEnumerable<EventMapping> eventDescriptors, Guid? timelineId);

        /// <summary>
        /// Saves an event within the store
        /// </summary>
        /// <param name="event">The event</param>
        void Save(DomainEvent @event);
    }
}
