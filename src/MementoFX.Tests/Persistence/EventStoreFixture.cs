using System;
using MementoFX.Domain;
using MementoFX.Persistence;
using Xunit;
using SharpTestsEx;
using Moq;
using MementoFX.Messaging;
using MementoFX.Persistence.InMemory;

namespace MementoFX.Tests.Persistence
{
    
    public class EventStoreFixture
    {
        [Fact]
        public void ManageTimestamp_should_set_Timestamp_property()
        {
            var timestamp = new DateTime(2015, 11, 13);
            var @event = new FakeEventProvidingNaturalTimestamp()
            {
                EventProperty = 42,
                EventTimestamp = timestamp
            };
            EventStore.ManageTimestamp(@event);
            Assert.Equal(timestamp, @event.TimeStamp);
        }

        [Fact]
        public void ManageTimestamp_should_not_alter_Timestamp_property()
        {           
            var @event = new FakeEventUsingBuiltinTimestamp()
            {
                EventProperty = 42
            };
            var timestamp = @event.TimeStamp;
            EventStore.ManageTimestamp(@event);
            Assert.Equal(timestamp, @event.TimeStamp);
        }

        [Fact]
        public void Save_should_throw_ArgumentNullException_on_null_parameter()
        {
            var eventDispatcherMockBuilder = new Mock<IEventDispatcher>();

            var sut = new InMemoryEventStore(eventDispatcherMockBuilder.Object);
            Executing.This(() => sut.Save(null))
                .Should()
                .Throw<ArgumentNullException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("event");
        }

        public class FakeEventUsingBuiltinTimestamp : DomainEvent
        {
            public int EventProperty { get; set; }

            public DateTime EventTimestamp { get; set; }
        }

        public class FakeEventProvidingNaturalTimestamp : DomainEvent
        {
            public int EventProperty { get; set; }

            [Timestamp]
            public DateTime EventTimestamp { get; set; }
        }
    }
}
