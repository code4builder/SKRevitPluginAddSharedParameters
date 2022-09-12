using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
//using TextFilesDealer;
//using WinDialogUtils;

namespace SKRevitPluginAddSharedParameters.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ICommand SelectParameterFile { get; set; }

        public ExternalCommandData CommandData { get; set; }

        public Document Document => this.CommandData?.Application.ActiveUIDocument.Document;

        public Application Application => this.Document?.Application;

        public FamilyManager FamilyManager => this.Document?.FamilyManager;

        public bool IsFamilyDocument => this.Document != null && this.Document.IsFamilyDocument;

        public DefinitionFile SharedParamFile => this.Document?.Application?.OpenSharedParameterFile();

        public DelegateCommand OpenFileCommand { get; private set; }
        public DelegateCommand AddParametersCommand { get; private set; }

        public MainWindowViewModel() { }
        public MainWindowViewModel(ExternalCommandData commandData)
        {
            this.CommandData = commandData;

            OpenFileCommand = new DelegateCommand(OpenFile);
            AddParametersCommand = new DelegateCommand(AddParameters);
        }

        private void OpenFile()
        {
            if (!this.IsFamilyDocument || this.SharedParamFile == null)
                return;

            string filePath = GetFilePath();
            if (string.IsNullOrEmpty(filePath))
                return;

            //foreach (List<string> stringList in new TextData(filePath).ReadFile(';', 0, Encoding.UTF8))
            var linesFromCSV = File.ReadAllLines(filePath).Select(a => a.Split(','));
            foreach (var line  in linesFromCSV)
            {
                this.AddSharedParam(line[0], line[1], line[2], line[3]);
            }

        }

        private void AddParameters()
        {
            //add method for command
        }

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
    }
}
