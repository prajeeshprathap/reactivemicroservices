using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Command.Contracts;
using Common.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using EnsureThat;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;

namespace ResilientMicroservices.Sample.Customers.Data
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        IMongoCollection<Customer> Customers => MongoDatabase.GetCollection<Customer>("Customer");

        public CustomerRepository(MongoDbSettings settings) : base(settings)
        {
        }

        public async Task<IEnumerable<Customer>> GetAll(CancellationToken cancellationToken)
        {
            return await Customers.AsQueryable().Select(c => c).ToListAsync(cancellationToken);
        }

        public Task<Customer> GetById(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(Customers.Find(c => c.Id == id, new FindOptions { AllowPartialResults = false }).FirstOrDefault(cancellationToken));
        }

        public async Task New(Customer customer, CancellationToken cancellationToken)
        {
            await Customers.InsertOneAsync(customer, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            await Customers.DeleteOneAsync(c => c.Id == id, cancellationToken);
        }
    }
}
