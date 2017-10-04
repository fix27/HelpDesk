using HelpDesk.DataService.Interface;
using HelpDesk.DataService.DTO;
using System;
using System.Collections.Generic;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с датой, временем и т.п.
    /// </summary>
    public class DateTimeService : BaseDataService, IDateTimeService
    {
        public DateTime GetCurrent()
        {
            return DateTime.Now;
        }

        public IList<Month> GetListMonth()
        {
            return new List<Month>()
            {
                new Month() { Ord = 1, Name = "Январь" },
                new Month() { Ord = 2, Name = "Февраль" },
                new Month() { Ord = 3, Name = "Март" },
                new Month() { Ord = 4, Name = "Апрель" },
                new Month() { Ord = 5, Name = "Май" },
                new Month() { Ord = 6, Name = "Июнь" },
                new Month() { Ord = 7, Name = "Июль" },
                new Month() { Ord = 8, Name = "Август" },
                new Month() { Ord = 9, Name = "Сентябрь" },
                new Month() { Ord = 10, Name = "Октябрь" },
                new Month() { Ord = 11, Name = "Ноябрь" },
                new Month() { Ord = 12, Name = "Декабрь" }
            };
        }

        /// <summary>
        /// Возвращает дату окончания срока по заявке в зависимости от системных настроек
        /// </summary>
        /// <param name="currentDateTime">Текущая дата</param>
        /// <param name="countHour">Количество часов на выполнение работ</param>
        /// <param name="startWorkDay">Час начала рабочего дня</param>
        /// <param name="endWorkDay">Час окончания рабочего дня</param>
        /// <param name="startLunchBreak">Час начала обеденного перерыва</param>
        /// <param name="endLunchBreak">Час окончания обеденного перерыва</param>
        /// <returns>Дата окончания срока по заявке</returns>
        public DateTime GetRequestDateEnd(DateTime currentDateTime, int countHour, 
            int? startWorkDay, int? endWorkDay, int? startLunchBreak, int? endLunchBreak)
        {
            if (!startWorkDay.HasValue || !endWorkDay.HasValue || 
                startWorkDay.HasValue && endWorkDay.HasValue && startWorkDay >= endWorkDay)
                return currentDateTime.AddHours(countHour);

            bool hasLunch = startLunchBreak.HasValue && endLunchBreak.HasValue && endLunchBreak > startLunchBreak;
            int countDailyWorkHour = endWorkDay.Value - startWorkDay.Value
                - (hasLunch? endLunchBreak.Value - startLunchBreak.Value : 0);
            int modHour = countHour % countDailyWorkHour;
            int countDay = countHour / countDailyWorkHour;
            DateTime requestDateEnd = currentDateTime.AddDays(countDay);
            

            if (requestDateEnd.Hour < startWorkDay) //до начала рабочего дня
            {
                requestDateEnd = requestDateEnd.Date.AddHours(startWorkDay.Value + modHour);
                if (hasLunch)
                {
                    if (requestDateEnd.Hour <= startLunchBreak)
                        return requestDateEnd;/**/
                    else
                    {
                        requestDateEnd = requestDateEnd.AddHours(endLunchBreak.Value - startLunchBreak.Value);
                        return requestDateEnd;/**/
                    }
                }
                else
                    return requestDateEnd;

            }
            else if (hasLunch && requestDateEnd.Hour < startLunchBreak)  //до обеда
            {
                requestDateEnd = requestDateEnd.AddHours(modHour);
                if (requestDateEnd.Hour < startLunchBreak)
                    return requestDateEnd;/**/
                else
                {
                    requestDateEnd = requestDateEnd.AddHours(endLunchBreak.Value - startLunchBreak.Value);
                    if (requestDateEnd.Hour < endWorkDay.Value)  //до конца рабочего дня
                        return requestDateEnd;/**/
                    else
                        return requestDateEnd/**/
                                .Date
                                .AddDays(1)
                                .AddHours(startWorkDay.Value + requestDateEnd.Hour - endWorkDay.Value)
                                .AddMinutes(requestDateEnd.Minute);
                }
            }
            else if (requestDateEnd.Hour < endWorkDay)  //до конца рабочего дня
            {
                requestDateEnd = requestDateEnd.AddHours(modHour);
                if (requestDateEnd.Hour > 0 && requestDateEnd.Hour < endWorkDay.Value)  //до конца рабочего дня
                {
                    return requestDateEnd;/**/
                }
                else
                {
                    requestDateEnd = requestDateEnd
                                .Date
                                .AddDays(1)
                                .AddHours(startWorkDay.Value + requestDateEnd.Hour - endWorkDay.Value)
                                .AddMinutes(requestDateEnd.Minute);

                    if (hasLunch)
                    {
                        if (requestDateEnd.Hour < startLunchBreak)
                            return requestDateEnd;/**/
                        else
                        {
                            requestDateEnd = requestDateEnd.AddHours(endLunchBreak.Value - startLunchBreak.Value);
                            return requestDateEnd;/**/
                        }
                    }
                    else
                        return requestDateEnd;

                }
            }
            else //после окончания рабочего дня
            {
                requestDateEnd = requestDateEnd.Date.AddDays(1).AddHours(startWorkDay.Value + modHour);
                if (hasLunch)
                {
                    if (requestDateEnd.Hour <= startLunchBreak)
                        return requestDateEnd;/**/
                    else
                    {
                        requestDateEnd = requestDateEnd.AddHours(endLunchBreak.Value - startLunchBreak.Value);
                        return requestDateEnd;/**/
                    }
                }
                else
                    return requestDateEnd;
            }


        }
    }
}
