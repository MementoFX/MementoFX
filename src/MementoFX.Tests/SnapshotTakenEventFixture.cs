using System;
using Moq;
using NUnit.Framework;
using SharpTestsEx;
using Memento;
using Memento.Domain;


namespace Memento.Tests
{
    [TestFixture]
    public class SnapshotTakenEventFixture
    {
        [Test]
        public void Ctor_should_throw_ArgumentNullException_on_null_memento()
        {
            Executing.This(() => new SnapshotTakenEvent(null))
                .Should()
                .Throw<ArgumentNullException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("memento");
        }

        [Test]
        public void Ctor_should_set_Memento_property()
        {
            var memento = new Mock<IAggregateMemento>().Object;
            var sut = new SnapshotTakenEvent(memento);
            Assert.AreSame(memento, sut.Memento);
        }

        [Test]
        public void Ctor_should_set_AggregateFullName_property()
        {
            var mementoMock = new Mock<IAggregateMemento>();
            var memento = mementoMock.Object;
            mementoMock.SetupProperty(t => t.AggregateFullTypeName, "fake");
            var sut = new SnapshotTakenEvent(memento);
            Assert.AreSame(memento.AggregateFullTypeName, sut.AggregateFullTypeName);
        }
    }
}
