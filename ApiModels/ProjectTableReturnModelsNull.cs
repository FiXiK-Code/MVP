using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.ApiModels
{
    public class ProjectTableReturnModelsNull : ProjectTableReturnModels
    {
        public List<string> filterProj { get; set; }
        public List<string> filterGip { get; set; }
        public List<string> filterResipirnt { get; set; }

    }
}
