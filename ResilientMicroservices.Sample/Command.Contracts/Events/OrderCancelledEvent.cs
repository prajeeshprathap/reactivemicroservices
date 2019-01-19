using System;

namespace Comman.Contracts.Events
{
    [Event("OrderCancelled")]
    public class OrderCancelledEvent : IEvent
    {
        public Guid Id { get; set; }
    }
}
