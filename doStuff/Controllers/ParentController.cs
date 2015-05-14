using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.ViewModels;
using doStuff.Services;
using doStuff.Models;
using System.IO;
using System.Configuration;

namespace doStuff.Controllers
{
    public class ParentController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;
            string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            string LogFileName = ConfigurationManager.AppSettings["LogFileName"];
            string message = string.Format("{0} and was thrown on the {1}.{4}For {2}{3}{4}", ex.Message, DateTime.Now, ex.Source, ex.StackTrace, Environment.NewLine);
            if (Directory.Exists(LogFilePath) == false)
            {
                Directory.CreateDirectory(LogFilePath);
            }
            System.IO.File.AppendAllText(LogFilePath + LogFileName, message);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}