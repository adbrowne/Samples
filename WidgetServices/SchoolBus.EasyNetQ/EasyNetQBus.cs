
using System;

namespace SchoolBus.EasyNetQ
{
    public class EasyNetQBus : SchoolBus.IBus
    {
        private readonly global::EasyNetQ.IBus _easyNetQBus;

        public EasyNetQBus(global::EasyNetQ.IBus easyNetQBus)
        {
            _easyNetQBus = easyNetQBus;
        }

        public void FuturePublish<T>(DateTime timeToRespond, T message)
        {
            _easyNetQBus.FuturePublish(timeToRespond, message);
        }

        public void Publish<T>(T message)
        {
            _easyNetQBus.Publish(message);
        }

        public void Request<TRequest, TResponse>(TRequest request, Action<TResponse> onResponse)
        {
            _easyNetQBus.Request(request, onResponse);
        }

        public void Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
        {
            _easyNetQBus.Respond(responder);
        }

        public void Subscribe<T>(string subscriptionId, Action<T> onMessage)
        {
            _easyNetQBus.Subscribe<T>(subscriptionId, message =>
                {
                    bool completed = false;
                    int count = 0;
                    while (!completed)
                    {
                        try
                        {
                            onMessage(message);
                            completed = true;
                        }
                        catch
                        {
                            count++;
                            if (count == 5)
                                throw;
                        }
                    }
                });
        }
    }
}