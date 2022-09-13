using Autodesk.Revit.UI;
using SKRevitPluginAddSharedParameters.ViewModels;
using System.Windows;

namespace SKRevitPluginAddSharedParameters.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ExternalCommandData commandData)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(commandData);
        }
    }
}
