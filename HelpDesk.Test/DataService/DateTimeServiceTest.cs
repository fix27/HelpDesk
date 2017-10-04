using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpDesk.DataService;
using System;
using Moq;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace HelpDesk.Test.DataService
{
    /// <summary>
    /// Тесты методов DateTimeServiceTest
    /// </summary>
    [TestClass]
    public class DateTimeServiceTest
    {
        
        [TestMethod]
        public void DateTimeService_GetRequestDateEnd_WithLunch()
        {
            Mock<IBaseRepository<WorkCalendarItem>> workCalendarItemRepository = new Mock<IBaseRepository<WorkCalendarItem>>(MockBehavior.Strict);
            workCalendarItemRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<WorkCalendarItem, bool>>>()))
               .Returns((Expression<Func<WorkCalendarItem, bool>> predicate) =>
               { return new List<WorkCalendarItem>().AsQueryable(); });


            Mock<IBaseRepository<WorkScheduleItem>> workScheduleItemRepository = new Mock<IBaseRepository<WorkScheduleItem>>(MockBehavior.Strict);
            workScheduleItemRepository.Setup(x => x.GetList())
               .Returns(() =>
               { return null; });

            Mock<ISettingsRepository> settingsRepository = new Mock<ISettingsRepository>(MockBehavior.Strict);
            settingsRepository.Setup(x => x.Get())
               .Returns(() =>
               {
                   return new Settings()
                   {
                        StartWorkDay    = 9,
                        EndWorkDay      = 18,
                        StartLunchBreak = 13,
                        EndLunchBreak   = 14
                    };
               });

            DateTimeService s = new DateTimeService(
                workCalendarItemRepository.Object, 
                workScheduleItemRepository.Object, 
                settingsRepository.Object);

            DateTime currentDateTime = DateTime.Parse("04.10.2017 11:20");
            int countHour           = 8;
            
            //до обеда
            DateTime dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 11:20"));

            countHour = 14;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("06.10.2017 09:20"));

            countHour = 13;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 17:20"));


            //после обеда до окончания рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 14:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("04.10.2017 17:20"));

            countHour = 12;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("06.10.2017 09:20"));

            currentDateTime = DateTime.Parse("04.10.2017 17:20");
            countHour = 15;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("06.10.2017 16:20"));


            //до рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 08:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("04.10.2017 12:00"));

            countHour = 12;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 13:00"));

            countHour = 13;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 15:00"));

            countHour = 15;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 17:00"));



            //после рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 22:40");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 12:00"));

            countHour = 4;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 13:00"));

            countHour = 6;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 16:00"));


        }

        [TestMethod]
        public void DateTimeService_GetRequestDateEnd_WithOutLunch()
        {
            Mock<IBaseRepository<WorkCalendarItem>> workCalendarItemRepository = new Mock<IBaseRepository<WorkCalendarItem>>(MockBehavior.Strict);
            workCalendarItemRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<WorkCalendarItem, bool>>>()))
               .Returns((Expression<Func<WorkCalendarItem, bool>> predicate) =>
               { return new List<WorkCalendarItem>().AsQueryable(); });


            Mock<IBaseRepository<WorkScheduleItem>> workScheduleItemRepository = new Mock<IBaseRepository<WorkScheduleItem>>(MockBehavior.Strict);
            workScheduleItemRepository.Setup(x => x.GetList())
               .Returns(() =>
               { return null; });

            Mock<ISettingsRepository> settingsRepository = new Mock<ISettingsRepository>(MockBehavior.Strict);
            settingsRepository.Setup(x => x.Get())
               .Returns(() =>
               {
                   return new Settings()
                   {
                       StartWorkDay = 9,
                       EndWorkDay = 18,
                       StartLunchBreak = null,
                       EndLunchBreak = null
                   };
               });

            DateTimeService s = new DateTimeService(
                workCalendarItemRepository.Object,
                workScheduleItemRepository.Object,
                settingsRepository.Object);

            DateTime currentDateTime = DateTime.Parse("04.10.2017 11:20");
            int countHour = 8;
            
            //до окончания рабочего дня
            DateTime dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 10:20"));

            countHour = 14;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 16:20"));

            countHour = 13;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 15:20"));
                        
            currentDateTime = DateTime.Parse("04.10.2017 14:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("04.10.2017 17:20"));

            countHour = 12;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 17:20"));

            currentDateTime = DateTime.Parse("04.10.2017 17:20");
            countHour = 24;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("09.10.2017 14:20"));


            //до рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 08:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("04.10.2017 12:00"));

            countHour = 12;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 12:00"));

            countHour = 13;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 13:00"));

            countHour = 15;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 15:00"));
            
            //после рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 19:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 12:00"));

            countHour = 4;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 13:00"));

            countHour = 6;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 15:00"));

        }

        [TestMethod]
        public void DateTimeService_GetRequestDateEnd_IrregularWorkingHours()
        {
            Mock<IBaseRepository<WorkCalendarItem>> workCalendarItemRepository = new Mock<IBaseRepository<WorkCalendarItem>>(MockBehavior.Strict);
            workCalendarItemRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<WorkCalendarItem, bool>>>()))
               .Returns((Expression<Func<WorkCalendarItem, bool>> predicate) =>
               { return null; });


            Mock<IBaseRepository<WorkScheduleItem>> workScheduleItemRepository = new Mock<IBaseRepository<WorkScheduleItem>>(MockBehavior.Strict);
            workScheduleItemRepository.Setup(x => x.GetList())
               .Returns(() =>
               { return null; });

            Mock<ISettingsRepository> settingsRepository = new Mock<ISettingsRepository>(MockBehavior.Strict);
            settingsRepository.Setup(x => x.Get())
               .Returns(() =>
               {
                   return new Settings()
                   {
                       StartWorkDay = null,
                       EndWorkDay = null,
                       StartLunchBreak = null,
                       EndLunchBreak = null
                   };
               });

            DateTime currentDateTime = DateTime.Parse("04.10.2017 11:20");
            int countHour = 17;

            DateTimeService s = new DateTimeService(
                workCalendarItemRepository.Object,
                workScheduleItemRepository.Object,
                settingsRepository.Object);

            DateTime dateEnd = s.GetRequestDateEnd(currentDateTime, countHour);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 04:20"));

        }
    }
}
