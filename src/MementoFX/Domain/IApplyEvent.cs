using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Domain
{
    /// <summary>
    /// Allows an aggregate to state which events it is interested in
    /// </summary>
    /// <typeparam name="T">The type of the event</typeparam>
    public interface IApplyEvent<T> where T : DomainEvent
    {
        /// <summary>
        /// Alters the aggregate state in order to reflect 
        /// the event's consequences
        /// </summary>
        /// <param name="event">The event to be replayed</param>
        void ApplyEvent(T @event);
    }
}
