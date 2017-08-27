using System.Collections.Generic;

namespace HelpDesk.DataService.Common
{
    public class DataServiceWarning
    {
        public DataServiceInfo DataServiceWarningData { get; private set; }

        public DataServiceWarning() { }
        public DataServiceWarning(string message)
        {
            DataServiceWarningData = new DataServiceInfo();
            DataServiceWarningData.GeneralMessage = message;
        }


        public DataServiceWarning(string message, List<object> extensions)
        {
            DataServiceWarningData = new DataServiceInfo();

            DataServiceWarningData.GeneralMessage = message;
            DataServiceWarningData.Extensions = extensions;
        }


        public DataServiceWarning(string message, Dictionary<string, List<string>> messages, List<object> extensions = null)
        {
            DataServiceWarningData = new DataServiceInfo();

            DataServiceWarningData.GeneralMessage = message;
            DataServiceWarningData.Messages = messages;
            DataServiceWarningData.Extensions = extensions;
        }

        public DataServiceWarning(string message, Dictionary<string, List<string>> messages, Dictionary<int, Dictionary<string, List<string>>> indexMessages, List<object> extensions = null)
        {
            DataServiceWarningData = new DataServiceInfo();

            DataServiceWarningData.GeneralMessage = message;
            DataServiceWarningData.Messages = messages;
            DataServiceWarningData.IndexMessages = indexMessages;
            DataServiceWarningData.Extensions = extensions;
        }

        public DataServiceWarning(string message, Dictionary<int, Dictionary<string, List<string>>> indexMessages, List<object> extensions = null)
        {
            DataServiceWarningData = new DataServiceInfo();

            DataServiceWarningData.GeneralMessage = message;
            DataServiceWarningData.IndexMessages = indexMessages;
            DataServiceWarningData.Extensions = extensions;
        }
    }
}
