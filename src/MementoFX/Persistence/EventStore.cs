using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Memento.Domain;
using Memento.Messaging;

namespace Memento.Persistence
{
    /// <summary>
    /// Provides the base implementation for an EventStore
    /// </summary>
    public abstract class EventStore : IEventStore
    {
        private static Dictionary<Type, PropertyInfo> domainEventTypes = new Dictionary<Type, PropertyInfo>();

        private IEventDispatcher EventDispatcher { get; set; }

        /// <summary>
        /// Initializes a new instance of Memento.Persistence.EventStore class.
        /// </summary>
        /// <param name="eventDispatcher"></param>
        public EventStore(IEventDispatcher eventDispatcher)
        {
            if (eventDispatcher == null)
                throw new ArgumentNullException(nameof(eventDispatcher));

            EventDispatcher = eventDispatcher;
        }

        /// <summary>
        /// Retrieves all events of a type which satisfy a condition
        /// </summary>
        /// <typeparam name="T">The type of the events to retrieve</typeparam>
        /// <param name="filter">The condition events must satisfy in order to be retrieved.</param>
        /// <returns>The events which satisfy the specified condition</returns>
        public abstract IEnumerable<T> Find<T>(Func<T, bool> filter) where T : DomainEvent;

        /// <summary>
        /// Retrieves the desired events from the store
        /// </summary>
        /// <param name="aggregateId">The aggregate id</param>
        /// <param name="pointInTime">The point in time up to which the events have to be retrieved</param>
        /// <param name="eventDescriptors">The descriptors defining the events to be retrieved</param>
        /// <param name="timelineId">The id of the timeline from which to retrieve the events</param>
        /// <returns>The list of the retrieved events</returns>
        public abstract IEnumerable<DomainEvent> RetrieveEvents(Guid aggregateId, DateTime pointInTime, IEnumerable<EventMapping> eventDescriptors, Guid? timelineId);

        /// <summary>
        /// Saves an event within the store
        /// </summary>
        /// <param name="event">The event</param>
        public async Task SaveAsync(DomainEvent @event)
        {
            await Task.Run(() => Save(@event));
        }

        /// <summary>
        /// Saves an event within the store
        /// </summary>
        /// <param name="event">The event</param>
        public void Save(DomainEvent @event) 
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            ManageTimestamp(@event);
            _Save(@event);
            EventDispatcher.Dispatch(@event);
        }

        /// <summary>
        /// Stores the event
        /// </summary>
        /// <param name="event">The event</param>
        protected abstract void _Save(DomainEvent @event);

        internal static void ManageTimestamp(DomainEvent @event)
        {
            PropertyInfo timestampProperty = null;
            var eventType = @event.GetType();
            if(!domainEventTypes.ContainsKey(eventType))
            {
                lock(domainEventTypes)
                {
                    timestampProperty = eventType
                        .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.PropertyType == typeof(DateTime))
                        .Where(p => Attribute.IsDefined(p, typeof(TimestampAttribute)))
                        .SingleOrDefault();
                    domainEventTypes[eventType] = timestampProperty;
                }
            }
            else
            {
                timestampProperty = domainEventTypes[eventType];
            }
            if (timestampProperty != null)
            {
                @event.TimeStamp = (DateTime)timestampProperty.GetValue(@event);
            }
        }
    }
}
