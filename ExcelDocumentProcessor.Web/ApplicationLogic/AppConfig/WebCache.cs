using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using AutoMapper;
using ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.Proxies.DataService;
using ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig
{
    public class WebCache
    {
        public static List<WebClient> Clients
        {
            get
            {
                const string configKey = "clients";
                if (HttpRuntime.Cache[configKey] == null)
                {
                    using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
                    {
                        var mesClients = serviceClient.GetActiveClients();
                        var clients = Mapper.Map<List<NeonClient>, List<WebClient>>(mesClients);
                        HttpRuntime.Cache.Insert(configKey, clients, null, Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }
                }
                return (List<WebClient>)HttpRuntime.Cache[configKey];
            }
        }

        public static List<WebUniverse> Universes
        {
            get
            {
                const string configKey = "universes";
                if (HttpRuntime.Cache[configKey] == null)
                {
                    using (var context = new Entities.SystemGenerated.UIConfigEntities(Configuration.UIConfigEFConnectionString))
                    {
                        var universeMaps = context.FundTypeMaps.ToList().OrderBy(x => x.FundType.Name);

                        var universes = new List<WebUniverse>();
                        var universe = new WebUniverse();
                        foreach (var universeMap in universeMaps)
                        {
                            if (!string.Equals(universeMap.FundType.Name, universe.UniverseName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                universe =
                                    new WebUniverse
                                        {
                                            UniverseName = universeMap.FundType.Name,
                                            UniverseMaps = new List<WebUniverseMap>()
                                        };
                                universes.Add(universe);
                            }

                            universe.UniverseMaps.Add(
                                new WebUniverseMap
                                    {
                                        UniverseTemplateKey = universeMap.FundTypeTemplateKey.Name,
                                        InputTable = universeMap.DBTable.Name,
                                        LoadISGDatabase = (WebISGDatabaseType)Enum.Parse(typeof(WebISGDatabaseType), universeMap.ISGDB.Name, true),
                                        QuarterInName = (universeMap.FundTypeTemplateKey.QuarterInName.HasValue && universeMap.FundTypeTemplateKey.QuarterInName.Value)
                                    });
                        }

                        var cacheFileDependancies =
                            new CacheDependency(
                                new[]
                                    {
                                        Configuration.UIConfigFileLocation
                                    });

                        HttpRuntime.Cache.Insert(configKey, universes, cacheFileDependancies, Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }
                }
                return (List<WebUniverse>)HttpRuntime.Cache[configKey];
            }
        }

        public static List<WebYearQuarter> YearQuarters
        {
            get
            {
                const string configKey = "yearquarters";
                if (HttpRuntime.Cache[configKey] == null)
                {
                    using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
                    {
                        var mesYearQuarters = serviceClient.GetYearQuarters();
                        var yearQuarters = Mapper.Map<List<NeonYearQuarter>, List<WebYearQuarter>>(mesYearQuarters);
                        HttpRuntime.Cache.Insert(configKey, yearQuarters, null, Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }
                }
                return (List<WebYearQuarter>)HttpRuntime.Cache[configKey];
            }
        }

        public static List<GridColumnMapping> GridColumnConfiguration
        {
            get
        {
            const string configKey = "gridColumnConfig";
            if (HttpRuntime.Cache[configKey] == null)
            {
                using (var context = new Entities.SystemGenerated.UIConfigEntities(Configuration.UIConfigEFConnectionString))
                {
                    var configSettings = context.GridColumnConfigs;
                    var gridColumns = new List<GridColumnMapping>();

                    if (configSettings != null)
                    {
                        configSettings.ToList().ForEach(
                            x => gridColumns.Add(
                                new GridColumnMapping
                                    {
                                        DBTableColumnName = x.DBTableColumnName,
                                        DBTableName = x.DBTable.Name,
                                        FriendlyName = x.FriendlyName,
                                        Hidden = (x.Hidden.HasValue && x.Hidden.Value),
                                        Order = ((x.Order.HasValue == false || x.Order == 0) ? int.MaxValue : x.Order.Value),
                                        Width = x.Width,
                                        Sortable = (!x.Sortable.HasValue || (x.Sortable.Value)),
                                        Editable = (!x.Editable.HasValue || (x.Editable.Value)),
                                        Frozen = (x.Frozen.HasValue && x.Frozen.Value),
                                        SearchType = (x.SearchType != null) ? x.SearchType.Name : string.Empty
                                    }));

                        var cacheFileDependancies =
                            new CacheDependency(
                                new[]
                                    {
                                        Configuration.UIConfigFileLocation
                                    });

                        HttpRuntime.Cache.Insert(configKey, gridColumns, cacheFileDependancies, Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }
                }
            }
            return (List<GridColumnMapping>)HttpRuntime.Cache[configKey];
        }
        }

        public static List<ISGColumnMapping> IsgColumnMappings
        {
            get
        {
            const string configKey = "isgColumnMappings";
            if (HttpRuntime.Cache[configKey] == null)
            {
                using (var context = new Entities.SystemGenerated.UIConfigEntities(Configuration.UIConfigEFConnectionString))
                {
                    var configSettings = context.ExcelDBTableColumnMaps;
                    var tableColumns = new List<ISGColumnMapping>();

                    if (configSettings != null)
                    {
                        configSettings.ToList().ForEach(
                            x => tableColumns.Add(
                                new ISGColumnMapping
                                    {
                                        ExcelColumnName = x.ExcelColumnName,
                                        TableColumnName = x.DBTableColumnName,
                                        TableName = x.DBTable.Name
                                    }));

                        var cacheFileDependancies =
                            new CacheDependency(
                                new[]
                                    {
                                        Configuration.UIConfigFileLocation
                                    });

                        HttpRuntime.Cache.Insert(configKey, tableColumns, cacheFileDependancies, Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }

                }
            }
            return (List<ISGColumnMapping>)HttpRuntime.Cache[configKey];
        }
    }

        public static List<WebDataTableMetaData> IsgMetaData
        {
            get
            {
                const string configKey = "metaData";
                if (HttpRuntime.Cache[configKey] == null)
                {
                    using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
                    {
                        var mesClients = serviceClient.GetAllMetaData();
                        var metaDataList = new List<WebDataTableMetaData>();
                        mesClients.ForEach(x => metaDataList.Add(new WebDataTableMetaData(x)));
                        HttpRuntime.Cache.Insert(configKey, metaDataList, null, Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }
                }
                return (List<WebDataTableMetaData>)HttpRuntime.Cache[configKey];
            }
        }

        public static void InvalidateCache(string cache)
        {
            HttpRuntime.Cache.Remove(cache);
        }
    }
}