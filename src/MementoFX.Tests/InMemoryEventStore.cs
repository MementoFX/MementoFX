using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX.Domain;
using MementoFX.Persistence;
using MementoFX.Messaging;
using EventCondition = System.Linq.Expressions.Expression<System.Func<MementoFX.Persistence.EventMapping, bool>>;

namespace MementoFX.Persistence.InMemory
{
    /// <summary>
    /// Provides an implementation of a Memento event store
    /// using memory as the storage
    /// </summary>
    public class InMemoryEventStore : EventStore
    {
        private static IList<DomainEvent> Events = new List<DomainEvent>();

        /// <summary>
        /// Creates a new instance of the event store
        /// </summary>
        /// <param name="eventDispatcher">The event dispatcher to be used by the instance</param>
        public InMemoryEventStore(IEventDispatcher eventDispatcher) : base(eventDispatcher) { }

        /// <summary>
        /// Retrieves all events of a type which satisfy a requirement
        /// </summary>
        /// <typeparam name="T">The type of the event</typeparam>
        /// <param name="filter">The requirement</param>
        /// <returns>The events which satisfy the given requirement</returns>
        public override IEnumerable<T> Find<T>(Func<T, bool> filter)
        {
            return Events.OfType<T>().Where(filter);
        }

        /// <summary>
        /// Retrieves the desired events from the store
        /// </summary>
        /// <param name="aggregateId">The aggregate id</param>
        /// <param name="pointInTime">The point in time up to which the events have to be retrieved</param>
        /// <param name="eventDescriptors">The descriptors defining the events to be retrieved</param>
        /// <param name="timelineId">The id of the timeline from which to retrieve the events</param>
        /// <returns>The list of the retrieved events</returns>
        public override IEnumerable<DomainEvent> RetrieveEvents(Guid aggregateId, DateTime pointInTime, IEnumerable<EventMapping> eventDescriptors, Guid? timelineId)
        {
            IEnumerable<DomainEvent> query = Events;
            if (!timelineId.HasValue)
                query = query.Where(e => e.TimeStamp <= pointInTime && e.TimelineId == null);
            else
                query = query.Where(e => e.TimeStamp <= pointInTime && (e.TimelineId == null || e.TimelineId.Value == timelineId.Value));

            var events = new List<DomainEvent>();
            foreach (var m in eventDescriptors)
            {
                var buf = from e in query
                          where e.GetType() == m.EventType && 
                                (Guid)e.GetType().GetProperty(m.AggregateIdPropertyName).GetValue(e, null) == aggregateId
                          select e;
                events.AddRange(buf);
            }

            return events.OrderBy(e => e.TimeStamp);
        }

        /// <summary>
        /// Saves an event into the store
        /// </summary>
        /// <param name="event">The event to be saved</param>
        protected override void _Save(DomainEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            Events.Add(@event);
        }
    }
}
