namespace WidgetDesigner.Contract
{
    public interface IBus
    {
        ICallback Send(params object[] messages);
    }
}