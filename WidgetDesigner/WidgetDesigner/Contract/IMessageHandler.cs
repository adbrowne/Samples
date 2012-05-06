namespace WidgetDesigner.Contract
{
    public interface IMessageHandler<in T>
    {
        void Handle(T command);
    }
}