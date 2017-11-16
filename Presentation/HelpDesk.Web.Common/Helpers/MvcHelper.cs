using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HelpDesk.Web.Common.Helpers
{
    public static class MvcHelper
    {
        public static IHtmlString ToJson(this HtmlHelper htmlHelper, object obj, bool addQuotes = false, bool encode = true)
        {
            var json = JsonConvert.SerializeObject(obj, new JavaScriptDateTimeConverter());
            if (addQuotes)
            {
                json = string.Concat("'", json, "'");
            }

            if (!encode)
            {
                json = htmlHelper.Encode(json);
            }

            return new HtmlString(json);
        }

        public static IHtmlString reCaptcha(this HtmlHelper helper)
        {
            string cultureName = Thread.CurrentThread.CurrentCulture.Name;
            StringBuilder sb = new StringBuilder();
            string publickey = WebConfigurationManager.AppSettings["RecaptchaPublicKey"];
            sb.AppendLine("<script type=\"text/javascript\" src='https://www.google.com/recaptcha/api.js?hl=" + cultureName + "'></script>");
            sb.AppendLine("");
            sb.AppendLine("<div class=\"g-recaptcha\" data-sitekey=\"" + publickey + "\"></div>");
            return MvcHtmlString.Create(sb.ToString());

        }
    }
}