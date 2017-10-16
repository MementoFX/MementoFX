using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX.Tests.Assets.Events
{
    public class WithdrawalEvent : UnitTestFriendlyDomainEvent
    {
        public decimal Amount { get; set; }

        public Guid CurrentAccountId { get; set; }
    }
}
