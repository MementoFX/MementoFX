using System;
using System.Collections.Generic;
using System.Text;

namespace Memento.Domain
{
    public class AggregateMemento : IAggregateMemento
    {
        public IEnumerable<DomainEvent> OccurredEvents { get; set; }
        public Dictionary<string, object> Values { get; set; }
    }
}
