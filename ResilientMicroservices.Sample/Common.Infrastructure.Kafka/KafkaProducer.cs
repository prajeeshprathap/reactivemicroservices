﻿using Command.Contracts.Events;
using Common.Contracts;
using Common.Domain;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Kafka
{
    public class KafkaProducer : IKakfaProducer
    {
        private readonly ILog _log;
        private readonly ISerializingProducer<Null, string> _producer;
        protected readonly KafkaEventProducerConfiguration Options;

        public KafkaProducer(IOptions<KafkaEventProducerConfiguration> options, ILog log)
        {
            Options = options.Value;
            var config = new Dictionary<string, object>
            {
                {"bootstrap.servers", Options.Server},
                {"message.send.max.retries", Options.MaxRetries},
                {
                    "default.topic.config", new Dictionary<string, object>
                    {
                        {"message.timeout.ms", Options.MessageTimeout.TotalMilliseconds}
                    }
                }
            };
            _producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
            _log = log;
        }

        public async Task Send(IEvent @event, string topic)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            if (string.IsNullOrEmpty(topic)) throw new ArgumentNullException(nameof(topic));

            var message = GetMessage(@event);
            var result = await _producer.ProduceAsync(topic, null, message);

            if (result.Error.Code != ErrorCode.NoError)
            {
                var errorReason = result.Error.Reason;
                throw new ArgumentException(errorReason);
            }

            _log.Info($"{nameof(KafkaProducer)}: message sent on topic <{topic}>: ", @event);
        }

        private string GetMessage(IEvent @event)
        {
            var attributes = @event.GetType().GetCustomAttribute<EventAttribute>();
            if (attributes == null)
            {
                throw new ArgumentException($"{nameof(EventAttribute)} missing on {nameof(@event)}");
            }

            if (string.IsNullOrEmpty(attributes.Name))
            {
                throw new ArgumentNullException(
                    $"{nameof(EventAttribute)}.Name missing on {nameof(@event)}");
            }

            var message = new KMessage
            {
                Name = attributes.Name,
                EventData = JsonConvert.SerializeObject(@event, Options.SerializerSettings)
            };

            return JsonConvert.SerializeObject(message, Options.SerializerSettings);
        }
    }
}
