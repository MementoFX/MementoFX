using System;
using System.Collections.Generic;
using System.Text;

namespace Memento.Domain
{
    public interface IAggregateMemento
    {
        IEnumerable<DomainEvent> OccurredEvents { get; }
    }
}
