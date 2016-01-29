using System;
using System.Linq;
using System.Web.Mvc;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.Models;

namespace ExcelDocumentProcessor.Web.Controllers
{
    public class FundController : BaseGridController
    {
        public ActionResult Index()
        {
            const WebISGDatabaseType databaseType = WebISGDatabaseType.ISGOutput;
            var fundModel =
                new FundModel
                    {
                        DatabaseType = databaseType,
                        HasLoaded = true
                    };

            foreach (var g in fundModel.GridModel.Grids)
            {
                var metaData =
                    WebCache.IsgMetaData.FirstOrDefault(
                        x =>
                        string.Equals(x.Name, g.TableName, StringComparison.CurrentCultureIgnoreCase) &&
                        x.DatabaseType == fundModel.DatabaseType);

                if (metaData == null)
                {
                    continue;
                }

                g.DatabaseType = fundModel.DatabaseType;
                g.JqGridNonEditableColumns = metaData.JqGridNonEditableColumns;
                g.JqGridColumnNames = metaData.JqGridColumnNames;
                g.JqGridColumns = metaData.JqGridColumns;
                g.ControllerName = fundModel.ControllerName;

            }

            return View("Index", fundModel);
        }
    }
}
