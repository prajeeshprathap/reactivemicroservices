using System;

namespace Comman.Contracts.Events
{
    [Event("CreditLimitChanged")]
    public class CreditLimitChangedEvent : IEvent
    {
        public Guid CustomerId { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
