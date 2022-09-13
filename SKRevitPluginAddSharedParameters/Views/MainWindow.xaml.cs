using Autodesk.Revit.DB;
using SKRevitPluginAddSharedParameters.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using System.IO;
using System.Linq;

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
