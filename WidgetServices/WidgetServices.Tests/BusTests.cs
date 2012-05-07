namespace WidgetServices.Tests
{
    using NUnit.Framework;

    using SchoolBus;
    using SchoolBus.InProcess;

    [TestFixture]
    public class BusTests
    {
        [Test]
        public void EventTest()
        {
            var bus = new Bus(null, new HandlerRegistry());
            int called = 0;
            bus.Subscribe<WidgetCreatedEvent>((x) => called++);

            bus.Subscribe<WidgetCreatedEvent>((x) => called++);

            bus.Publish(new WidgetCreatedEvent());
            Assert.AreEqual(2, called);

        }
    }

    public class WidgetCreatedEvent : IMessage
    {
    }
}
