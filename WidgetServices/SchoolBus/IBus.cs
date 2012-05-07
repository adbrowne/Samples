namespace SchoolBus
{
    using System;

    using SchoolBus.InProcess;

    public interface IBus
    {
        void Publish(IMessage message);

        void Subscribe<TEvent>(Action<IMessage> handler) where TEvent : IMessage;

        TResult Execute<TResult>(object command);

        void Reply(object replyCode);
    }
}