namespace WidgetDesigner.Infra
{
    using System;
    using System.Threading;

    using WidgetDesigner.Contract;

    public class Callback : ICallback
    {
        public IAsyncResult Register(AsyncCallback callback, object state)
        {
            var result = new BusAsyncResult(callback, state);

            return result;
        }
    }

    public class BusAsyncResult : IAsyncResult
    {
        private readonly AsyncCallback _callback;

        private readonly object _state;

        private bool _completed;

        private ManualResetEvent _sync;

        public BusAsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            _state = state;
            _sync = new ManualResetEvent(false);
        }

        public void Complete()
        {
            _completed = true;
            if(_callback != null)
            {
                _callback(this);
            }
            _sync.Set();
        }

        public bool IsCompleted
        {
            get
            {
                return _completed;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return _sync;
            }
        }

        public object AsyncState
        {
            get
            {
                return new object();
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }
    }
}