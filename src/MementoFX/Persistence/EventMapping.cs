using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX.Persistence
{
    /// <summary>
    /// Defines a domain event descriptor used by a repository
    /// to retrieve events from the store 
    /// </summary>
    public class EventMapping
    {
        /// <summary>
        /// The type of the event
        /// </summary>
        public Type EventType;

        /// <summary>
        /// The descriptor of the method accepting the event as its parameter
        /// </summary>
        internal ParameterInfo ApplyMethodParameter;

        /// <summary>
        /// The name of the event property to be used as the aggregate Id
        /// </summary>
        public string AggregateIdPropertyName;
    }
}
