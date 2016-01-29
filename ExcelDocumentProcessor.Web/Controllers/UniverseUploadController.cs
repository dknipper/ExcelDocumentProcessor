using System.Web;
using System.Web.Mvc;
using ExcelDocumentProcessor.Web.Models;

namespace ExcelDocumentProcessor.Web.Controllers
{
    public class UniverseUploadController : BaseController
    {
        public ActionResult Index(UniverseUploadModel testUploadModel)
        {
            testUploadModel = (testUploadModel == null || testUploadModel.HasLoaded == false) ? new UniverseUploadModel{HasLoaded = true} : testUploadModel;
            return View(testUploadModel);
        }

        public ActionResult FileUpload(UniverseUploadModel testUploadModel, HttpPostedFileBase universeFile)
        {
            testUploadModel.UploadFile(universeFile);
            return View("Index", testUploadModel);
        }
    }
}
