namespace Tests.MultiDbWithRabbit
{
    public class ValueService
    {
        public int Value { get; set; }
        public void Handle(SetValueCommand command)
        {
            this.Value = command.Value;
        }
    }
}