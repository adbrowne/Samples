namespace WidgetDesigner.Contract
{
    using System;

    public interface ICallback
    {
        IAsyncResult Register(AsyncCallback callback, object state);
    }
}