using System.Web.Mvc;

namespace ExcelDocumentProcessor.Web.Controllers
{
    public class SecurityIssuesController : Controller
    {
        public ActionResult Unauthenticated()
        {
            return View();
        }
        public ActionResult Unauthorized()
        {
            return PartialView();
        }
    }
}
