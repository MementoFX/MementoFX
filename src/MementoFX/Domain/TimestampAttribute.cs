using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX.Domain
{
    /// <summary>
    /// Qualifies a property as the event's timestamp
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TimestampAttribute : Attribute
    {
    }
}
