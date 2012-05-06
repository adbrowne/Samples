namespace WidgetDesigner.Infra.InProcess
{
    using WidgetDesigner.Contract;

    public class Bus : IBus
    {
        public ICallback Send(params object[] messages)
        {
            return new Callback();
        }
    }

}