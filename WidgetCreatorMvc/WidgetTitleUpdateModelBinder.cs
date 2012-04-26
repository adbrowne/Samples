namespace WidgetCreatorMvc
{
    using System.Web.Mvc;

    using Autofac.Integration.Mvc;

    using WidgetCreatorMvc.Service.DTO;

    [ModelBinderType(typeof(WidgetTitleUpdate))]
    public class WidgetTitleUpdateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}