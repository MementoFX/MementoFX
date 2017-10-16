using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX.Tests.Assets.Events
{
    public class UnitTestFriendlyDomainEvent : DomainEvent
    {
        public void SetTimeStamp(DateTime pointInTime)
        {
            var type = typeof(DomainEvent);
            var property = type.GetProperty(nameof(DomainEvent.TimeStamp));
            property.SetValue(this, pointInTime);
        }
    }
}
