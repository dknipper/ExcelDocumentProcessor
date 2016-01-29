using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers
{
    public class SelectLists
    {
        
        public static IList<SelectListItem> YearQuarters
        {
            get
            {
                var yearQuarterListItems = new List<SelectListItem>();
                WebCache.YearQuarters.ForEach(
                    x =>
                    yearQuarterListItems.Add(
                        new SelectListItem
                            {
                                Value = x.Quarter,
                                Text = x.Quarter
                            })
                    );
                return yearQuarterListItems;
            }
        }

        public static string DefaultYearQuarter
        {
            get
            {
                string rtrn = null;
                var firstYearQuarter = WebCache.YearQuarters.FirstOrDefault();
                if (firstYearQuarter != null)
                {
                    rtrn = firstYearQuarter.Quarter;
                }
                return rtrn;
            }
        }

        public static IList<SelectListItem> Clients
        {
            get
            {
                var clientsListItems = new List<SelectListItem>();
                WebCache.Clients.ForEach(
                    x =>
                    clientsListItems.Add(
                        new SelectListItem
                            {
                                Value = x.ClientKey.ToString(CultureInfo.CurrentCulture),
                                Text = x.ClientName
                            })
                    );
                return clientsListItems;
            }
        }

        public static int DefaultClient
        {
            get
            {
                var rtrn = -1;
                var firstClient = WebCache.Clients.FirstOrDefault();
                if (firstClient != null)
                {
                    rtrn = firstClient.ClientKey;
                }
                return rtrn;
            }
        }

        public static IList<SelectListItem> Universes
        {
            get
            {
                var universeListItems = new List<SelectListItem>();
                WebCache.Universes.ForEach(
                    x =>
                    universeListItems.Add(
                        new SelectListItem
                            {
                                Value = x.UniverseName,
                                Text = x.UniverseName
                            })
                    );
                return universeListItems;
            }
        }

        public static string DefaultUniverse
        {
            get
            {
                string rtrn = null;
                var universe = WebCache.Universes.FirstOrDefault();
                if (universe != null)
                {
                    rtrn = universe.UniverseName;
                }
                return rtrn;
            }
        }
    }
}