using VCRevitRibbonUtil;
using Autodesk.Revit.UI;
using RP_test.Properties;


namespace RP_test
{
    internal class MainPanel : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            Ribbon.GetApplicationRibbon(a).Tab("Плагины") 
                .Panel("Помещение") 
                .CreateButton<Test>("text3", "Расстановка марок", b => b.SetLargeImage(Resources.RP_logo32)
                .SetSmallImage(Resources.RP_logo16)
                .SetLongDescription("Позволяет расставить марки в помещения")); 
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}