using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.ViewModels;
using doStuff.Services;
using doStuff.Models;

namespace doStuff.Controllers
{
    public class ParentController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            //TODO MAGIC
        }
    }
}