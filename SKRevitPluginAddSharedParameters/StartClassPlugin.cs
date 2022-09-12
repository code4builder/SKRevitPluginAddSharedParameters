using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using SKRevitPluginAddSharedParameters.Views;
using SKRevitPluginAddSharedParameters.ViewModels;

namespace SKRevitPluginAddSharedParameters
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class StartClassPlugin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(commandData);
            MainWindow mainWindow = new MainWindow();

            mainWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            mainWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}
