using ExcelDocumentProcessor.ExcelDocumentCleaner.Constants;
using ExcelDocumentProcessor.ExcelDocumentCleaner.Processors.Interfaces;

namespace ExcelDocumentProcessor.ExcelDocumentCleaner.Processors
{
    public class ProcessorFactory
    {
        public static IProcessor GetProcessor(string sheetName, string fileName)
        {
            IProcessor rtrn = null;

            if (sheetName.EndsWith(ExcelTabName.Main))
            {
                rtrn =
                    new Standard
                        {
                            HeaderRowIndex = 1,
                            BodyStartRowIndex = 2
                        };
            }
            else if (sheetName.EndsWith(ExcelTabName.ParentMigration) && fileName.Contains("VA"))
            {
                rtrn = new ParentMigrationVA();
            }
            else if (sheetName.EndsWith(ExcelTabName.ParentMigration))
            {
                rtrn =
                    new Standard
                    {
                        HeaderRowIndex = 3,
                        BodyStartRowIndex = 4
                    };
            }
            else if (sheetName.EndsWith(ExcelTabName.ParentAndPgMaster))
            {
                rtrn =
                    new Standard
                        {
                            HeaderRowIndex = 2,
                            BodyStartRowIndex = 4
                        };
            }
            else if (sheetName.EndsWith(ExcelTabName.PeerGroupData))
            {
                rtrn = new PeerGroupData();
            }
            else if (sheetName.EndsWith(ExcelTabName.Summary))
            {
                rtrn =
                    new Standard
                        {
                            HeaderRowIndex = 2,
                            BodyStartRowIndex = 4
                        };
            }
            else if (sheetName.EndsWith(ExcelTabName.GroupDataAndBmkSets))
            {
                rtrn = new GroupDataAndBmkSets();
            }
            else if (sheetName.EndsWith(ExcelTabName.DistinctManagers))
            {
                rtrn = new DistinctManagers();
            }

            if (rtrn != null)
            {
                rtrn.SheetName = sheetName;
            }

            return rtrn;
        }
    }
}
