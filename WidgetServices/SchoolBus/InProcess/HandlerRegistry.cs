namespace SchoolBus.InProcess
{
    using System;
    using System.Collections.Generic;

    public class HandlerRegistry
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _handlers = new Dictionary<Type, List<Action<IMessage>>>();

        public Dictionary<Type, List<Action<IMessage>>> Handlers { get { return _handlers; } }

        public void Subscribe<TEvent>(Action<IMessage> handler) where TEvent : IMessage
        {
            var eventType = typeof(TEvent);
            if (!this._handlers.ContainsKey(eventType))
            {
                this._handlers.Add(eventType, new List<Action<IMessage>>());
            }
            var handlers = this._handlers[eventType];
            handlers.Add(handler);
        }

        private readonly Dictionary<Type, CommandHandlerChainBuilder> _commandHandlerBuilders = new Dictionary<Type, CommandHandlerChainBuilder>();

        public Dictionary<Type, CommandHandlerChainBuilder> CommandHandlerBuilders { get { return _commandHandlerBuilders; } }
    }
}