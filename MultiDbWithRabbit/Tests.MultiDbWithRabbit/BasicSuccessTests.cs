namespace Tests.MultiDbWithRabbit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EasyNetQ;

    using NUnit.Framework;

    using Tests.MultiDbWithRabbit.SutBasic;

    [TestFixture]
    public class BasicSuccessTests
    {
        [Test]
        public void ValueCanBeSaved()
        {
            const int Value = 10;

            var valueService = new ValueService();
            var serviceBus = RabbitHutch.CreateBus();
            serviceBus.Subscribe<SetValueCommand>("ValueService", valueService.Handle);

            var clientBus = RabbitHutch.CreateBus();

            using (var publishChannel = clientBus.OpenPublishChannel())
            {
                publishChannel.Publish(new SetValueCommand { Value = Value });
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(TestConstants.WaitTime));
            Assert.That(valueService.Value, Is.EqualTo(Value));
        }
    }
}
