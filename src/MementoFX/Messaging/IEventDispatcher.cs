using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX.Messaging
{
    /// <summary>
    /// Provides the definition of a Memento event dispatcher
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Dispatches an event
        /// </summary>
        /// <typeparam name="T">The type of the event</typeparam>
        /// <param name="event">The event to dispatch</param>
        void Dispatch<T>(T @event) where T : DomainEvent;
    }
}
