using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using SKRevitPluginAddSharedParameters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace SKRevitPluginAddSharedParameters.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region PROPERTIES AND FIELDS
        private ExternalCommandData CommandData { get; set; }
        private Document Document => CommandData?.Application.ActiveUIDocument.Document;
        private FamilyManager FamilyManager => Document?.FamilyManager;
        private bool IsFamilyDocument => Document != null && Document.IsFamilyDocument;
        private DefinitionFile SharedParamFile => Document?.Application?.OpenSharedParameterFile();
        private IEnumerable<string[]> _linesFromCSV;
        #endregion

        public MainWindowViewModel(ExternalCommandData commandData)
        {
            CommandData = commandData;
        }

        #region COMMANDS
        private RelayCommand _openFileCommand;
        public RelayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new RelayCommand(obj =>
                {
                    if (IsFamilyDocument == false)
                    {
                        MessageBox.Show("Open a family. This is not a family file");
                        return;
                    }

                    if (SharedParamFile == null)
                    {
                        MessageBox.Show("Add shared parameters file");
                        return;
                    }

                    string filePath = GetFilePath();
                    if (string.IsNullOrEmpty(filePath))
                        return;

                    _linesFromCSV = File.ReadAllLines(filePath).Select(a => a.Split(';'));
                }
                ));
            }
        }

        private RelayCommand _addParametersCommand;
        public RelayCommand AddParametersCommand
        {
            get
            {
                return _addParametersCommand ?? (_addParametersCommand = new RelayCommand(obj =>
                {
                    foreach (var line in _linesFromCSV)
                    {
                        AddSharedParam(line[0], line[1], line[2], line[3]);
                    }
                    MessageBox.Show("Parameters have been added");
                }
                ));
            }
        }
        #endregion

        #region METHODS
        private void AddSharedParam(
                                      string parameterName,
                                      string groupName,
                                      string paramGroupName,
                                      string instanceOrTypeValue)
        {
            using (Transaction transaction = new Transaction(this.Document, "Add params"))
            {
                int num1 = (int)transaction.Start();
                DefinitionGroup definitionGroup = this.SharedParamFile.Groups.FirstOrDefault<DefinitionGroup>((Func<DefinitionGroup, bool>)(g => g.Name.Equals(groupName)));
                if (definitionGroup == null || !(definitionGroup.Definitions.FirstOrDefault<Definition>((Func<Definition, bool>)(d => d.Name.Equals(parameterName))) is ExternalDefinition familyDefinition2))
                    return;
                IList<FamilyParameter> parameters = this.FamilyManager.GetParameters();
                bool isExistingParameter = false;
                foreach (FamilyParameter familyParameter in (IEnumerable<FamilyParameter>)parameters)
                {
                    if (familyParameter.Definition.Name.Equals(familyDefinition2.Name))
                    {
                        isExistingParameter = true;
                        break;
                    }
                }
                BuiltInParameterGroup result;
                if (isExistingParameter || !Enum.TryParse<BuiltInParameterGroup>(paramGroupName, out result))
                    return;
                bool isInstance = instanceOrTypeValue.Equals("instance");
                this.FamilyManager.AddParameter(familyDefinition2, result, isInstance);
                int num2 = (int)transaction.Commit();
            }
        }

        private string GetFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Comma Separated Values File|*.csv";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            string filePath = openFileDialog.FileName;
            return filePath;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
