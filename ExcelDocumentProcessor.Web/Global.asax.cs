using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;

namespace ExcelDocumentProcessor.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.CreateMap<ApplicationLogic.Proxies.DataService.NeonClient, WebClient>();
            Mapper.CreateMap<ApplicationLogic.Proxies.DataService.NeonISGDatabaseType, WebISGDatabaseType>();
            Mapper.CreateMap<WebISGDatabaseType, ApplicationLogic.Proxies.DataService.NeonISGDatabaseType>();
            Mapper.CreateMap<ApplicationLogic.Proxies.DataService.NeonYearQuarter, WebYearQuarter>();
            Mapper.CreateMap<WebYearQuarter, ApplicationLogic.Proxies.DataService.NeonYearQuarter>();

            Mapper.CreateMap<ApplicationLogic.Proxies.FileService.NeonFSISGDatabaseType, WebISGDatabaseType>();
            Mapper.CreateMap<WebISGDatabaseType, ApplicationLogic.Proxies.FileService.NeonFSISGDatabaseType>();

            Mapper.CreateMap<ApplicationLogic.Proxies.FileService.NeonFSDataTableMetaData, WebDataTableMetaData>();
            Mapper.CreateMap<ApplicationLogic.Proxies.FileService.NeonFSDataTableColumn, WebDataTableColumn>();
            Mapper.CreateMap<WebDataTableMetaData, ApplicationLogic.Proxies.FileService.NeonFSDataTableMetaData>();
            Mapper.CreateMap<WebDataTableColumn, ApplicationLogic.Proxies.FileService.NeonFSDataTableColumn>();

            Mapper.CreateMap<ApplicationLogic.Proxies.FileService.NeonFSUniverse, WebUniverse>();
            Mapper.CreateMap<ApplicationLogic.Proxies.FileService.NeonFSUniverseMap, WebUniverseMap>();
            Mapper.CreateMap<ApplicationLogic.Proxies.FileService.NeonFSDataColumnBuilder, WebDataColumnBuilder>();
            Mapper.CreateMap<WebUniverse, ApplicationLogic.Proxies.FileService.NeonFSUniverse>();
            Mapper.CreateMap<WebUniverseMap, ApplicationLogic.Proxies.FileService.NeonFSUniverseMap>();
            Mapper.CreateMap<WebDataColumnBuilder, ApplicationLogic.Proxies.FileService.NeonFSDataColumnBuilder>();

            AreaRegistration.RegisterAllAreas();

            //todo: fix this
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}