using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Artaxias.Web.Server.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelStateValidationFilter : Attribute, IActionFilter
    {
        /// <summary>
        /// On action executing
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;
            Dictionary<string, string> lErrorsDictionary = new();
            foreach (ModelError lModelError in context.ModelState.Values.SelectMany(error => error.Errors))
            {
                lErrorsDictionary[lModelError.ErrorMessage] = lModelError.Exception == null ? lModelError.ErrorMessage : lModelError.Exception.Message;
            }
            context.Result = ((ControllerBase)context.Controller).StatusCode((int)HttpStatusCode.BadRequest, lErrorsDictionary);
        }

        /// <summary>
        /// On action executed
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

    }
}
