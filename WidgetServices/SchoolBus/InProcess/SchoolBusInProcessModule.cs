namespace SchoolBus.InProcess
{
    using Autofac;

    using SchoolBus;

    public class SchoolBusInProcessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<Bus>().As<IBus>().As<Bus>().InstancePerLifetimeScope();
            builder.RegisterType<HandlerRegistry>().SingleInstance();
            builder.RegisterType<CommandHandlerChainBuilder>();
            builder.RegisterType<CommandExecutor>();
        }
    }
}