using System;
using System.IO;
using System.Web.Mvc;
using ExcelDocumentProcessor.Web.ApplicationLogic.Security;

namespace ExcelDocumentProcessor.Web.Controllers
{
    [NeonAuthorize]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Generic log method for exception
        /// </summary>
        /// <param name="ex"></param>
        public void Log(Exception ex)
        {
        }

        /// <summary>
        /// Generic log method for strings
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
        }

        /// <summary>
        /// Renders a partial to be returned as a string.
        /// Used for the rendering of the jquery dialog boxes.
        /// </summary>
        /// <param name="partialName">Partial Name</param>
        /// <param name="model">Model</param>
        /// <returns>Partial View</returns>
        protected string RenderPartialView(string partialName, object model = null)
        {
            if (ControllerContext == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(partialName))
            {
                throw new ArgumentNullException("partialName");
            }

            ModelState.Clear();
            ViewData.Model = model;

            using (var strWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, partialName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, strWriter);
                viewResult.View.Render(viewContext, strWriter);
                return strWriter.GetStringBuilder().ToString();
            }
        }
    }
}
