using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace SKRevitPluginAddSharedParameters.Models
{
    public class Helper
    {

        public Dictionary<string, string> parameterGroupsDictionary = new Dictionary<string, string>()
        {
            {"Analysis Results", "PG_ANALYSIS_RESULTS"},
            {"Analytical Alignment", "PG_ANALYTICAL_ALIGNMENT"},
            {"Analytical Model", "PG_ANALYTICAL_MODEL"},
            {"Constraints", "PG_CONSTRAINTS"},
            {"Construction", "PG_CONSTRUCTION"},
            {"Data", "PG_DATA"},
            {"Dimensions", "PG_GEOMETRY"},
            {"Division Geometry", "PG_DIVISION_GEOMETRY"},
            {"Electrical", "PG_ELECTRICAL"},
            {"Electrical - Circuiting", "PG_ELECTRICAL_CIRCUITING"},
            {"Electrical - Lighting", "PG_ELECTRICAL_LIGHTING"},
            {"Electrical - Loads", "PG_ELECTRICAL_LOADS"},
            {"Electrical Analysis", "PG_ELECTRICAL_ANALYSIS"},
            {"Electrical Engineering", "PG_ELECTRICAL_ENGINEERING"},
            {"Energy Analysis", "PG_ENERGY_ANALYSIS"},
            {"Fire Protection", "PG_FIRE_PROTECTION"},
            {"Forces", "PG_FORCES"},
            {"General", "PG_GENERAL"},
            {"Graphics", "PG_GRAPHICS"},
            {"Green Building Properties", "PG_GREEN_BUILDING"},
            {"Identity Data", "PG_IDENTITY_DATA"},
            {"IFC Parameters", "PG_IFC"},
            {"Layers", "PG_REBAR_SYSTEM_LAYERS"},
            {"Materials and Finishes", "PG_MATERIALS"},
            {"Mechanical", "PG_MECHANICAL"},
            {"Mechanical - Flow", "PG_MECHANICAL_AIRFLOW"},
            {"Mechanical - Loads", "PG_MECHANICAL_LOADS"},
            {"Model Properties", "PG_ADSK_MODEL_PROPERTIES"},
            {"Moments", "PG_MOMENTS"},
            {"Other", "INVALID"},
            {"Overall Legend", "PG_OVERALL_LEGEND"},
            {"Phasing", "PG_PHASING"},
            {"Photometrics", "PG_LIGHT_PHOTOMETRICS"},
            {"Plumbing", "PG_PLUMBING"},
            {"Primary End", "PG_PRIMARY_END"},
            {"Rebar Set", "PG_REBAR_ARRAY"},
            {"Releases / Member Forces", "PG_RELEASES_MEMBER_FORCES"},
            {"Secondary End", "PG_SECONDARY_END"},
            {"Segments and Fittings", "PG_SEGMENTS_FITTINGS"},
            {"Set", "PG_COUPLER_ARRAY"},
            {"Slab Shape Edit", "PG_SLAB_SHAPE_EDIT"},
            {"Structural", "PG_STRUCTURAL"},
            {"Structural Analysis", "PG_STRUCTURAL_ANALYSIS"},
            {"Text", "PG_TEXT"},
            {"Title Text", "PG_TITLE"},
            {"Visibility", "PG_VISIBILITY"}
        };


        //TODO: EVENTUALLY ADD POSSIBILITY FOR OPEN XLSX FILES

        public static List<string[]> GetAllParametersFromExcel(string path)
        {
            List<string[]> parameters = new List<string[]> ();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var firstSheet = package.Workbook.Worksheets["Parameters"];
                bool isEmpty = false;
                int itemIndex = 1;
                while (!isEmpty)
                {
                    string parameterName = firstSheet.Cells[$"A{itemIndex}"].Text;
                    if (!string.IsNullOrEmpty(parameterName))
                    {
                        string parameterGroupInSharedParamsFile = firstSheet.Cells[$"B{itemIndex}"].Text;
                        string parameterGroup = firstSheet.Cells[$"C{itemIndex}"].Text;
                        string typeInstance = firstSheet.Cells[$"D{itemIndex}"].Text;

                        string[] parameter = new string[] { parameterName, parameterGroupInSharedParamsFile, parameterGroup, typeInstance };
                        parameters.Add(parameter);
                        itemIndex++;
                    }
                    else
                    {
                        isEmpty = true;
                    }
                }
            }
            return parameters;
        }
    }
}
