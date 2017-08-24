using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Domain
{
    /// <summary>
    /// Provides the base implementation for the Memento.IAggregate interface
    /// </summary>
    public abstract class Aggregate : IAggregate
    {
        #region IAggregate interface implementation
        Guid IAggregate.Id 
        { 
            get
            {
                return Id;
            }
        }

        bool IAggregate.HasPendingChanges 
        { 
            get 
            { 
                return this.uncommittedEvents.Any();  
            } 
        }

        IEnumerable<DomainEvent> IAggregate.GetUncommittedEvents()
        {
            return uncommittedEvents.ToArray();
        }

        void IAggregate.ClearUncommittedEvents()
        {
            uncommittedEvents.Clear();
        }

        IList<DomainEvent> IAggregate.OccurredEvents { get; set; }

        void IAggregate.MarkEventAsSaved(DomainEvent @event)
        {
            if(!this.uncommittedEvents.Contains(@event))
                throw new InvalidOperationException("The specified event does not belong to this object");
            this.uncommittedEvents.Remove(@event);

            IAggregate self = this;
            if (self.OccurredEvents == null)
                self.OccurredEvents = new List<DomainEvent>();

            (this as IAggregate).OccurredEvents.Add(@event);
        }

        /// <summary>
        /// Inject the list of occurred events into the aggregate instance
        /// </summary>
        /// <param name="occurredEvents"></param>
        void IAggregate.ReplayEvents(IEnumerable<DomainEvent> occurredEvents)
        {
            IAggregate self = this;
            if (self.OccurredEvents == null)
                self.OccurredEvents = new List<DomainEvent>();
            foreach (var @event in occurredEvents)
            {
                self.OccurredEvents.Add(@event);
                ((dynamic)this).ApplyEvent((dynamic)@event);
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets whether the instance is a time travelling one
        /// </summary>
        internal bool IsTimeTravelling { get; set; }

        /// <summary>
        /// Gets or sets the aggregate id
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Gets the list of occurred events
        /// </summary>
        protected IEnumerable<DomainEvent> OccurredEvents
        {
            get
            {
                IAggregate self = this;
                if (self.OccurredEvents == null)
                    self.OccurredEvents = new List<DomainEvent>();
                return self.OccurredEvents.ToArray();
            }
        }

        private IList<DomainEvent> uncommittedEvents = null;

        private IList<DomainEvent> UncommittedEvents
        {
            get
            {
                if (uncommittedEvents == null)
                    uncommittedEvents = new List<DomainEvent>();
                return uncommittedEvents;
            }
        }

        /// <summary>
        /// Raises an event so to have it applied at aggregate's instance level
        /// </summary>
        /// <param name="event">The event to be applied</param>
        protected void RaiseEvent(DomainEvent @event)
        {
            UncommittedEvents.Add(@event);
            (this as dynamic).ApplyEvent((dynamic)@event);
        }

        #region snapshots management
        /// <summary>
        /// Creates a memento for the current instance
        /// </summary>
        /// <returns></returns>
        protected AggregateMemento CreateMemento()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the state of the current instance via a memento
        /// </summary>
        /// <param name="memento">The memento used to set the instance's state</param>
        protected void RestoreMemento(AggregateMemento memento)
        {
            if (memento == null)
                throw new ArgumentNullException(nameof(memento));
            
            throw new NotImplementedException();
        }
        #endregion
    }
}
