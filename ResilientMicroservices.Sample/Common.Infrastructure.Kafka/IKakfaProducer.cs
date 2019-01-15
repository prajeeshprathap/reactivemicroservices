using Command.Contracts.Events;
using System.Threading.Tasks;

namespace Common.Infrastructure.Kafka
{
    public interface IKakfaProducer
    {
        Task Send(IEvent @event, string topic);
    }
}
