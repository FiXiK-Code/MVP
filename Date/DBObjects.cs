using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = MVP.Date.Models.Tasks;
using TaskStatus = MVP.Date.Models.TaskStatus;

namespace MVP.Date
{
    public class DBObjects
    {
        public static void Initial(AppDB content)
        {

            if (!content.DBCompanyStructure.Any())
                content.DBCompanyStructure.AddRange(ComStuct.Select(p => p.Value));

            if (!content.DBDivision.Any())
                content.DBDivision.AddRange(Div.Select(p => p.Value));

            if (!content.DBPost.Any())
                content.DBPost.AddRange(Post.Select(p => p.Value));

            if (!content.DBProject.Any())
            {
                content.AddRange(
                    new Project
                    {
                        code = "17/22 - TCP",
                        name = "Длинное название проекта",
                        shortName = "Проект TCP",
                        priority = 1,
                        dateStart = new DateTime(2022, 12, 23, 15, 0, 0),
                        plannedFinishDate = new DateTime(2023, 01, 23, 15, 0, 0),
                        actualFinishDate = new DateTime(2021),
                        supervisor = "",
                        link = "https://docs.google.com/document/d/1rCaPpc_Sddw-tLANQGTIKWDSpdBCoAUCy6-l8SRjGFA/edit#",
                        history = null,
                        archive = "Нет",
                        nowStage = "Инициализация"
                    }
                );
            }

            if (!content.DBRole.Any())//затык с MokRole (List<string>)
                content.DBRole.AddRange(Role.Select(p => p.Value));

            if (!content.DBStaff.Any())
                content.DBStaff.AddRange(Staff.Select(p => p.Value));

            if (!content.DBStage.Any())
                content.DBStage.AddRange(Stage.Select(p => p.Value));

            if (!content.DBTask.Any())
            {
                content.AddRange(
                     new Task
                     {
                         code = "test",
                         desc = "Тестовая задача",
                         projectCode = "17/22 - TCP",
                         supervisor = "Смирнов АА",
                         recipient = "Иванов ПП",
                         priority = 1,
                         comment = null,
                         plannedTime = new TimeSpan(6,20,0),
                         actualTime = new TimeSpan(0,0,0),
                         start = new DateTime(2022, 12, 16, 15, 0, 0),
                         finish = new DateTime(2022),
                         date = new DateTime(2022, 12, 14, 15, 0, 0),
                         Stage = "Инициализация",
                         status = "На паузе",
                         liteTask =false
                     },
                    new Task
                    {
                        code = "test1",
                        desc = "Тестовая задача 2",
                        projectCode = "17/22 - TCP",
                        supervisor = "Смирнов АА",
                        recipient = "Иванов ПП",
                        priority = 1,
                        comment = null,
                        plannedTime = new TimeSpan(3,30, 0),
                        actualTime = new TimeSpan(),
                        start = new DateTime(2022, 12, 16, 15, 0, 0),
                        finish = new DateTime(2022),
                        date = new DateTime(2022, 12, 14, 15, 0, 0),
                        Stage = "Инициализация",
                        status = "Создана",
                        liteTask = false,
                    }
                );
            }

            if (!content.DBTaskStatus.Any())
                content.DBTaskStatus.AddRange(TaskStat.Select(p => p.Value));
            


            content.SaveChanges();
        }




        public static Dictionary<string, Division> _Div;
        public static Dictionary<string, Division> Div
        {
            get
            {

                if (_Div == null)
                {
                    var list = new Division[]
                    {
                         new Division {
                        code = "01",
                        name = "Управление"
                    },
                    new Division {
                        code = "02",
                        name = "Отдел проектирования"
                    },
                    new Division {
                        code = "03",
                        name = "Отдел изысканий"
                    }
                    };

                    _Div = new Dictionary<string, Division>();
                    foreach (Division el in list)
                    {
                        _Div.Add(el.code, el);
                    }
                }
                return _Div;
            }
        }



        public static Dictionary<string, TaskStatus> _TaskStat;
        public static Dictionary<string, TaskStatus> TaskStat
        {
            get
            {

                if (_TaskStat == null)
                {
                    var list = new TaskStatus[]
                    {
                        new TaskStatus { name = "Создана" },
                        new TaskStatus { name = "В работе" },
                        new TaskStatus { name = "На паузе" },
                        new TaskStatus { name = "Выполнена" }

                    };
                    _TaskStat = new Dictionary<string, TaskStatus>();
                    foreach (TaskStatus el in list)
                    {
                        _TaskStat.Add(el.name, el);
                    }
                }
                return _TaskStat;
            }
        }


        public static Dictionary<int, LogistickTask> _LogTask;
        public static Dictionary<int, LogistickTask> LogTask
        {
            get
            {

                if (_LogTask == null)
                {
                    var list = new LogistickTask[]
                    {
                        new LogistickTask (){}

                    };
                    _LogTask = new Dictionary<int, LogistickTask>();
                    foreach (LogistickTask el in list)
                    {
                        _LogTask.Add(el.id, el);
                    }
                }
                return _LogTask;
            }
        }

        public static Dictionary<int, LogisticProject> _LogProj;
        public static Dictionary<int, LogisticProject> LogProj
        {
            get
            {

                if (_LogProj == null)
                {
                    var list = new LogisticProject[]
                    {
                        new LogisticProject (){}

                    };
                    _LogProj = new Dictionary<int, LogisticProject>();
                    foreach (LogisticProject el in list)
                    {
                        _LogProj.Add(el.id, el);
                    }
                }
                return _LogProj;
            }
        }


        public static Dictionary<string, Stage> _Stage;
        public static Dictionary<string, Stage> Stage
        {
            get
            {

                if (_Stage == null)
                {
                    var list = new Stage[]
                    {
                        new Stage {
                        projectId = 1,
                        name = "Инициализация"
                    },
                    new Stage {
                        projectId = 1,
                        name = "Планирование"
                    },
                    new Stage {
                        projectId = 1,
                        name = "Реализация"
                    },
                    new Stage {
                        projectId = 1,
                        name = "Завершение"
                    }

                    };
                    _Stage = new Dictionary<string, Stage>();
                    foreach (Stage el in list)
                    {
                        _Stage.Add(el.name, el);
                    }
                }
                return _Stage;
            }
        }


        public static Dictionary<int, CompanyStructure> _ComStuct;
        public static Dictionary<int, CompanyStructure> ComStuct
        {
            get
            {

                if (_ComStuct == null)
                {
                    var list = new CompanyStructure[]
                    {
                        new CompanyStructure
                    {
                        divisionsId = 1,
                        supervisor = "Директор Петров"
                    },
                    new CompanyStructure
                    {
                        divisionsId = 2,
                        supervisor = "ГИП Смирнов"
                    },
                    new CompanyStructure
                    {
                        divisionsId = 3,
                        supervisor = "Но Иванов"
                    }
                    };
                    _ComStuct = new Dictionary<int, CompanyStructure>();
                    foreach (CompanyStructure el in list)
                    {
                        _ComStuct.Add(el.divisionsId, el);
                    }
                }
                return _ComStuct;
            }
        }


        public static Dictionary<string, Post> _Post;
        public static Dictionary<string, Post> Post
        {
            get
            {

                if (_Post == null)
                {
                    var list = new Post[]
                    {
                        new Post
                    {
                        code = "P01",
                        name = "Директор",
                        roleCod = "R01"
                    },
                    new Post
                    {
                        code = "P02",
                        name = "ГИП",
                        roleCod = "R02"
                    },
                    new Post
                    {
                        code = "P03",
                        name = "Помощник ГИПа",
                        roleCod = "R03"
                    },
                    new Post
                    {
                        code = "P04",
                        name = "НО",
                        roleCod = "R04"
                    },
                    new Post
                    {
                        code = "P05",
                        name = "РГ",
                        roleCod = "R05"
                    },
                    new Post
                    {
                        code = "P06",
                        name = "Сотрудник",
                        roleCod = "R06"
                    }
                    };
                    _Post = new Dictionary<string, Post>();
                    foreach (Post el in list)
                    {
                        _Post.Add(el.code, el);
                    }
                }
                return _Post;
            }
        }


        public static Dictionary<string, Staff> _Staff;
        public static Dictionary<string, Staff> Staff
        {
            get
            {

                if (_Staff == null)
                {
                    var list = new Staff[]
                    {
                        new Staff
                        {
                        code = "100",
                        name = "Зырянов ГС",
                        divisionId = 1,
                        roleId = 6,
                        post = "Директор",
                        login ="Директор",
                        passvord = "123456"
                        },

                        //////// ГИПы
                        new Staff
                        {
                        code = "101",
                        name = "Смирнов АА",
                        divisionId = 1,
                        supervisorId = 1,
                        roleId = 5,
                        post = "ГИП",
                        login ="ГИП",
                        passvord = "123456"
                        },
                        new Staff
                        {
                        code = "111",
                        name = "Алексеев ПП",
                        divisionId = 1,
                        supervisorId = 1,
                        roleId = 5,
                        post = "ГИП",
                        login ="ГИП",
                        passvord = "123456"
                        },


                        /////// Помощники ГИПов
                        new Staff
                        {
                        code = "102",
                        name = "Генадиев ДД",
                        divisionId = 1,
                        supervisorId = 1,
                        roleId = 4,
                        post = "Помощник ГИПа",
                        login ="Помощник ГИПа",
                        passvord = "123456"
                        },


                        /////// НО
                        new Staff
                        {
                        code = "103",
                        name = "Иванов ПП",
                        divisionId = 2,
                        roleId = 3,
                        post = "НО",
                        login = "НО",
                        passvord = "123456"
                        },
                        new Staff
                        {
                        code = "113",
                        name = "Марков КК",
                        divisionId = 2,
                        roleId = 3,
                        post = "НО",
                        login = "НО",
                        passvord = "123456"
                        },

                        //////// Сотрудники
                        new Staff
                        {
                        code = "104",
                        name = "Савин НН",
                        divisionId = 3,
                        supervisorId = 3,
                        roleId = 1,
                        post = "Сотрудник",
                        login = "Сотрудник",
                        passvord = "123456"
                        },
                        new Staff
                        {
                        code = "114",
                        name = "Димиденко ОО",
                        divisionId = 3,
                        supervisorId = 3,
                        roleId = 1,
                        post = "Сотрудник",
                        login = "Сотрудник",
                        passvord = "123456"
                        }
                    };
                    _Staff = new Dictionary<string, Staff>();
                    foreach (Staff el in list)
                    {
                        _Staff.Add(el.code, el);
                    }
                }
                return _Staff;
            }
        }


        public static Dictionary<string, Role> _Role; //затык с MokRole и тут (List<string>)
        public static Dictionary<string, Role> Role
        {
            get
            {

                if (_Role == null)
                {
                    var list = new Role[]
                    {
                         new Role {
                        code = "R01",
                        name = "Директор",
                        supervisor = null,/////
                        recipient = "ГИП,НО"////
                    },
                    new Role {
                        code = "R02",
                        name = "ГИП",
                        supervisor = "Дирктор,НО",
                        recipient = "Помощник ГИПа,НО"
                    },
                    new Role {
                        code = "R03",
                        name = "Помощник ГИПа",
                        supervisor = "ГИП,НО",
                        recipient = "НО"
                    },
                    new Role {
                        code = "R04",
                        name = "НО",
                        supervisor = "Директор,ГИП,Помощник ГИПа",
                        recipient = "ГИП,РГ,Сотрудник"
                    },
                    new Role {
                        code = "R05",
                        name = "РГ",
                        supervisor = "НО",
                        recipient = "Сотрудник"
                    },
                    new Role {
                        code = "R06",
                        name = "Сотрудник",
                        supervisor = "РГ,НО",
                        recipient = null
                    }
                    };

                    _Role = new Dictionary<string, Role>();
                    foreach (Role el in list)
                    {
                        _Role.Add(el.code, el);
                    }
                }
                return _Role;
            }
        }
    }
}
