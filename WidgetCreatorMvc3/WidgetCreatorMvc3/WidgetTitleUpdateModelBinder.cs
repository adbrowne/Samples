namespace WidgetCreatorMvc
{
    using System;
    using System.Web.Mvc;

    using Autofac.Integration.Mvc;

    using WidgetCreatorMvc3.Service.Core;
    using WidgetCreatorMvc3.Service.DTO;

    [ModelBinderType(typeof(WidgetId))]
    public class WidgetIdModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            var value = bindingContext.ValueProvider.GetValue(modelName);
            var id = value.ConvertTo(typeof(Guid));
            return new WidgetId((Guid)id);
        }
    }

    [ModelBinderType(typeof(WidgetTitleUpdate))]
    public class WidgetTitleUpdateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var bindModel = base.BindModel(controllerContext, bindingContext);
            return bindModel;
        }

        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            var idValue = bindingContext.ValueProvider.GetValue("Id");
            ((WidgetTitleUpdate)bindingContext.Model).Id = new WidgetId(new Guid(idValue.RawValue.ToString()));
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}