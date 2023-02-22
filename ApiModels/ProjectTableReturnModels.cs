﻿using MVP.Date.API;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.ApiModels
{
    public class ProjectTableReturnModels
    {
        public List<ProjectOut> projects { get; set; }
        public List<TasksOut> completed { get; set; }
        public List<TasksOut> today { get; set; }
        public List<TasksOut> future { get; set; }
    }
}
