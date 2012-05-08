namespace SchoolBus.InProcess
{
    using System;
    using System.Reflection;

    using Autofac;

    public class CommandExecutor
    {
        private readonly Type _handler;

        private readonly ILifetimeScope _scope;

        public CommandExecutor(Type handler, ILifetimeScope scope)
        {
            _handler = handler;
            _scope = scope;
        }

        public void Execute(object command)
        {
            using (var childScope = _scope.BeginLifetimeScope())
            {
                var instance = childScope.Resolve(_handler);
                _handler.InvokeMember("Execute", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance, null, instance, new[] { command });
            }
        }
    }
}