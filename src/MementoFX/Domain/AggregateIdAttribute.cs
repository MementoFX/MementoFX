using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Domain
{
    /// <summary>
    /// Specifies the name of the event's property to be used as the aggregate id
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class AggregateIdAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the Property Name
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Creates an instance of the attribute
        /// </summary>
        /// <param name="propertyName">The name of the property to be used as the aggregate id</param>
        public AggregateIdAttribute(string propertyName)
        {
            if(string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("The propertyName parameter cannot be null or blank", nameof(propertyName));
            if (propertyName.Contains(" "))
                throw new ArgumentException("The propertyName parameter cannot contain whitespaces", nameof(propertyName));

            PropertyName = propertyName;
        }
    }
}
