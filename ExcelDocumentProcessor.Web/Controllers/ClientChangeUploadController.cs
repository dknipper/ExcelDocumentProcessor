using System.Web;
using System.Web.Mvc;
using ExcelDocumentProcessor.Web.Models;

namespace ExcelDocumentProcessor.Web.Controllers
{
    public class ClientChangeUploadController : Controller
    {
        public ActionResult Index(ClientChangeUploadModel testUploadModel)
        {
            testUploadModel =
                (testUploadModel == null || testUploadModel.HasLoaded == false)
                    ? new ClientChangeUploadModel
                        {
                            HasLoaded = true
                        }
                    : testUploadModel;

            return View(testUploadModel);
        }

        public ActionResult FileUpload(ClientChangeUploadModel testUploadModel, HttpPostedFileBase clientFile)
        {
            testUploadModel.UploadFile(clientFile);
            return View("Index", testUploadModel);
        }
    }
}
