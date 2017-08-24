using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Memento.Messaging;
using Memento.Persistence;
using Memento.Persistence.InMemory;

namespace Memento.Tests.Persistence
{
    [TestFixture]
    public class Repository_SnapshotTestFixture
    {
        private IEventStore EventStore;

        [SetUp]
        public void SetUp()
        {
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            EventStore = new InMemoryEventStore(eventDispatcherMock.Object);
        }

        //public void SaveAndTakeSnapshot
    }
}
