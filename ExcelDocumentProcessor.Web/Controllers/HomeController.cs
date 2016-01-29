using System.Web.Mvc;
using ExcelDocumentProcessor.Web.Models;

namespace ExcelDocumentProcessor.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string tableName)
        {
            var homeModel =
                new HomeModel
                {
                    HasLoaded = true
                };

            homeModel.GridModel.Grids.Add(
                new GridModel
                {
                    JqGridColumnNames = homeModel.JqGridColumnNames,
                    JqGridColumns = homeModel.JqGridColumns,
                    TableName = "currentQuarter",
                    FriendlyTableName = "Current Quarter",
                    ControllerName = homeModel.ControllerName
                }
            );

            homeModel.GridModel.Grids.Add(
                new GridModel
                {
                    JqGridColumnNames = homeModel.JqGridColumnNames,
                    JqGridColumns = homeModel.JqGridColumns,
                    TableName = "priorExecution",
                    FriendlyTableName = "Prior Execution",
                    ControllerName = homeModel.ControllerName
                }
            );

            return View("Index", homeModel);
        }
    }
}
