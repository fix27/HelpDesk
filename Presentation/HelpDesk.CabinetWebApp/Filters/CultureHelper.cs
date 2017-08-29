using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Linq;

namespace HelpDesk.CabinetWebApp.Filters
{
    public static class CultureHelper
    {
        public static void SetCulture(IDictionary<string, object> values, ref string cultureName)
        {
            cultureName = "ru";
            if (values!=null && values.Any() && values["lang"] != null && values["lang"].ToString() != "null")
                cultureName = values["lang"].ToString();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
            
        }
    }
}