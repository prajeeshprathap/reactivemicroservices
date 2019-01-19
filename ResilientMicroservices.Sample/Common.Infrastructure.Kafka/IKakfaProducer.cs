using System.Threading;
using System.Threading.Tasks;
using Comman.Contracts.Events;

namespace Common.Infrastructure.Kafka
{
    public interface IKakfaProducer
    {
        Task Send(IEvent @event, string topic);
    }
}
