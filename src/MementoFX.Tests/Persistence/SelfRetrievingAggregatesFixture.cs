using System;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using SharpTestsEx;
using Memento.Tests.Assets.Events;
using Memento.Tests.Assets.Model;
using Memento.Messaging;
using Memento.Persistence;
using Memento.Persistence.InMemory;

namespace Memento.Tests.Persistence
{
    [TestFixture]
    public class SelfRetrievingAggregatesFixture
    {
        private IEventStore EventStore;

        [SetUp]
        public void SetUp()
        {
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            EventStore = new InMemoryEventStore(eventDispatcherMock.Object);
        }

        [Test]
        public void Test_EventReplaying_evaluating_CurrentAccountBalance_using_a_stream_containing_past_events_only()
        {
            var currentAccountId = Guid.NewGuid();
            var accountOpening = new AccountOpenedEvent
            {
                CurrentAccountId = currentAccountId,
                Balance = 200
            };
            EventStore.Save(accountOpening);

            var withdrawal1 = new WithdrawalEvent()
            {
                CurrentAccountId = currentAccountId,
                Amount = 100
            };
            withdrawal1.SetTimeStamp(DateTime.Now.AddMonths(1));
            EventStore.Save(withdrawal1);

            var sut = new Repository(EventStore);
            var currentAccount = sut.GetById<SelfRetrievingCurrentAccount>(currentAccountId, DateTime.Now.AddMonths(2));
            Assert.AreEqual(100, currentAccount.Balance);
        }

        [Test]
        public void Test_EventReplaying_evaluating_CurrentAccountBalance_using_a_stream_containing_both_past_and_future_events()
        {
            var currentAccountId = Guid.NewGuid();
            var accountOpening = new AccountOpenedEvent
            {
                CurrentAccountId = currentAccountId,
                Balance = 200
            };
            EventStore.Save(accountOpening);

            var withdrawal1 = new WithdrawalEvent()
            {
                CurrentAccountId = currentAccountId,
                Amount = 100
            };
            withdrawal1.SetTimeStamp(DateTime.Now.AddMonths(1));
            EventStore.Save(withdrawal1);

            var withdrawal2 = new WithdrawalEvent()
            {
                CurrentAccountId = currentAccountId,
                Amount = 100
            };
            withdrawal2.SetTimeStamp(DateTime.Now.AddMonths(3));
            EventStore.Save(withdrawal2);

            var sut = new Repository(EventStore);
            var currentAccount = sut.GetById<SelfRetrievingCurrentAccount>(currentAccountId, DateTime.Now.AddMonths(2));
            Assert.AreEqual(100, currentAccount.Balance);
        }

        [Test]
        public void Test_EventReplaying_evaluating_CurrentAccountBalance_using_a_stream_containing_past_events_only_and_a_different_timeline()
        {
            var currentAccountId = Guid.NewGuid();
            var accountOpening = new AccountOpenedEvent
            {
                CurrentAccountId = currentAccountId,
                Balance = 200
            };
            EventStore.Save(accountOpening);

            var withdrawal1 = new WithdrawalEvent()
            {
                CurrentAccountId = currentAccountId,
                Amount = 100
            };
            withdrawal1.SetTimeStamp(DateTime.Now.AddMonths(1));
            EventStore.Save(withdrawal1);

            var withdrawal2 = new WithdrawalEvent()
            {
                CurrentAccountId = currentAccountId,
                Amount = 100,
                TimelineId = Guid.NewGuid()
            };
            withdrawal2.SetTimeStamp(DateTime.Now.AddMonths(3));
            EventStore.Save(withdrawal2);

            var sut = new Repository(EventStore);
            var currentAccount = sut.GetById<SelfRetrievingCurrentAccount>(currentAccountId, DateTime.Now.AddMonths(3));
            Assert.AreEqual(100, currentAccount.Balance);
        }

        [Test]
        public void Test_Timeline_specific_EventReplaying_evaluating_CurrentAccountBalance_using_a_stream_containing_both_past_and_future_events()
        {
            var currentAccountId = Guid.NewGuid();
            var timelineId = Guid.NewGuid();
            var accountOpening = new AccountOpenedEvent
            {
                CurrentAccountId = currentAccountId,
                Balance = 200,
                TimelineId = timelineId
            };
            EventStore.Save(accountOpening);

            var withdrawal1 = new WithdrawalEvent()
            {
                CurrentAccountId = currentAccountId,
                Amount = 100
            };
            withdrawal1.SetTimeStamp(DateTime.Now.AddMonths(1));
            EventStore.Save(withdrawal1);

            var withdrawal2 = new WithdrawalEvent()
            {
                CurrentAccountId = currentAccountId,
                Amount = 50,
                TimelineId = timelineId
            };
            withdrawal2.SetTimeStamp(DateTime.Now.AddMonths(2));
            EventStore.Save(withdrawal2);

            var sut = new Repository(EventStore);
            var currentAccount = sut.GetById<SelfRetrievingCurrentAccount>(currentAccountId, timelineId, DateTime.Now.AddMonths(3));
            Assert.AreEqual(50, currentAccount.Balance);
        }

    }
}
