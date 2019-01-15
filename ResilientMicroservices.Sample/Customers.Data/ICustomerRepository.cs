using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Command.Contracts;

namespace ResilientMicroservices.Sample.Customers.Data
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAll(CancellationToken cancellationToken);
        Task<Customer> GetById(Guid id, CancellationToken cancellationToken);
        Task New(Customer customer, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}