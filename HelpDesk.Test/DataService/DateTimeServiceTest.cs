using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpDesk.DataService;
using System;

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
            DateTimeService s = new DateTimeService();
            
            DateTime currentDateTime = DateTime.Parse("04.10.2017 11:20");
            int countHour           = 8;
            int? startWorkDay       = 9;
            int? endWorkDay         = 18;
            int? startLunchBreak    = 13;
            int? endLunchBreak      = 14;

            //до обеда
            DateTime dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 11:20"));

            countHour = 14;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("06.10.2017 09:20"));

            countHour = 13;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 17:20"));


            //после обеда до окончания рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 14:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("04.10.2017 17:20"));

            countHour = 12;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("06.10.2017 09:20"));

            currentDateTime = DateTime.Parse("04.10.2017 17:20");
            countHour = 15;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("06.10.2017 16:20"));


            //до рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 08:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("04.10.2017 12:00"));

            countHour = 12;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 13:00"));

            countHour = 13;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 15:00"));

            countHour = 15;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 17:00"));



            //после рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 19:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 12:00"));

            countHour = 4;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 13:00"));

            countHour = 6;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 16:00"));


        }

        [TestMethod]
        public void DateTimeService_GetRequestDateEnd_WithOutLunch()
        {
            DateTimeService s = new DateTimeService();

            DateTime currentDateTime = DateTime.Parse("04.10.2017 11:20");
            int countHour = 8;
            int? startWorkDay = 9;
            int? endWorkDay = 18;
            int? startLunchBreak = null;
            int? endLunchBreak = null;

            //до окончания рабочего дня
            DateTime dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 10:20"));

            countHour = 14;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 16:20"));

            countHour = 13;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 15:20"));
                        
            currentDateTime = DateTime.Parse("04.10.2017 14:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("04.10.2017 17:20"));

            countHour = 12;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 17:20"));

            currentDateTime = DateTime.Parse("04.10.2017 17:20");
            countHour = 15;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("06.10.2017 14:20"));


            //до рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 08:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("04.10.2017 12:00"));

            countHour = 12;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 12:00"));

            countHour = 13;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 13:00"));

            countHour = 15;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 15:00"));
            
            //после рабочего дня
            currentDateTime = DateTime.Parse("04.10.2017 19:20");
            countHour = 3;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 12:00"));

            countHour = 4;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 13:00"));

            countHour = 6;
            dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 15:00"));

        }

        [TestMethod]
        public void DateTimeService_GetRequestDateEnd_IrregularWorkingHours()
        {
            DateTimeService s = new DateTimeService();

            DateTime currentDateTime = DateTime.Parse("04.10.2017 11:20");
            int countHour = 17;
            int? startWorkDay = null;
            int? endWorkDay = null; 
            int? startLunchBreak = null;
            int? endLunchBreak = null;

            DateTime dateEnd = s.GetRequestDateEnd(currentDateTime, countHour,
                startWorkDay, endWorkDay, startLunchBreak, endLunchBreak);
            Assert.AreEqual(dateEnd, DateTime.Parse("05.10.2017 04:20"));

        }
    }
}
