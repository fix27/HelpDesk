using HelpDesk.DataService.Interface;
using HelpDesk.DataService.DTO;
using System;
using System.Linq;
using System.Collections.Generic;
using HelpDesk.DataService.Resources;
using HelpDesk.Entity;
using HelpDesk.Data.Repository;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с датой, временем и т.п.
    /// </summary>
    public class DateTimeService : BaseDataService, IDateTimeService
    {
        
        private readonly IBaseRepository<WorkCalendarItem> workCalendarItemRepository;
        private readonly IBaseRepository<WorkScheduleItem> workScheduleItemRepository;
        private readonly ISettingsRepository settingsRepository;

        public DateTimeService(IBaseRepository<WorkCalendarItem> workCalendarItemRepository,
            IBaseRepository<WorkScheduleItem> workScheduleItemRepository,
            ISettingsRepository settingsRepository)
        {
            this.workCalendarItemRepository = workCalendarItemRepository;
            this.workScheduleItemRepository = workScheduleItemRepository;
            this.settingsRepository = settingsRepository;
        }


        public DateTime GetCurrent()
        {
            return DateTime.Now;
        }

        public IList<Month> GetListMonth()
        {
            return new List<Month>()
            {
                new Month() { Ord = 1,  Name = Resource.January },
                new Month() { Ord = 2,  Name = Resource.February },
                new Month() { Ord = 3,  Name = Resource.March },
                new Month() { Ord = 4,  Name = Resource.April },
                new Month() { Ord = 5,  Name = Resource.May },
                new Month() { Ord = 6,  Name = Resource.June },
                new Month() { Ord = 7,  Name = Resource.July },
                new Month() { Ord = 8,  Name = Resource.August },
                new Month() { Ord = 9,  Name = Resource.September },
                new Month() { Ord = 10, Name = Resource.October },
                new Month() { Ord = 11, Name = Resource.November },
                new Month() { Ord = 12, Name = Resource.December }
            };
        }

        /// <summary>
        /// Возвращает дату окончания срока по заявке в зависимости от системных настроек
        /// </summary>
        /// <param name="currentDateTime">Текущая дата</param>
        /// <param name="countHour">Количество часов на выполнение работ</param>
        /// <returns>Дата окончания срока по заявке</returns>
        public DateTime GetRequestDateEnd(DateTime currentDateTime, int countHour)
        {
            Settings settings = settingsRepository.Get();

            if (!settings.StartWorkDay.HasValue || !settings.EndWorkDay.HasValue || 
                settings.StartWorkDay.HasValue && settings.EndWorkDay.HasValue && settings.StartWorkDay >= settings.EndWorkDay)
                return currentDateTime.AddHours(countHour);

            bool hasLunch = settings.StartLunchBreak.HasValue && settings.EndLunchBreak.HasValue && settings.EndLunchBreak > settings.StartLunchBreak;
            int countDailyWorkHour = settings.EndWorkDay.Value - settings.StartWorkDay.Value
                - (hasLunch? settings.EndLunchBreak.Value - settings.StartLunchBreak.Value : 0);
            int modHour = countHour % countDailyWorkHour;
            int countDay = countHour / countDailyWorkHour;
            DateTime requestDateEnd = currentDateTime.AddDays(countDay);
            

            if (requestDateEnd.Hour < settings.StartWorkDay) //до начала рабочего дня
            {
                requestDateEnd = requestDateEnd.Date.AddHours(settings.StartWorkDay.Value + modHour);
                if (hasLunch && requestDateEnd.Hour > settings.StartLunchBreak)
                    requestDateEnd = requestDateEnd.AddHours(settings.EndLunchBreak.Value - settings.StartLunchBreak.Value);/**/

            }
            else if (hasLunch && requestDateEnd.Hour < settings.StartLunchBreak)  //до обеда
            {
                requestDateEnd = requestDateEnd.AddHours(modHour);
                if (requestDateEnd.Hour >= settings.StartLunchBreak)
                {
                    requestDateEnd = requestDateEnd
                        .AddHours(settings.EndLunchBreak.Value - settings.StartLunchBreak.Value);
                    if (requestDateEnd.Hour >= settings.EndWorkDay.Value)  
                          requestDateEnd = requestDateEnd/**/
                                .Date
                                .AddDays(1)
                                .AddHours(settings.StartWorkDay.Value + requestDateEnd.Hour - settings.EndWorkDay.Value)
                                .AddMinutes(requestDateEnd.Minute);
                }
            }
            else if (requestDateEnd.Hour < settings.EndWorkDay)  //до окончания рабочего дня
            {
                requestDateEnd = requestDateEnd.AddHours(modHour);
                if (!(requestDateEnd.Hour > 0 && requestDateEnd.Hour < settings.EndWorkDay.Value))  
                {
                    requestDateEnd = requestDateEnd
                                .Date
                                .AddDays(1)
                                .AddHours(settings.StartWorkDay.Value + requestDateEnd.Hour - settings.EndWorkDay.Value)
                                .AddMinutes(requestDateEnd.Minute);

                    if (hasLunch && requestDateEnd.Hour >= settings.StartLunchBreak)
                        requestDateEnd = requestDateEnd.AddHours(settings.EndLunchBreak.Value - settings.StartLunchBreak.Value); /**/

                }
            }
            else //после окончания рабочего дня
            {
                requestDateEnd = requestDateEnd
                    .Date
                    .AddDays(1)
                    .AddHours(settings.StartWorkDay.Value + modHour);

                if (hasLunch && requestDateEnd.Hour > settings.StartLunchBreak)
                    requestDateEnd = requestDateEnd.AddHours(settings.EndLunchBreak.Value - settings.StartLunchBreak.Value);/**/

            }

            IEnumerable<WorkCalendarItem> workCalendarItems =
                workCalendarItemRepository
                .GetList(t => t.Date.Year == requestDateEnd.Year)
                .OrderBy(t => t.Date)
                .ToList();

            DayOfWeek dayOfWeek = requestDateEnd.DayOfWeek;
                        
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                //вдруг это рабочий день?
                if(workCalendarItems!=null && workCalendarItems.Count(t => t.Date.Date == requestDateEnd.Date && t.TypeItem == TypeWorkCalendarItem.Work) > 0)
                    return requestDateEnd;
            }

            //корректировка на праздники и сб вс
            WorkCalendarItem item = null;
            while (true)
            {
                item = workCalendarItems
                    .FirstOrDefault(t => t.Date.Date == requestDateEnd.Date && t.TypeItem == TypeWorkCalendarItem.Holiday);
                dayOfWeek = requestDateEnd.DayOfWeek;
                if (item != null)
                    requestDateEnd = item.Date.AddHours(requestDateEnd.Hour).AddMinutes(requestDateEnd.Minute);
                else if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                    requestDateEnd = requestDateEnd.AddDays(1);
                else
                    break;
            }

            return requestDateEnd;
            
        }
    }
}
