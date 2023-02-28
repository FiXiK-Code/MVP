using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.ApiModels
{
    public class StaffTableReturnModelsNull : StaffTableReturnModels
    {
        public List<string> filterTasks { get; set; }
        public List<string> filterPosts { get; set; }
        public List<string> filterStaffs { get; set; }
    }
}
