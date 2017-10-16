using MementoFX.Domain;
using MementoFX.Tests.Assets.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX.Tests.Assets.Model
{
    public class CurrentAccount : Aggregate
    {
        public decimal Balance { get; private set; }

        public void ApplyEvent(AccountOpenedEvent @event)
        {
            this.Id = @event.CurrentAccountId;
            this.Balance = @event.Balance;
        }

        public void ApplyEvent(WithdrawalEvent @event)
        {
            this.Balance -= @event.Amount;
        }

        public static class Factory
        {
            public static CurrentAccount CreateCurrentAccount()
            {
                var a = new CurrentAccount()
                {
                    Id = Guid.NewGuid()
                };
                var e = new AccountOpenedEvent()
                {
                    CurrentAccountId = a.Id,
                    Balance = 0
                };
                
                a.RaiseEvent(e);

                return a;
            }
        }
    }
}
