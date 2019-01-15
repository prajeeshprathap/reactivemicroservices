using System;
using System.Collections.Generic;
using System.Text;

namespace Command.Contracts
{
    [Serializable]
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
