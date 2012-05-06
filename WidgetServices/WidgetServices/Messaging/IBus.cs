namespace WidgetServices.Messaging
{
    using System;

    public interface IBus
    {
        void Publish(IMessage message);

        void Subscribe<TEvent>(Action<IMessage> handler) where TEvent : IMessage;
    }
}