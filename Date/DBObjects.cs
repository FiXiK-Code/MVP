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
                        code = "31/18 - ТСП",
                        shortName = "Департамент. РЕК ул. Щербакова от ул. 2-я Луговая до ул. Дружбы",
                        dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
                        plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
                        actualFinishDate = new DateTime(2023),
                        history = null,
                        archive = "Нет",
                    },

new Project
{
    code = "14/19 - ТСП",
    shortName = "Брусника. СТР улиц в ЖЗ (Газовиков-Профсоюзная). Коррективы.",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},



new Project
{
    code = "27/19 - ТСП",
    shortName = "Департамент. СТР ад в жр Березняковский",
    priority = 1,
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},



new Project
{
    code = "10/20 - ТСП",
    shortName = "Н. Уренгой. РЕК.  ул. Геологоразведчиков",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "12/20 - ТСП",
    shortName = "ГКУ ТО «УАД». СТР жб моста чз р. Барсук (Вик. р-он)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "15/20 - ТСП",
    shortName = "Департамент. КОРР. ПД «СТР ад по ул. Стартовая».",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "16/20 - ТСП",
    shortName = "Департамент. КОРР. ПД «СТР ад в д. Казарово»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},



new Project
{
    code = "18/20 - ТСП",
    shortName = "Департамент. ПД «КРЕМ ул. Ставропольская»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Да",
},

new Project
{
    code = "19/20 - ТСП",
    shortName = "Департамент. ПД «КРЕМ ул. Ялуторовская»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "24/20 - ТСП",
    shortName = "ДКС Н. Уренгой КОРР ПСД «РЕК пр. Губкина»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "27/20 - ТСП",
    shortName = "ООО «Уренгойдорстрой». Н. Уренгой КОРР ПСД «РЕК пр. Губкина»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "32/20 - ТСП",
    shortName = "СОГУ. РЕК ад Володинское- Ек-Тюмень",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "34/20 - ТСП",
    shortName = "СОГУ. Рек ад ЕКБ - Реж (остановка)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "40/20 - ТСП",
    shortName = "ГКУ СО «УАД». РЕК ад подъезд к с.Коменки",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "41/20 - ТСП",
    shortName = "ООО «Брусника». Р-ка транс-пеш концепц. ул. Профсоюзная и ул. Госпаровская",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},
new Project
{
    code = "00/00 - ТСП",
    shortName = "Прочее",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "45/20 - ТСП",
    shortName = "ГКУ СО «УАД». РЕК ад д. Кокуй – д. Серкова ТГО",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "46/20 - ТСП",
    shortName = "Департамент. ПД «СТР ул. в р-не Комаровский 1 очередь»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "47/20 - ТСП",
    shortName = "Департамент. ПД «СТР ул. в р-не Комаровский 2 очередь»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "01/21 - ТСП",
    shortName = "Лабытнанги. РЕК. ул. Ленина",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "02/21 - ТСП",
    shortName = "Лабытнанги. СТР. ул. Ленинградская",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "03/21 - ТСП",
    shortName = "Лабытнанги. СТР. проезда от ул. Дзержинского-ул. Ленинградская",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "04/21 - ТСП",
    shortName = "Лабытнанги. РЕК. ул. Мира",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "05/21 - ТСП",
    shortName = "Лабытнанги. РЕК. ул. Новая",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "06/21 - ТСП",
    shortName = "Лабытнанги. СТР. вп в квартале 01.02.07",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "16/21 - ТСП",
    shortName = "ПСД «СТР УДС в р-не оз. Алебашево»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "19/21 - ТСП",
    shortName = "СЗ Тюм. р-на. СТР подъезда к ФАД",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},



new Project
{
    code = "27/21 - ТСП",
    shortName = "ГКУ ТО УАД. СТР ад Обход д. Ожогина. Корректировка",
    priority = 1,
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},




new Project
{
    code = "28/21 - ТСП",
    shortName = "Ставрополь. ПП развязка на 63 км",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "29/21 - ТСП",
    shortName = "УКС г. Нижневартовска. Изменения в ПД «Проезд Восточный»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "31/22 - ТСП",
    shortName = "СОГУ. СТР пп чз жд на км 37 ад Богданович-Покровское (КОРРЕКТИРОВКА)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "32/21 - ТСП",
    shortName = "Автомобильные дороги пгт. Пангоды (ПИР)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "33/21 - ТСП",
    shortName = "СОГУ. РЕК мп чз р. Пышму",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "36/21 - ТСП",
    shortName = "Сургут. Ул. Киртбая",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "37/21 - ТСП",
    shortName = "Навигатор. Девелопмент. СТР. дорог в жк по ул. В. Полякова. 5-6-й этап — ПД,РД",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Да",
},

new Project
{
    code = "38/21 - ТСП",
    shortName = "Лабытнанги. ИО участок 01_01_01",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "39/21 - ТСП",
    shortName = "Н. Уренгой. КРЕМ проездов ул. Пушкинская",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет",
},

new Project
{
    code = "41/21 - ТСП",
    shortName = "Нефтеюганск. АН. ИО вдоль ул. Нефтяников",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Да",
},

new Project
{
    code = "42/21 - ТСП",
    shortName = "Нефтеюганск. АН. ИО 4 мкр",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Да",
},





////////
new Project
{
    code = "01/22 - ТСП", 
shortName = "Лабытнанги. Биатлонный комплекс",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "50/21 - ТСП", 
shortName = "ООО «Сибинтел-Холдинг». КРЕМ ул. Даудельная",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    priority = 2,
    archive = "Нет"
},

new Project
{
    code = "49/21 - ТСП", 
shortName = "ООО «Сибинтел-Холдинг». СТР ул. Софьи Ковалевской",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    priority = 2,
    archive = "Нет"
},

new Project
{
    code = "48/21 - ТСП", 
shortName = "ООО «Сибинтел-Холдинг». КРЕМ ул. Станкостроителей",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    priority = 1,
    archive = "Нет"
},

new Project
{
    code = "45/21 - ТСП", 
shortName = "Н. Девелопмент. СТР. дорог в жк по ул. В. Полякова, 4 этап — корр. 2",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Да"
},

new Project
{
    code = "43/21 - ТСП", 
shortName = "Тюмень. УАДТО. СТР ТПУ с. Щетково",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "37/22 - ТСП", 
shortName = "ООО «СК «РЕАЛИСТ». Разработка РД «СТР ад в д. Казарово»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "36/22 - ТСП", 
shortName = "СЗ Тюм. р-на. СТР ад Ожогина-Патрушева",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "35/22 - ТСП", 
shortName = "ТОДЭП. Разработка РД ул. А. Бушуева",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "34/22 - ТСП", 
shortName = "Н. Уренгой. РЕК ул. Подшибякина",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "33/22 - ТСП", 
shortName = "Н. Уренгой. РЕК ул. И. Подовжнего",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "32/22 - ТСП", 
shortName = "ООО «СЗ Зеленые аллеи». Светофоры",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "31/22 - ТСП", 
shortName = "СОГУ. СТР пп чз жд на км 37 ад Богданович-Покровское (КОРРЕКТИРОВКА)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "30/22 - ТСП", 
shortName = "Салехард. ИО застройки правого берега р. Шайтанка ",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "29/22 - ТСП", 
shortName = "Н. Уренгой. 2 моста Западная магистраль",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "27/22 - ТСП", 
shortName = "ПД на устройство ККС (Обход д. Ожогина)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "26/22 - ТСП", 
shortName = "Н. Уренгой. КРЕМ ул. 26 съезда КПСС",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "25/22 - ТСП", 
shortName = "Н. Уренгой. КРЕМ проездов мкр. Советский",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "24/22 - ТСП", 
shortName = "РД «СТР ул. Розы Трениной»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "23/22 - ТСП", 
shortName = "Н. Уренгой. 2 моста Центральная магистраль",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "22/22 - ТСП", 
shortName = "ООО «Бридж». Изменения в РД пп чз жд на км 37 ад Богданович-Покровское",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "21/22 - ТСП", 
shortName = "СОГУ. К. РЕМ. ад Северский-Полевской (КОРРЕКТИРОВКА)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "20/22 - ТСП", 
shortName = "Н. Уренгой «Реконструкция проспекта Губкина» (АН)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "19/22 - ТСП", 
shortName = "Ишим, племзавод Юбилейный. Подъездная дорога",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "18/22 - ТСП", 
shortName = "АН «Улица Нововартовская. 2 этап»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Да"
},

new Project
{
    code = "17/22 - ТСП", 
shortName = "ООО «Валла-Тунтури». СТР ад от примык. к ад Р-21 «Кола»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    priority = 1,
    archive = "Нет"
},

new Project
{
    code = "14/22 - ТСП", 
shortName = "Н. Уренгой. РЕК. ул. Новая",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "12/22 - ТСП", 
shortName = "ТОДЭП. Разработка РД ул. Т. Кармацкого",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "11/22 - ТСП", 
shortName = "СОГУ. СТР пп чз жд на км 37 ад Богданович-Покровское (АН)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "10/22 - ТСП", 
shortName = "СОГУ. СТР пп чз жд на км 12 ад Глубокое-Бобровский (АН)",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "09/22 - ТСП", 
shortName = "Н. Уренгой. РЕК ул. Железнодорожной",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "08/22 - ТСП", 
shortName = "Тобольск. СТР дорог в мкр. Усадьба",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "07/22 - ТСП", 
shortName = "Тобольск. РЕК ул. Алябьева",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "05/22 - ТСП", 
shortName = "Тобольск. СТР дорог в 19 мкр",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
},

new Project
{
    code = "04/22 - ТСП", 
shortName = "Департамент. ПД «КРЕМ ад от ул. Авторемонтная до ул. Ямская»",
    dateStart = new DateTime(2023, 1, 14, 15, 0, 0),
    plannedFinishDate = new DateTime(2023, 01, 25, 15, 0, 0),
    actualFinishDate = new DateTime(2023),
    history = null,
    archive = "Нет"
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
                        date = new DateTime(2022, 8, 24, 0, 0, 0),
                        projectCode = "00/00 - ТСП",
                        desc = "Ответить на письмо Югорску о повторной экспертизе",
                        supervisor = "Мочалов Александр Николаевич",
                        plannedTime = new TimeSpan(0, 15, 0),
                        actualTime = new TimeSpan(0, 15, 0),
                        comment = null,
                        start = new DateTime(2022, 12, 16, 15, 0, 0),

                        finish = new DateTime(2023),
                        status = "Создана",
                        liteTask = false
                    },

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Направил Сафронову откорректированный отчет по геодезии",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    priority = 1,
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "37/20 - ТСП",
    desc = "до Моргачевой не смог дозвониться, написал ей на почту что бы она описала, что нужно сделать",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 10, 0),
    actualTime = new TimeSpan(0, 10, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "08/19 - ТСП",
    desc = "Запросил и переслал Пушкину ответ Шаповалова по ходу работ Платона",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 5, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "48/21 - ТСП",
    desc = "Направил отчеты ИИ в дирекцию (просила Богданова) (48/21, 49/21, 50/21)",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 5, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "08/19 - ТСП",
    desc = "Получил у Шаповалова и направил Пушкину разъяснения по срокам",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 10, 0),
    actualTime = new TimeSpan(0, 10, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Письмо о приостановке по разработке плана трассы (неофициально)",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 40, 0),
    actualTime = new TimeSpan(0, 40, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "41/21 - ТСП",
    desc = "Разговор с Востриковым, АН откладывается на конец сентября",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 10, 0),
    actualTime = new TimeSpan(0, 10, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "48/21 - ТСП",
    desc = "Анализ ТС по станкостроителей",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 30, 0),
    actualTime = new TimeSpan(0, 30, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Написать о приостановке на срок предоставления изысканий",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 20, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "29/21 - ТСП",
    desc = "Подписать отзыв у Кашкаровой",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 5, 0),
    actualTime = new TimeSpan(0, 10, 0),
    comment = "Отправил ей конфеты)",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "48/21 - ТСП",
    desc = "Заручиться поддержкой Николаенко по ТС",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 5, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 8, 24, 0, 0, 0),
    projectCode = "51/21 - ТСП",
    desc = "Подключить к решению вопроса по ДК Николаенко",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},


/////////////////
new Task
{
    date = new DateTime(2022, 9, 1, 0, 0, 0),
    projectCode = "29/17 - ТСП",
    desc = "Подготовил и направил ответ на замечания управы ЦАО",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 40, 0),
    actualTime = new TimeSpan(0, 160, 0),
    comment = "Анализ большого количества данных, согласование формулировок с директором",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 6, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Изучение отчета по гидрологии",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 30, 0),
    actualTime = new TimeSpan(0, 90, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 6, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Изучение отчета по геологии",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 30, 0),
    actualTime = new TimeSpan(0, 30, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 6, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Изучение отчета по экологии",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 30, 0),
    actualTime = new TimeSpan(0, 30, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 7, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Запросить ГП в ред формате у ЖК Ожогино, переговорить по планируемым мероприятиям",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

//////////////
new Task
{
    date = new DateTime(2022, 9, 12, 0, 0, 0),
    projectCode = "18/20 - ТСП",
    desc = "Найти письмо №45-60-485/22 от 17.05.2022.",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 12, 0, 0, 0),
    projectCode = "18/22 - ТСП",
    desc = "Собрать Нововартовскую и отдать на ПДФ",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 12, 0, 0, 0),
    projectCode = "27/19 - ТСП",
    desc = "Гаврилова говорит (почта) что у нас косяк в диаметрах. Проверить",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 10, 0),
    actualTime = new TimeSpan(0, 10, 0),
    comment = "Отдал Хамитовой в работу",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 12, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Просмотреть и Направить на согласование ТКР ЭС",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 20, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 12, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Просмотреть и Направить на согласование ТКР ЭС УСТЭК",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 20, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

//////
new Task
{
    date = new DateTime(2022, 9, 13, 0, 0, 0),
    projectCode = "18/20 - ТСП",
    desc = "Просмотреть и Направить на согласование ТКР ЭС УСТЭК",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 40, 0),
    actualTime = new TimeSpan(0, 180, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 13, 0, 0, 0),
    projectCode = "27/19 - ТСП",
    desc = "Просмотреть и Направить на согласование ТКР ЭС УСТЭК",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 0, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 13, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Просмотреть и Направить на согласование ТКР ЭС УСТЭК",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 30, 0),
    actualTime = new TimeSpan(0, 30, 0),
    comment = "Чертеж отдал директору",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

/////////
new Task
{
    date = new DateTime(2022, 9, 14, 0, 0, 0),
    projectCode = "09/20 - ТСП",
    desc = "Ответить Булдаковой по опоре ЛЭП",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 15, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 14, 0, 0, 0),
    projectCode = "00/00 - ТСП",
    desc = "Направить Моргачевой карточку ПП",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 5, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 14, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Направить в Алмакс актуальные изыскания",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 14, 0, 0, 0),
    projectCode = "27/19 - ТСП",
    desc = "выезд на разведку в березняки",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 180, 0),
    actualTime = new TimeSpan(0, 180, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 14, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Сделать запрос в УАДО на стоимость изъятия участков",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 10, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

//////////////
new Task
{
    date = new DateTime(2022, 9, 15, 0, 0, 0),
    projectCode = "03/19 - ТСП",
    desc = "Ответ на внесение изменений в ПД",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 5, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = "Мы не против, изменяйте",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 15, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Прошевелить \"согласование\" ЖК Ожогино",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 15, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Ответить по съезду к автомойке",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 20, 0),
    comment = "Идите…..., вы далеко",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 16, 0, 0, 0),
    projectCode = "18/22 - ТСП",
    desc = "Подготовить подтверждение ИЗМ",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 40, 0),
    actualTime = new TimeSpan(0, 40, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 16, 0, 0, 0),
    projectCode = "48/21 - ТСП",
    desc = "Направить повторно на согласование трассу ТС",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 40, 0),
    actualTime = new TimeSpan(0, 40, 0),
    comment = "Копию направил Николаенко",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

////////
new Task
{
    date = new DateTime(2022, 9, 19, 0, 0, 0),
    projectCode = "00/00 - ТСП",
    desc = "Планерка",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 40, 0),
    actualTime = new TimeSpan(0, 90, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 19, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Выйти на связь с подпорщиками",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 5, 0),
    actualTime = new TimeSpan(0, 5, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 19, 0, 0, 0),
    projectCode = "27/19 - ТСП",
    desc = "Шахлина, Заводоуковская запросить ТП в ГОРМОСТЕ",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 25, 0),
    actualTime = new TimeSpan(0, 25, 0),
    comment = "Переговорил с Перепелицей, говорит от них подключиться реально",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 19, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Направить в ЗКЦ ПД",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 20, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Направить подпорщикам планы, вертикалку, геологию",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    comment = "Направил без вертикалки",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 20, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "Направить Перепелице данные для сборки тома 110 кВ",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

///////////
new Task
{
    date = new DateTime(2022, 9, 21, 0, 0, 0),
    projectCode = "08/19 - ТСП",
    desc = "Направить платон в ФУАД",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 20, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 21, 0, 0, 0),
    projectCode = "00/00 - ТСП",
    desc = "Выезд в Навигатор",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 60, 0),
    actualTime = new TimeSpan(0, 120, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 21, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "Направить материалы Домрачеву для начала проектирования моста",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 20, 0),
    comment = null,
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 21, 0, 0, 0),
    projectCode = "48/21 - ТСП",
    desc = "Вызвонить УСТЭК узнать о решениее по откорректированной  трассе ТС",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = "Переговорил 21.09, обещала дать ответ до 28.09",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

////////////
new Task
{
    date = new DateTime(2022, 9, 22, 0, 0, 0),
    projectCode = "27/22 - ТСП",
    desc = "Проверить томик по 110 кВ, что сделал перепелица",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 10, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 22, 0, 0, 0),
    projectCode = "27/19 - ТСП",
    desc = "Направить Полещуку планы рассы по всем улицам",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 10, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 22, 0, 0, 0),
    projectCode = "48/21 - ТСП",
    desc = "Разобраться с актом промера расстояний (Миргородская)",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 5, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 22, 0, 0, 0),
    projectCode = "09/20 - ТСП",
    desc = "Направить в СУЭС материалы для увязки ул Северная",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 20, 0),
    actualTime = new TimeSpan(0, 20, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 26, 0, 0, 0),
    projectCode = "48/21 - ТСП",
    desc = "Вызвонить УСТЭК узнать о решении по откорректированной  трассе ТС",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    comment = "Переговорил, ответил на ее вопросы",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 9, 27, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "В ПЗ по 110 кВ обязяательно прописать, что нет давления от транспорта на плиту",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 10, 0),
    actualTime = new TimeSpan(0, 10, 0),
    comment = "Переговорил 21.09, обещала дать ответ до 28.09",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

///////////
new Task
{
    date = new DateTime(2022, 10, 4, 0, 0, 0),
    projectCode = "17/22 - ТСП",
    desc = "В ПЗ по 110 кВ обязяательно прописать, что нет давления от транспорта на плиту",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 10, 4, 0, 0, 0),
    projectCode = "18/22 - ТСП",
    desc = "В ПЗ по 110 кВ обязяательно прописать, что нет давления от транспорта на плиту",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 5, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 10, 4, 0, 0, 0),
    projectCode = "27/21 - ТСП",
    desc = "В ПЗ по 110 кВ обязяательно прописать, что нет давления от транспорта на плиту",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 10, 0),
    actualTime = new TimeSpan(0, 5, 0),
    comment = "Света передала Секисовой",
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 10, 4, 0, 0, 0),
    projectCode = "48/21 - ТСП",
    desc = "В ПЗ по 110 кВ обязяательно прописать, что нет давления от транспорта на плиту",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 15, 0),
    actualTime = new TimeSpan(0, 15, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
},

new Task
{
    date = new DateTime(2022, 10, 6, 0, 0, 0),
    projectCode = "18/22 - ТСП",
    desc = "В ПЗ по 110 кВ обязяательно прописать, что нет давления от транспорта на плиту",
    supervisor = "Мочалов Александр Николаевич",
    plannedTime = new TimeSpan(0, 60, 0),
    actualTime = new TimeSpan(0, 10, 0),
    start = new DateTime(2022, 12, 16, 15, 0, 0),

    finish = new DateTime(2023),
    status = "Создана",
    liteTask = false
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
                        name = "Главный инженер проектов",
                        roleCod = "R02"
                    },
                    new Post
                    {
                        code = "P03",
                        name = "Помощник главного инженера проекта",
                        roleCod = "R03"
                    },
                    new Post
                    {
                        code = "P04",
                        name = "Начальник отдела проектирования",
                        roleCod = "R04"
                    },
                    new Post
                    {
                        code = "P05",
                        name = "Руководитель группы проектирования",
                        roleCod = "R05"
                    },
                    new Post
                    {
                        code = "P06",
                        name = "Ведущий инженер отдела проектирования",
                        roleCod = "R06"
                    },
                    new Post
                    {
                        code = "P07",
                        name = "Инженер – проектировщик 1 категории отдела проектирования",
                        roleCod = "R06"
                    },
                    new Post
                    {
                        code = "P08",
                        name = "Инженер – проектировщик 2 категории отдела проектирования",
                        roleCod = "R06"
                    },
                    new Post
                    {
                        code = "P09",
                        name = "Инженер – проектировщик 3 категории отдела проектирования",
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
                        new Staff {
code = "01",
name = "Григорьев Виктор Александрович",
divisionId = 1,
roleId = 5,
post = "Главный инженер проектов",
login = "ГригорьевВА",
passvord = "123456"
},

new Staff {
code = "02",
name = "Домрачев Алексей Юрьевич",
divisionId = 1,
roleId = 5,
post = "Главный инженер проектов",
login = "ДомрачевАЮ",
passvord = "123456"
},

new Staff {
code = "03",
name = "Мочалов Александр Николаевич",
divisionId = 1,
roleId = 5,
post = "Главный инженер проектов",
login = "МочаловАН",
passvord = "123456"
},

new Staff {
code = "04",
name = "Терехова Нина Михайловна",
divisionId = 1,
roleId = 5,
post = "Главный инженер проектов",
login = "ТереховаНМ",
passvord = "123456"
},

new Staff {
code = "05",
name = "Менщиков Андрей Игоревич",
divisionId = 1,
roleId = 6,
post = "Помощник главного инженера проекта",
login = "МенщиковАИ",
passvord = "123456"
},

new Staff {
code = "06",
name = "Файзуллин Давид Эдуардович",
divisionId = 1,
roleId = 6,
post = "Помощник главного инженера проекта",
login = "ФайзуллинДЭ",
passvord = "123456"
},

///////
new Staff {
code = "07",
name = "Мухортова Светлана Ивановна",
divisionId = 2,
roleId = 3,
post = "Начальник отдела проектирования",
login = "МухортоваСИ",
passvord = "123456"
},

new Staff {
code = "08",
name = "Лесина Ольга Сергеевна",
divisionId = 2,
roleId = 1,
post = "Руководитель группы проектирования",
login = "ЛесинаОС",
passvord = "123456"
},

new Staff {
code = "09",
name = "Хамитова Анна Федоровна",
divisionId = 2,
roleId = 2,
post = "Ведущий инженер отдела проектирования",
login = "ХамитоваАФ",
passvord = "123456"
},

new Staff {
code = "10",
name = "Власюк Анастасия Олеговна",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 2 категории отдела проектирования",
login = "ВласюкАО",
passvord = "123456"
},

new Staff {
code = "11",
name = "Сергеева Анастасия Андреевна",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 2 категории отдела проектирования",
login = "СергееваАИ",
passvord = "123456"
},

new Staff {
code = "12",
name = "Сидоров Артем Сергеевич",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 2 категории отдела проектирования",
login = "СидоровАС",
passvord = "123456"
},

/////
new Staff {
code = "13",
name = "Хамитова Анна Федоровна",
divisionId = 2,
roleId = 1,
post = "Руководитель группы проектирования",
login = "ХамитоваАФ",
passvord = "123456"
},

new Staff {
code = "14",
name = "Домрачева Наталья Николаевна",
divisionId = 2,
roleId = 2,
post = "Ведущий инженер отдела проектирования",
login = "ДомрачеваНН",
passvord = "123456"
},

new Staff {
code = "15",
name = "Хицунова Неонила Юрьевна",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 1 категории отдела проектирования",
login = "ХицуноваНЮ",
passvord = "123456"
},

new Staff {
code = "16",
name = "Кузнецова Инна Владимировна",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 1 категории отдела проектирования",
login = "КузнецоваИВ",
passvord = "123456"
},

//////
new Staff {
code = "17",
name = "Лялина Ксения Евгеньевна",
divisionId = 2,
roleId = 1,
post = "Руководитель группы проектирования",
login = "ЛялинаКЕ",
passvord = "123456"
},

new Staff {
code = "18",
name = "Тимофеев Георгий Павлович",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 1 категории отдела проектирования",
login = "ТимофеевГП",
passvord = "123456"
},

new Staff {
code = "19",
name = "Габайдулина Ильнара Итиясовна",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 2 категории отдела проектирования",
login = "ГабайдулинаИИ",
passvord = "123456"
},

new Staff {
code = "20",
name = "Третьякова Жанна Викторовна",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 2 категории отдела проектирования",
login = "ТретьяковаЖВ",
passvord = "123456"
},

new Staff {
code = "21",
name = "Колмогоров Максим Андреевич",
divisionId = 2,
roleId = 2,
post = "Инженер – проектировщик 3 категории отдела проектирования",
login = "КолмогоровМА",
passvord = "123456"
},
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
