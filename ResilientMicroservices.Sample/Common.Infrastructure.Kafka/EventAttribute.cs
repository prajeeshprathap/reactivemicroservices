using System;

namespace Common.Infrastructure.Kafka
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EventAttribute : Attribute
    {
        public string Name { get; }

        public EventAttribute(string name)
        {
            Name = name;
        }
    }
}
