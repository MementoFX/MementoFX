using System;
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
            Executing.This(() => new SnapshotTakenEvent(null, null))
                .Should()
                .Throw<ArgumentNullException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("memento");
        }
    }
}
