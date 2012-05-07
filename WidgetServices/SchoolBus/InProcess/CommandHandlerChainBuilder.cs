namespace SchoolBus.InProcess
{
    using System;

    public class CommandHandlerChainBuilder
    {
        private readonly Func<Type, CommandExecutor> _createExecutor;

        public CommandHandlerChainBuilder(Func<Type, CommandExecutor> createExecutor)
        {
            this._createExecutor = createExecutor;
        }

        private Type _handler;

        public CommandHandlerChainBuilder Execute<T>()
        {
            this._handler = typeof(T);
            return this;
        }

        public CommandExecutor Build()
        {
            return this._createExecutor(this._handler);
        }
    }
}