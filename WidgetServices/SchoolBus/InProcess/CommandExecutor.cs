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
            this._handler = handler;
            this._scope = scope;
        }

        public void Execute(object command)
        {
            var instance = this._scope.Resolve(this._handler);
            this._handler.InvokeMember("Execute", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance, null, instance, new[] { command });
        }
    }
}