namespace WidgetServices.Messaging
{
    using System;
    using System.Collections.Generic;

    public class Bus : IBus
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _handlers = new Dictionary<Type, List<Action<IMessage>>>();

        public void Publish(IMessage message)
        {
            if (_handlers.ContainsKey(message.GetType()))
            {
                var handlers = _handlers[message.GetType()];
                foreach (var messageHandler in handlers)
                {
                    messageHandler(message);
                }
            }
        }

        public void Subscribe<TEvent>(Action<IMessage> handler) where TEvent : IMessage
        {
            var eventType = typeof(TEvent);
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers.Add(eventType, new List<Action<IMessage>>());
            }
            var handlers = _handlers[eventType];
            handlers.Add(handler);
        }
    }

    public interface IMessage
    {
    }
}