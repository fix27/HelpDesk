using System;
using System.Collections.Generic;

namespace HelpDesk.DataService.Common
{
    public class DataServiceException : Exception
    {
        public DataServiceInfo DataServiceExceptionData { get; private set; }
        
        public DataServiceException()
            : base()
        { }

        public DataServiceException(string message)
            : base(message)
        {
            DataServiceExceptionData = new DataServiceInfo();
            DataServiceExceptionData.GeneralMessage = message;
        }


        public DataServiceException(string message, List<object> extensions)
            : base(message)
        {
            DataServiceExceptionData = new DataServiceInfo();

            DataServiceExceptionData.GeneralMessage = message;
            DataServiceExceptionData.Extensions = extensions;
        }


        public DataServiceException(string message, Dictionary<string, List<string>> messages, List<object> extensions = null)
            : base(message)
        {
            DataServiceExceptionData = new DataServiceInfo();

            DataServiceExceptionData.GeneralMessage = message;
            DataServiceExceptionData.Messages = messages;
            DataServiceExceptionData.Extensions = extensions;
        }

        public DataServiceException(string message, Dictionary<string, List<string>> messages, Dictionary<int, Dictionary<string, List<string>>> indexMessages, List<object> extensions = null)
            : base(message)
        {
            DataServiceExceptionData = new DataServiceInfo();

            DataServiceExceptionData.GeneralMessage = message;
            DataServiceExceptionData.Messages = messages;
            DataServiceExceptionData.IndexMessages = indexMessages;
            DataServiceExceptionData.Extensions = extensions;
        }

        public DataServiceException(string message, Dictionary<int, Dictionary<string, List<string>>> indexMessages, List<object> extensions = null)
            : base(message)
        {
            DataServiceExceptionData = new DataServiceInfo();

            DataServiceExceptionData.GeneralMessage = message;
            DataServiceExceptionData.IndexMessages = indexMessages;
            DataServiceExceptionData.Extensions = extensions;
        }
    }
}
