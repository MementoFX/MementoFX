using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento.Domain;
using Memento.Persistence;
using Memento.Tests.Assets.Events;

namespace Memento.Tests.Assets.Model
{
    public class SelfRetrievingCurrentAccount : Aggregate, IManageEventRetrieval
    {
        public decimal Balance { get; private set; }

        public void ApplyEvent(AccountOpenedEvent @event)
        {
            this.Id = @event.CurrentAccountId;
            this.Balance = @event.Balance;
        }
        public void ApplyEvent(WithdrawalEvent @event)
        {
            this.Balance -= @event.Amount;
        }

        public IEnumerable<DomainEvent> RetrieveEvents(IEventStore eventStore, Guid aggregateId, Guid? timelineId, DateTime? pointInTime)
        {
            var events = new List<DomainEvent>();

            var withdrawalsQuery = eventStore.Find<WithdrawalEvent>(e => e.CurrentAccountId == aggregateId && (e.TimelineId == null || e.TimelineId == timelineId));
            if(pointInTime.HasValue)
            {
                withdrawalsQuery = withdrawalsQuery.Where(e => e.TimeStamp <= pointInTime);
            }
            var withdrawals = (from e in withdrawalsQuery
                              orderby e.TimeStamp 
                              select e).ToList();
            events.AddRange(withdrawals);

            var accountOpeningQuery = eventStore.Find<AccountOpenedEvent>(e => e.CurrentAccountId == aggregateId && (e.TimelineId == null || e.TimelineId == timelineId));
            if (pointInTime.HasValue)
            {
                accountOpeningQuery = accountOpeningQuery.Where(e => e.TimeStamp <= pointInTime);
            }
            var accountOpeningEvents = (from e in accountOpeningQuery
                                       orderby e.TimeStamp
                                      select e).ToList();
            events.AddRange(accountOpeningEvents);

            return events.OrderBy(e => e.TimeStamp);
        }
    }
}
