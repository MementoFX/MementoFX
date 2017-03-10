using System;
using Moq;
using NUnit.Framework;
using SharpTestsEx;
using Memento.Domain;
using Memento.Messaging;
using Memento.Persistence;
using Memento.Tests.Events;
using Memento.Tests.Model;
using Memento.Persistence.InMemory;

namespace Memento.Tests.Persistence
{
    [TestFixture]
    public class RepositoryTextFixture
    {
        private IEventStore EventStore;

        [SetUp]
        public void SetUp()
        {
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            EventStore = new InMemoryEventStore(eventDispatcherMock.Object);
        }

        [Test]
        public void Ctor_should_throw_ArgumentNullException_on_null_eventStore_and_value_of_parameter_should_be_eventStore()
        {
            Executing.This(() => new Repository(null))
                           .Should()
                           .Throw<ArgumentNullException>()
                           .And
                           .ValueOf
                           .ParamName
                           .Should()
                           .Be
                           .EqualTo("eventStore");
        }

        [Test]
        public void Test_EventCount()
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
            EventStore.Save(withdrawal1);

            var sut = new Repository(EventStore);
            var currentAccount = sut.GetById<CurrentAccount>(currentAccountId);
            Assert.AreEqual(2, ((IAggregate)currentAccount).OccurredEvents.Count);
        }


        [Test]
        public void Test_EventCount_at_a_specific_date()
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
            var currentAccount = sut.GetById<CurrentAccount>(currentAccountId, DateTime.Now.AddMonths(2));
            Assert.AreEqual(2, ((IAggregate)currentAccount).OccurredEvents.Count);
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
            var currentAccount = sut.GetById<CurrentAccount>(currentAccountId, DateTime.Now.AddMonths(2));
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
            var currentAccount = sut.GetById<CurrentAccount>(currentAccountId, DateTime.Now.AddMonths(2));

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
            var currentAccount = sut.GetById<CurrentAccount>(currentAccountId, DateTime.Now.AddMonths(3));
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
            var currentAccount = sut.GetById<CurrentAccount>(currentAccountId, timelineId, DateTime.Now.AddMonths(3));
            Assert.AreEqual(50, currentAccount.Balance);
        }

        [Test]
        public void Test_Repository_should_update_OccurredEvents()
        {
            var currentAccount = CurrentAccount.Factory.CreateCurrentAccount();

            var eventStore = new Mock<IEventStore>().Object;
            var sut = new Repository(eventStore);

            sut.Save<CurrentAccount>(currentAccount);
            Assert.AreEqual(1, ((IAggregate)currentAccount).OccurredEvents.Count);
        }
    }
}
