using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Messaging
{
    /// <summary>
    /// Provides the definition of a Memento event dispatcher
    /// </summary>
    public interface IAsyncEventDispatcher
    {
        /// <summary>
        /// Dispatches an event
        /// </summary>
        /// <typeparam name="T">The type of the event</typeparam>
        /// <param name="event">The event to dispatch</param>
        Task Dispatch<T>(T @event) where T : DomainEvent;
    }
}
