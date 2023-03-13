using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.API
{
    public class StaffParameters
    {
        public string filterTasks { get; set; } = "Мои задачи";

        public string filterPosts { get; set; } = "";
        public string filterStaffs { get; set; } = "";
    }
}
