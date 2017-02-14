using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QS.ALM.CloudShellApi
{
    class APIExplorerResult
    {  
        public string Name { get; private set; }
        public string Type { get; private set; } // “Test” or “Folder”

        public APIExplorerResult(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
    
    class ArrAPIExplorerResult
    {
        public APIExplorerResult[] Children;
    }

}
