using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace SKRevitPluginAddSharedParameters
{
    class App : IExternalApplication
    {
        // define a method that will create our tab and button
        static void AddRibbonPanel(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            const string RIBBON_TAB = "SKTools";
            const string RIBBON_PANEL = "Tools";
            try
            {
                application.CreateRibbonTab(RIBBON_TAB);
            }
            catch (Exception) { } //tab already exists

            // Get or create the ribbon panel
            RibbonPanel ribbonPanel = null;
            List<RibbonPanel> panels = application.GetRibbonPanels(RIBBON_TAB);

            foreach (RibbonPanel panel in panels)
            {
                if (panel.Name == RIBBON_PANEL)
                {
                    ribbonPanel = panel;
                    break;
                }
            }

            //Create ribbon panel if you can't find it
            if (ribbonPanel == null)
            {
                ribbonPanel = application.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL);
            }

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // create push button for CurveTotalLength
            PushButtonData b1Data = new PushButtonData(
                "Add Shared Parameters",
                "Add Shared Parameters",
                thisAssemblyPath,
                "SKRevitPluginAddSharedParameters.StartClassPlugin");

            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Add Shared Parameters";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/SKRevitPluginAddSharedParameters;component/Resources/addParametersImage30x32.png"));
            pb1.LargeImage = pb1Image;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // do nothing
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // call our method that will load up our toolbar
            AddRibbonPanel(application);
            return Result.Succeeded;
        }
    }
}
