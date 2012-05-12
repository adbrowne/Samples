namespace SchoolBus.InProcess
{
    using System;
    using System.Collections.Generic;

    public class Bus : IBus
    {
        private readonly Func<CommandHandlerChainBuilder> _chcb;

        private readonly HandlerRegistry _handlerRegistry;

        public Bus(Func<CommandHandlerChainBuilder> chcb, HandlerRegistry handlerRegistry)
        {
            this._chcb = chcb;
            _handlerRegistry = handlerRegistry;
        }
 
        public void Publish(IMessage message)
        {
            if (_handlerRegistry.Handlers.ContainsKey(message.GetType()))
            {
                var handlers = _handlerRegistry.Handlers[message.GetType()];
                foreach (var messageHandler in handlers)
                {
                    messageHandler(message);
                }
            }
        }

        public void Subscribe<TEvent>(Action<IMessage> handler) where TEvent : IMessage
        {
            _handlerRegistry.Subscribe<TEvent>(handler);
        }

        public TResult Execute<TResult>(object command)
        {
            this._reply = null;
            var commandHandler = _handlerRegistry.CommandHandlerBuilders[command.GetType()].Build();
            commandHandler.Execute(command);

            if (this._reply != null)
            {
                return (TResult)this._reply;
            }
            else
            {
                return default(TResult);
            }
        }

        public void Reply(object replyCode)
        {
            this._reply = replyCode;
        }


        private object _reply;

        public CommandHandlerChainBuilder ForCommand<T>()
        {
            var chainBuilder = this._chcb();
            _handlerRegistry.CommandHandlerBuilders.Add(typeof(T), chainBuilder);
            return chainBuilder;
        }

        public void FuturePublish<T>(DateTime timeToRespond, T message)
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Request<TRequest, TResponse>(TRequest request, Action<TResponse> onResponse)
        {
            throw new NotImplementedException();
        }

        public void Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>(string subscriptionId, Action<T> onMessage)
        {
            throw new NotImplementedException();
        }
    }
}