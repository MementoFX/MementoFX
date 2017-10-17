using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;
using MementoFX.Domain;

namespace MementoFX.Persistence
{
    /// <summary>
    /// Provides the base implementation for a Repository
    /// </summary>
    public class Repository : IRepository
    {
        private static IDictionary<Type, IEnumerable<EventMapping>> ReplayableEventsDescriptor = new Dictionary<Type, IEnumerable<EventMapping>>();
        /// <summary>
        /// Gets the event store
        /// </summary>
        public IEventStore EventStore { get; private set; }

        /// <summary>
        /// Creates an instance of the Repository
        /// </summary>
        /// <param name="eventStore">The event store used by the repository to retrieve events</param>
        public Repository(IEventStore eventStore)
        {
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));
            EventStore = eventStore;
        }

        private IEnumerable<EventMapping> BuildReplayableEventsDescriptorByAggregate<T>() where T : IAggregate
        {
            var aggregateType = typeof(T);
            if (ReplayableEventsDescriptor.ContainsKey(aggregateType))
            {
                return ReplayableEventsDescriptor[aggregateType];
            }
            else
            {
                var applyMethods = aggregateType.GetMethods()
                                            .Where(m => m.Name == "ApplyEvent");
                var methodParameters = applyMethods.SelectMany(e => e.GetParameters())
                                            .Distinct();

                var eventDescriptors = (from p in methodParameters
                                        let parameterType = p.ParameterType
                                        select new EventMapping { EventType = parameterType, ApplyMethodParameter = p }).ToList();

                foreach (var e in eventDescriptors)
                {
                    var attribute = e.ApplyMethodParameter.GetCustomAttribute<AggregateIdAttribute>();
                    if (attribute == null)
                    {
                        e.AggregateIdPropertyName = aggregateType.Name + "Id";
                    }
                    else
                    {
                        e.AggregateIdPropertyName = attribute.PropertyName;
                    }
                }
                ReplayableEventsDescriptor.Add(aggregateType, eventDescriptors);

                return eventDescriptors;
            }

        }

        #region IRepository implementation

        private T _GetById<T>(Guid id, DateTime pointInTime) where T : IAggregate
        {
            IEnumerable<DomainEvent> events = null;
            dynamic aggregateInstance = FormatterServices.GetUninitializedObject(typeof(T));
            var eventDescriptors = BuildReplayableEventsDescriptorByAggregate<T>();
            events = EventStore.RetrieveEvents(id, pointInTime.ToUniversalTime(), eventDescriptors, null);
            (aggregateInstance as IAggregate).ReplayEvents(events);
            return (T)aggregateInstance;
        }

        /// <summary>
        /// Retrieves an aggregate instance
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <returns>The aggregate instance</returns>
        public T GetById<T>(Guid id) where T : IAggregate
        {
            var aggregateInstance = _GetById<T>(id, DateTime.UtcNow);
            (aggregateInstance as Aggregate).IsTimeTravelling = false;
            return aggregateInstance;
        }

        /// <summary>
        /// Retrieve an aggregate instance restoring the value it had at a given point in time
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <param name="pointInTime">A point in time</param>
        /// <returns>The aggregate instance</returns>
        public T GetById<T>(Guid id, DateTime pointInTime) where T : IAggregate
        {
            var aggregateInstance = _GetById<T>(id, pointInTime.ToUniversalTime());
            (aggregateInstance as Aggregate).IsTimeTravelling = true;
            return aggregateInstance;
        }

        private T _GetById<T>(Guid id, Guid timelineId, DateTime pointInTime) where T : IAggregate
        {
            IEnumerable<DomainEvent> events = null;
            dynamic aggregateInstance = FormatterServices.GetUninitializedObject(typeof(T));
            var eventDescriptors = BuildReplayableEventsDescriptorByAggregate<T>();
            events = EventStore.RetrieveEvents(id, pointInTime.ToUniversalTime(), eventDescriptors, timelineId);
            (aggregateInstance as IAggregate).ReplayEvents(events);
            (aggregateInstance as Aggregate).timelineId = timelineId;
            return (T)aggregateInstance;
        }

        /// <summary>
        /// Retrieve an aggregate instance restoring the value it would have in a given point in time happening along a timeline
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <param name="timelineId">The timeline Id</param>
        /// <returns>The aggregate instance</returns>
        public T GetById<T>(Guid id, Guid timelineId) where T : IAggregate
        {
            var aggregateInstance = _GetById<T>(id, timelineId, DateTime.Now);
            (aggregateInstance as Aggregate).IsTimeTravelling = false;
            return aggregateInstance;
        }

        /// <summary>
        /// Retrieve an aggregate instance restoring the value it would have in a given point in time happening along a timeline
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <param name="timelineId">The timeline Id</param>
        /// <param name="pointInTime">A point in time</param>
        /// <returns>The aggregate instance</returns>
        public T GetById<T>(Guid id, Guid timelineId, DateTime pointInTime) where T : IAggregate
        {
            var aggregateInstance = _GetById<T>(id, timelineId, pointInTime.ToUniversalTime());
            (aggregateInstance as Aggregate).IsTimeTravelling = true;
            return aggregateInstance;
        }

        /// <summary>
        /// Retrieve an aggregate instance restoring the value it had at a given point in time
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <param name="pointsInTime">The list of dates to use to produce the series</param>
        /// <returns>The aggregate instance</returns>
        public T[] GetSeriesById<T>(Guid id, IEnumerable<DateTime> pointsInTime) where T : IAggregate
        {
            var lastPointInTime = pointsInTime.Max();
            IEnumerable<DomainEvent> events = null;
            dynamic aggregateInstance = FormatterServices.GetUninitializedObject(typeof(T));
            var eventDescriptors = BuildReplayableEventsDescriptorByAggregate<T>();
            events = EventStore.RetrieveEvents(id, lastPointInTime.ToUniversalTime(), eventDescriptors, null);
            IList<T> aggregates = new List<T>();
            foreach(var date in pointsInTime.OrderBy(d => d))
            {
                dynamic aggregate = FormatterServices.GetUninitializedObject(typeof(T));
                var evs = events.Where(e => e.TimeStamp <= date);
                (aggregate as Aggregate).IsTimeTravelling = true;
                (aggregate as IAggregate).ReplayEvents(evs);
                aggregates.Add((T) aggregate);
            }
            return aggregates.ToArray();
        }


        /// <summary>
        /// Saves an aggregate instance
        /// </summary>
        /// <typeparam name="T">The aggregate's type</typeparam>
        /// <param name="item">The aggregate instance</param>
        public void Save<T>(T item) where T : IAggregate
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item as Aggregate != null && (item as Aggregate).IsTimeTravelling)
                throw new ArgumentException("Can't save a time travelling instance.", nameof(item));

            var uncommittedEvents = item.GetUncommittedEvents().ToArray();
            foreach (var @event in uncommittedEvents)
            {
                EventStore.Save(@event);
                item.MarkEventAsSaved(@event);
            }
        }

        /// <summary>
        /// Saves an aggregate instance
        /// </summary>
        /// <typeparam name="T">The aggregate's type</typeparam>
        /// <param name="item">The aggregate instance</param>
        public async Task SaveAsync<T>(T item) where T : IAggregate
        {
            await Task.Run(() => Save(item));
        }
        #endregion
    }
}
