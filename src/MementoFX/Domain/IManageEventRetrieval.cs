using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento.Persistence;

namespace Memento.Domain
{
    /// <summary>
    /// Defines an aggregate capable of self retrieving needed events
    /// </summary>
    public interface IManageEventRetrieval
    {
        /// <summary>
        /// Retrieves the events needed to properly setup an aggregate instance
        /// </summary>
        /// <param name="eventStore">The store form wich to retrieve the events</param>
        /// <param name="aggregateId">The aggregate id</param>
        /// <param name="timelineId">The timeline id</param>
        /// <param name="pointInTime">The point in time</param>
        /// <returns>A list of events</returns>
        IEnumerable<DomainEvent> RetrieveEvents(IEventStore eventStore, Guid aggregateId, Guid? timelineId, DateTime? pointInTime);
    }
}
