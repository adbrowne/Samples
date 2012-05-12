namespace SchoolBus
{
    using System;

    public interface IBus
    {
        void FuturePublish<T>(DateTime timeToRespond, T message);
        void Publish<T>(T message);
        void Request<TRequest, TResponse>(TRequest request, Action<TResponse> onResponse);
        void Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder);
        void Subscribe<T>(string subscriptionId, Action<T> onMessage);
    }
}