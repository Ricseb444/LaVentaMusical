using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;


namespace Utilidades
{
    public class RazorViewToStringRenderer
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public RazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewToStringAsync<TModel>(
            Controller controller,
            string viewName,
            TModel model)
        {
            var actionContext = new ActionContext(
                controller.HttpContext,
                controller.RouteData,
                controller.ControllerContext.ActionDescriptor
            );

            var viewResult = _viewEngine.FindView(actionContext, viewName, false);
            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"No se encontró la vista: {viewName}");
            }

            var viewData = new ViewDataDictionary<TModel>(
                controller.ViewData,
                controller.ModelState)
            {
                Model = model
            };

            using var sw = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                new TempDataDictionary(controller.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
