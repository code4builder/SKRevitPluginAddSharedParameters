using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKRevitPluginAddSharedParameters.Models
{
    public class ParameterDefinition
    {
        public string ParameterName { get; set; }
        public string ParameterGroupInSharedParamsFile { get; set; }

        public string ParameterGroup { get; set; }
        public string TypeInstance { get; set; }

        public ParameterDefinition(string parameterName, string parameterGroupInSharedParamsFile, string parameterGroup, string typeInstance)
        {
            ParameterName = parameterName;
            ParameterGroupInSharedParamsFile = parameterGroupInSharedParamsFile;
            ParameterGroup = parameterGroup;
            TypeInstance = typeInstance;
        }

    }
}
