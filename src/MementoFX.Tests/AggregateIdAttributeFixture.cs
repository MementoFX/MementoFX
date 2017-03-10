using System;
using SharpTestsEx;
using Memento.Domain;
using NUnit.Framework;

namespace Memento.Tests
{
    [TestFixture]
    public class AggregateIdAttributeFixture
    {
        [Test]
        public void Ctor_should_throw_ArgumentException_on_null_propertyName_parameter()
        {
            Executing.This(() => new AggregateIdAttribute(null))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("propertyName");
        }

        [Test]
        public void Ctor_should_throw_ArgumentException_on_blank_propertyName_parameter()
        {
            Executing.This(() => new AggregateIdAttribute(string.Empty))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("propertyName");
        }

        [Test]
        public void Ctor_should_throw_ArgumentException_on_whitespace_valued_propertyName_parameter()
        {
            Executing.This(() => new AggregateIdAttribute(" "))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("propertyName");
        }

        [Test]
        public void Ctor_should_throw_ArgumentException_if_provided_propertyName_parameter_contains_whitespaces()
        {
            Executing.This(() => new AggregateIdAttribute("The event"))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("propertyName");
        }

        [Test]
        public void Ctor_should_set_PropertyName_property()
        {
            var propertyName = "ThePropertyName";
            var sut = new AggregateIdAttribute(propertyName);
            Assert.AreEqual(propertyName, sut.PropertyName);
        }
    }
}
