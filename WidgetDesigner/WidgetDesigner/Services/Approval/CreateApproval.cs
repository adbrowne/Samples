namespace WidgetDesigner.Services.Approval
{
    using System;

    using WidgetDesigner.Contract;

    public class CreateApproval : IMessage
    {
        public Guid ApprovalId { get; set; }
        public string Title { get; set; }
    }
}