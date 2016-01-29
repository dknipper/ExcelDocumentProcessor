using System;
using System.Linq;
using System.Web.Mvc;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.Models;

namespace ExcelDocumentProcessor.Web.Controllers
{
    public class ClientController : BaseGridController
    {
        public ActionResult Index(ClientModel clientModel)
        {
            const WebISGDatabaseType databaseType = WebISGDatabaseType.ISGOutput;
            clientModel =
                (clientModel == null || clientModel.HasLoaded == false)
                    ? new ClientModel
                        {
                            DatabaseType = databaseType,
                            HasLoaded = true
                        }
                    : clientModel;

            return View("Index", clientModel);
        }

        public ActionResult SelectClient(ClientModel clientModel)
        {
            var metaData = WebCache.IsgMetaData.FirstOrDefault(x => string.Equals(x.Name, clientModel.SelectedClient.InputTable, StringComparison.CurrentCultureIgnoreCase) && x.DatabaseType == clientModel.DatabaseType);
            if (metaData != null)
            {
                clientModel.GridModel.Grids.Add(
                    new GridModel
                        {
                            DatabaseType = clientModel.DatabaseType,
                            JqGridNonEditableColumns = metaData.JqGridNonEditableColumns,
                            JqGridColumnNames = metaData.JqGridColumnNames,
                            JqGridColumns = metaData.JqGridColumns,
                            TableName = clientModel.SelectedClient.InputTable,
                            FriendlyTableName = clientModel.SelectedClient.FriendlyClientName,
                            ControllerName = clientModel.ControllerName
                        });
            }

            return View("Index", clientModel);
        }

        public override ActionResult SaveGridData(string tableName, string postData, string databaseType)
        {
            var retVal = base.SaveGridData(tableName, postData, databaseType);
            WebCache.InvalidateCache("metaData");
            WebCache.InvalidateCache("clients");
            return retVal;
        }
    }
}
